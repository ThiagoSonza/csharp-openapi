using csharp_scalar.Warmup;
using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer()
    .AddRouting(opt => opt.LowercaseUrls = true)
    .AddVersioning()
    .AddOpenApiDoc()
    .AddApiProblemDetails()
    .AddRequestValidations()
    .AddMediaTR()
    .AddApiControllers()
    .AddAutoFac(builder.Host)
    .AddTelemetry(builder.Logging);

var app = builder.Build();
app.UseProblemDetails();
app.UseOpenApi();
app.MapControllers();
app.UseHttpsRedirection();
app.UseCors(builder => builder
    .SetIsOriginAllowed(origin => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
app.Run();

// Adicionar:
// 3 - separar projetos
// 4 - mensageria