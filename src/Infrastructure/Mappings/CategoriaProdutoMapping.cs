using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class CategoriaProdutoMapping : IEntityTypeConfiguration<CategoriaProduto>
    {
        public void Configure(EntityTypeBuilder<CategoriaProduto> builder)
        {
            builder.ToTable("CategoriaProdutos");

            builder.HasKey(k => k.Id);

            builder.HasOne<Produto>()
                .WithMany()
                .HasForeignKey(fk => fk.ProdutoId)
                .HasConstraintName("FK_CATEGORIA_PRODUTOS_PRODUTO")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Categoria>()
                .WithMany()
                .HasForeignKey(fk => fk.CategoriaId)
                .HasConstraintName("FK_CATEGORIA_PRODUTOS_CATEGORIA")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        }
    }
}