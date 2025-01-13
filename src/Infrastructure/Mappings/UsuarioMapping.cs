using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(k => k.Id);

            builder.Property(p => p.Login).HasMaxLength(30);
            builder.Property(p => p.Senha).HasMaxLength(500);

            builder.Property(p => p.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        }
    }
}