using CSharpFunctionalExtensions;

namespace Entities
{
    public sealed class Avaliacao : Entity<int>
    {
        public byte[] RowVersion { get; private set; } = null!;
        public decimal Nota { get; }
        public int ProdutoId { get; }
    }
}