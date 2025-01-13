using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class PedidoProdutoMapping : IEntityTypeConfiguration<PedidoProduto>
    {
        public void Configure(EntityTypeBuilder<PedidoProduto> builder)
        {
            builder.ToTable("PedidoProdutos");

            builder.HasKey(k => k.Id);

            builder.Property(p => p.Nome).HasMaxLength(50);
            builder.Property(p => p.Imagem).HasMaxLength(150);
            builder.Property(p => p.Descricao).HasMaxLength(500);
            builder.Property(p => p.ValorVendaOriginal).HasPrecision(10, 4);
            builder.Property(p => p.ValorVendaDesconto).HasPrecision(10, 4);
            builder.Property(p => p.ValorCusto).HasPrecision(10, 4);

            builder.HasOne<Produto>()
                .WithMany()
                .HasForeignKey(fk => fk.ProdutoId)
                .HasConstraintName("FK_PEDIDO_PRODUTO_PRODUTO")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Pedido>()
                .WithMany()
                .HasForeignKey(fk => fk.PedidoId)
                .HasConstraintName("FK_PEDIDO_PRODUTO_PEDIDO")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        }
    }
}