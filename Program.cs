using csharp_scalar.Warmup;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer()
    .AddVersioning()
    .AddAppOpenApi()
    .AddControllers()
    ;

var app = builder.Build();
app.UseOpenApi();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();