using CSharpFunctionalExtensions;

namespace Entities
{
    public sealed class PedidoProduto : Entity<int>
    {
        public byte[] RowVersion { get; private set; } = null!;
        public int PedidoId { get; }
        public int ProdutoId { get; }
        public string Nome { get; } = string.Empty;
        public string Imagem { get; } = string.Empty;
        public string Descricao { get; } = string.Empty;
        public decimal ValorVendaOriginal { get; }
        public decimal ValorVendaDesconto { get; }
        public decimal ValorCusto { get; }
    }
}