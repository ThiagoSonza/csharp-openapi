using csharp_scalar.Entities;
using CSharpFunctionalExtensions;
using MediatR;

namespace csharp_scalar.Features.Receitas.CadastrarReceita
{
    public class CadastrarReceitaHandler(
        ReceitaRepository repository,
        CadastrarReceitaTelemetry telemetry
    ) : IRequestHandler<CadastrarReceitaCommand, Result<Receita>>
    {
        public async Task<Result<Receita>> Handle(CadastrarReceitaCommand command, CancellationToken cancellationToken)
        {
            await repository.CadatrarReceita();
            telemetry.InscricaoRelizada(command);

            return Result.Success(new Receita { });
        }
    }
}