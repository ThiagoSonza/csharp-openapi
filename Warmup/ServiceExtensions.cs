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

        public static IServiceCollection AddAppOpenApi(this IServiceCollection services)
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
            services.AddValidatorsFromAssemblyContaining<Program>();

            return services;
        }

        public static IServiceCollection AddMediaTR(this IServiceCollection services)
        {
            string applicationAssemblyName = Assembly.GetExecutingAssembly().GetName().Name!;
            var assembly = AppDomain.CurrentDomain.Load(applicationAssemblyName);

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));

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