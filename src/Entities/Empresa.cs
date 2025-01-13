using CSharpFunctionalExtensions;

namespace Entities
{
    public sealed class Empresa : Entity<int>
    {
        public byte[] RowVersion { get; private set; } = null!;
        public string Nome { get; } = string.Empty;
        public string Cnpj { get; } = string.Empty;
    }
}