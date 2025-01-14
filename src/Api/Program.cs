using csharp_scalar.Warmup;
using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer()
    .AddDatabase(builder.Configuration)
    .AddCaching(builder.Configuration)
    .AddRateLimit(builder.Configuration)
    .AddRouting(opt => opt.LowercaseUrls = true)
    .AddVersioning()
    .AddOpenApiDoc()
    .AddApiProblemDetails()
    .AddRequestValidations()
    .AddMediaTR()
    .AddApiControllers()
    .AddAutoFac(builder.Host)
    .AddTelemetry(builder.Logging, builder.Configuration);

var app = builder.Build();
app.UseProblemDetails();
app.UseOpenApi();
app.MapControllers();
app.UseOutputCache();
app.UseRateLimiter();
app.UseHttpsRedirection();
app.UseCustomCors();
app.Run();

// Adicionar:
// C4 model

// Autenticação
// Mensageria
// Hangfire