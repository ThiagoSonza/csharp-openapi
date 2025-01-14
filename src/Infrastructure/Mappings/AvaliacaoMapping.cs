using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class AvaliacaoMapping : IEntityTypeConfiguration<Avaliacao>
    {
        public void Configure(EntityTypeBuilder<Avaliacao> builder)
        {
            builder.ToTable("Avaliacoes");

            builder.HasKey(k => k.Id);

            builder.Property(p => p.Nota).HasPrecision(2, 2);

            builder.HasOne<Produto>()
                .WithMany()
                .HasForeignKey(fk => fk.ProdutoId)
                .HasConstraintName("FK_AVALIACAO_PRODUTO")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        }
    }
}