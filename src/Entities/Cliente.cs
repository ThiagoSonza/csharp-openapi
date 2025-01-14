using CSharpFunctionalExtensions;

namespace Entities
{
    public sealed class Cliente : Entity<int>
    {
        public byte[] RowVersion { get; private set; } = null!;
        public string Nome { get; } = string.Empty;
        public DateTime DataNascimento { get; }
        public string Cpf { get; } = string.Empty;
        public string Sexo { get; } = string.Empty;
        public int? UsuarioId { get; }
    }
}