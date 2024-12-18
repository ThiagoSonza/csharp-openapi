using Scalar.AspNetCore;

namespace csharp_scalar.Warmup
{
    public static class ApplicationExtensions
    {
        public static WebApplication UseOpenApi(this WebApplication app)
        {
            app.MapOpenApi("/openapi/{documentName}.json");

            app.MapScalarApiReference(options =>
            {
                options
                    .WithEndpointPrefix("/scalar/{documentName}")
                    .WithTitle("CSharp Scalar Api - {documentName}")
                    .WithTheme(ScalarTheme.Solarized)
                    .WithModels(false)
                    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
                    .WithDarkModeToggle(false)
                    .WithTestRequestButton(false);
                ;
            });

            return app;
        }
    }
}