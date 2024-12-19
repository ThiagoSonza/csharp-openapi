using csharp_scalar.Warmup;
using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer()
    .AddRouting(opt => opt.LowercaseUrls = true)
    .AddVersioning()
    .AddAppOpenApi()
    .AddApiProblemDetails()
    .AddRequestValidations()
    .AddMediaTR()
    .AddApiControllers()
    .AddAutoFac(builder.Host);

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
// 1 - telemetria
// 3 - separar projetos
// 4 - mensageria