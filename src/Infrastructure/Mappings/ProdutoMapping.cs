using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos");

            builder.HasKey(k => k.Id);

            builder.Property(p => p.Nome).HasMaxLength(50);
            builder.Property(p => p.Imagem).HasMaxLength(150);
            builder.Property(p => p.Descricao).HasMaxLength(500);
            builder.Property(p => p.ValorVendaOriginal).HasPrecision(10, 4);
            builder.Property(p => p.ValorVendaDesconto).HasPrecision(10, 4);
            builder.Property(p => p.ValorCusto).HasPrecision(10, 4);

            builder.HasOne<Empresa>()
                .WithMany()
                .HasForeignKey(fk => fk.EmpresaId)
                .HasConstraintName("FK_EMPRESA_PRODUTO")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        }
    }
}