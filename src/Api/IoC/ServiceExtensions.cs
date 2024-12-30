using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;
using csharp_scalar.Warmup.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry.Logs;
using System.Diagnostics;
using csharp_scalar.Warmup.Settings;
using Features.Ambiente;
using Api.IoC;

namespace csharp_scalar.Warmup
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.ReportApiVersions = true; // Inclui as versões suportadas no cabeçalho da resposta
                config.AssumeDefaultVersionWhenUnspecified = true; // Define uma versão padrão se nenhuma for especificada
                config.DefaultApiVersion = new ApiVersion(1); // Versão padrão
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V"; // Formato: "v1", "v2", etc.
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        public static IServiceCollection AddOpenApiDoc(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var version in provider.ApiVersionDescriptions)
                services.AddOpenApi(version.GroupName, options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

            return services;
        }

        public static IServiceCollection AddRequestValidations(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<Ambiente>();

            return services;
        }

        public static IServiceCollection AddMediaTR(this IServiceCollection services)
        {
            string applicationAssemblyName = Assembly.GetExecutingAssembly().GetName().Name!;
            var assembly = AppDomain.CurrentDomain.Load(applicationAssemblyName);

            services.AddMediatR(cfg
                => cfg.RegisterServicesFromAssemblies(
                    typeof(Program).Assembly,
                    typeof(Ambiente).Assembly)
            );

            return services;
        }

        public static IServiceCollection AddApiProblemDetails(this IServiceCollection services)
        {
            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (ctx, ex) =>
                {
                    var env = ctx.RequestServices.GetRequiredService<IHostEnvironment>();
                    return env.IsDevelopment() || env.IsStaging();
                };

                options.MapExceptionToStatusCodeWithMessage<UnauthorizedAccessException>(StatusCodes.Status401Unauthorized);
                options.MapExceptionToStatusCodeWithMessage<ArgumentException>(StatusCodes.Status400BadRequest);
                options.MapExceptionToStatusCodeWithMessage<ArgumentNullException>(StatusCodes.Status400BadRequest);
                options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
                options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
                options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
            })
            .AddProblemDetailsConventions();

            return services;
        }

        public static void MapExceptionToStatusCodeWithMessage<TException>(
            this ProblemDetailsOptions options, int statusCode)
            where TException : Exception
        {
            options.Map<TException>(ex => new StatusCodeProblemDetails(statusCode)
            {
                Detail = ex.Message
            });
        }

        public static IServiceCollection AddApiControllers(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options
                => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            return services;
        }

        public static IServiceCollection AddAutoFac(this IServiceCollection services, ConfigureHostBuilder host)
        {
            host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            var containerBuilder = new ContainerBuilder();
            host.ConfigureContainer<ContainerBuilder>(builder =>
                builder.RegisterModule<ApplicationModule>());

            return services;
        }

        public static IServiceCollection AddTelemetry(this IServiceCollection services, ILoggingBuilder logging, IConfiguration configuration)
        {
            var config = configuration.GetSection(nameof(Configurations)).Get<Configurations>()!;

            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            var serviceName = assemblyName.Name!;
            var serviceVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString()!;

            var activity = new ActivitySource(serviceName);
            services.AddSingleton(activity);

            void configureResource(ResourceBuilder r) => r.AddService(
                serviceName: serviceName,
                serviceVersion: serviceVersion,
                serviceInstanceId: Environment.MachineName);

            services
                .AddOpenTelemetry()
                .ConfigureResource(configureResource)
                .WithTracing(tracing =>
                {
                    tracing
                        .AddSource(serviceName)
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddConsoleExporter()
                        .AddOtlpExporter(opt => opt.Endpoint = new Uri(config.Telemetry.Exporter));
                })
                .WithMetrics(metrics =>
                {
                    metrics
                        .AddAspNetCoreInstrumentation()
                        // .AddMeter(greeterMeter.Name)
                        .AddMeter("Microsoft.AspNetCore.Hosting")
                        .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                        .AddOtlpExporter(opt => opt.Endpoint = new Uri(config.Telemetry.Exporter));
                });

            logging.AddOpenTelemetry(logging =>
            {
                logging.ParseStateValues = true;
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
                logging.AddOtlpExporter(opt => opt.Endpoint = new Uri(config.Telemetry.Exporter));
            });

            return services;
        }

        public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            services.AddOutputCache(options =>
            {
                options.AddBasePolicy(builder =>
                {
                    builder.Expire(TimeSpan.FromSeconds(180));
                    builder.AddPolicy<OutputCachePolicy>();
                }, true);
            });

            services.AddStackExchangeRedisOutputCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisCacheConnection");
                options.InstanceName = assemblyName;
            });

            return services;
        }
    }

    internal sealed class BearerSecuritySchemeTransformer() : IOpenApiDocumentTransformer
    {
        public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            var requirements = new Dictionary<string, OpenApiSecurityScheme>
            {
                ["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    BearerFormat = "Json Web Token"
                }
            };
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;

            foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations))
                operation.Value.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme { Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme } }] = Array.Empty<string>()
                });

            await Task.CompletedTask;
        }
    }
}