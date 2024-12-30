using Asp.Versioning.ApiExplorer;

namespace csharp_scalar.Warmup
{
    public static class ApplicationExtensions
    {
        public static WebApplication UseOpenApi(this WebApplication app)
        {
            app.MapOpenApi("/openapi/{documentName}.json");

            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var version in provider.ApiVersionDescriptions)
            {
                app.UseReDoc(options =>
                {
                    options.SpecUrl($"/openapi/{version.GroupName}.json");
                    options.RoutePrefix = $"docs/{version.GroupName}";
                    options.DocumentTitle = $"Documentação da API - Versão {version.ApiVersion}";
                    // Expande automaticamente as respostas com código (200,201,...)
                    options.ExpandResponses("");
                    // Esconde o botão de download do JSON
                    options.HideDownloadButton();
                    options.NoAutoAuth();
                    options.DisableSearch();
                });
            }

            return app;
        }

        public static WebApplication UseCustomCors(this WebApplication app)
        {
            app.UseCors(builder => builder
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            return app;

        }
    }
}