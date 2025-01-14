using CSharpFunctionalExtensions;

namespace Entities
{
    public sealed class CategoriaProduto : Entity<int>
    {
        public byte[] RowVersion { get; private set; } = null!;
        public int ProdutoId { get; }
        public int CategoriaId { get; }
    }
}