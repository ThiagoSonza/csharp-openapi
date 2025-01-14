using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class UsuarioFavoritoMapping : IEntityTypeConfiguration<UsuarioFavorito>
    {
        public void Configure(EntityTypeBuilder<UsuarioFavorito> builder)
        {
            builder.ToTable("UsuarioFavoritos");

            builder.HasKey(k => k.Id);

            builder.HasOne<Produto>()
                .WithMany()
                .HasForeignKey(fk => fk.ProdutoId)
                .HasConstraintName("FK_USUARIO_FAVORITO_PRODUTO")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(fk => fk.UsuarioId)
                .HasConstraintName("FK_USUARIO_FAVORITO_USUARIO")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        }
    }
}