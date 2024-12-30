using csharp_scalar.Entities;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;

namespace csharp_scalar.Features.Receitas.CadastrarReceita
{
    public class CadastrarReceitaCommand : IRequest<Result<Receita>>
    {
        private CadastrarReceitaCommand(int codigo, string descricao)
        {
            Id = 0;
            Codigo = codigo;
            Descricao = descricao;
        }

        public int Id { get; }
        public int Codigo { get; }
        public string Descricao { get; }

        public static Result<CadastrarReceitaCommand, string[]?> Criar(int codigo, string descricao)
        {
            CadastrarReceitaCommand command = new(codigo, descricao);

            var validation = new CadastrarReceitaValidator().Validate(command);
            if (validation.IsValid)
                return Result.Success<CadastrarReceitaCommand, string[]?>(command);

            var errorMessages = validation.Errors
                                  .Select(e => e.ErrorMessage)
                                  .ToArray();

            return Result.Failure<CadastrarReceitaCommand, string[]?>(errorMessages);
        }
    }

    internal class CadastrarReceitaValidator : AbstractValidator<CadastrarReceitaCommand>
    {
        public CadastrarReceitaValidator()
        {
            // RuleFor(v => v.Id).LessThan(0).WithMessage("O id é inválido");
        }
    }
}