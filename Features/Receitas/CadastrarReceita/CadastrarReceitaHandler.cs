using csharp_scalar.Entities;
using CSharpFunctionalExtensions;
using MediatR;

namespace csharp_scalar.Features.Receitas.CadastrarReceita
{
    public class CadastrarReceitaHandler : IRequestHandler<CadastrarReceitaCommand, Result<Receita>>
    {
        public Task<Result<Receita>> Handle(CadastrarReceitaCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}