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
    .AddTelemetry(builder.Logging, builder.Configuration);

var app = builder.Build();
app.UseProblemDetails();
app.UseOpenApi();
app.MapControllers();
app.UseHttpsRedirection();
app.UseCustomCors();
app.Run();

// Adicionar:
// 2 - separar projetos
// 3 - mensageria