using System.Diagnostics;
using csharp_scalar.Warmup.DependencyInjection;
using csharp_scalar.Warmup.Telemetry;
using Microsoft.Extensions.Logging;

namespace csharp_scalar.Features.Receitas.CadastrarReceita
{
    public class CadastrarReceitaTelemetry(
        ActivitySource activitySource,
        ILogger<CadastrarReceitaTelemetry> logger
    ) : IService<CadastrarReceitaTelemetry>, IDisposable
    {
        private readonly Activity _telemetryService = activitySource.StartActivity($"{nameof(CadastrarReceitaHandler)}.{nameof(CadastrarReceitaHandler.Handle)}")!;
        private bool _disposed;

        public void InscricaoRelizada(CadastrarReceitaCommand receita)
        {
            _telemetryService
                .AddTag(TelemetryVariables.ReceitaId, receita.Id)
                .AddEvent(new ActivityEvent("Nova receita cadastrada"));

            logger.LogInformation("Receita cadastrada {receita} | XPTO", receita.Id);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                _telemetryService!.Dispose();
            }
            _disposed = true;
        }
    }
}