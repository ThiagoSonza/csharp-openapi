using FluentValidation;

namespace csharp_scalar.Features.Receitas.CadastrarReceita
{
    public record CadastrarReceitaRequest(int Codigo, string Descricao);

    public class RequestDataValidator : AbstractValidator<CadastrarReceitaRequest>
    {
        public RequestDataValidator()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("Name é obrigatório.")
                .MinimumLength(3).WithMessage("Nome deve conter mais que 3 caracteres.");

            RuleFor(x => x.Codigo)
                .GreaterThan(0).WithMessage("O código deve ser maior que 0.");
        }
    }
}