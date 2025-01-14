using CSharpFunctionalExtensions;

namespace Entities
{
    public sealed class Produto : Entity<int>
    {
        public byte[] RowVersion { get; private set; } = null!;
        public string Nome { get; } = string.Empty;
        public string Imagem { get; } = string.Empty;
        public string Descricao { get; } = string.Empty;
        public decimal ValorVendaOriginal { get; }
        public decimal ValorVendaDesconto { get; }
        public decimal ValorCusto { get; }
        public int EmpresaId { get; }
    }
}