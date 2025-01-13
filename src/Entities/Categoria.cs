using CSharpFunctionalExtensions;

namespace Entities
{
    public sealed class Categoria : Entity<int>
    {
        public byte[] RowVersion { get; private set; } = null!;
        public string Nome { get; } = string.Empty;
        public string Imagem { get; } = string.Empty;
    }
}