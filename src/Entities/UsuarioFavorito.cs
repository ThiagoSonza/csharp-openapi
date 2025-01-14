using CSharpFunctionalExtensions;

namespace Entities
{
    public sealed class UsuarioFavorito : Entity<int>
    {
        public byte[] RowVersion { get; private set; } = null!;
        public int ProdutoId { get; }
        public int UsuarioId { get; }
    }
}