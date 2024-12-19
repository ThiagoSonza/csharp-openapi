using csharp_scalar.Entities;
using CSharpFunctionalExtensions;
using MediatR;

namespace csharp_scalar.Features.Receitas.CadastrarReceita
{
    public class CadastrarReceitaHandler(ReceitaRepository repository) : IRequestHandler<CadastrarReceitaCommand, Result<Receita>>
    {
        public async Task<Result<Receita>> Handle(CadastrarReceitaCommand request, CancellationToken cancellationToken)
        {
            await repository.CadatrarReceita();
            return Result.Success(new Receita { });
        }
    }
}