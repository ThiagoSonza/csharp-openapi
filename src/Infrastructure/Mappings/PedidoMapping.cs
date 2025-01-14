using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");

            builder.HasKey(k => k.Id);

            builder.Property(p => p.Data);
            builder.Property(p => p.Valor).HasPrecision(10, 4);
            builder.Property(p => p.Status).HasMaxLength(20);

            builder.OwnsOne(p => p.PedidoCliente, pedidoCliente =>
            {
                pedidoCliente.Property(p => p.Nome).HasColumnName("ClienteNome").HasMaxLength(50);
                pedidoCliente.Property(p => p.DataNascimento).HasColumnName("ClienteDataNascimento");
                pedidoCliente.Property(p => p.Cpf).HasColumnName("ClienteCpf").HasMaxLength(14);
                pedidoCliente.Property(p => p.Genero).HasColumnName("ClienteGenero").HasMaxLength(20);
            });

            builder.HasOne<Cliente>()
                .WithMany()
                .HasForeignKey(fk => fk.ClienteId)
                .HasConstraintName("FK_PEDIDO_CLIENTE")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        }
    }
}