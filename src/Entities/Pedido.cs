using CSharpFunctionalExtensions;

namespace Entities
{
    public sealed class Pedido : Entity<int>
    {
        public byte[] RowVersion { get; private set; } = null!;
        public DateTime Data { get; }
        public decimal Valor { get; }
        public string Status { get; } = string.Empty;
        public int ClienteId { get; }
        public PedidoCliente PedidoCliente { get; } = null!;
    }

    public record PedidoCliente(string Nome, DateTime DataNascimento, string Cpf, string Genero);
}