using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class ClienteMapping : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");

            builder.HasKey(k => k.Id);

            builder.Property(k => k.Nome).HasMaxLength(50);
            builder.Property(k => k.DataNascimento);
            builder.Property(k => k.Cpf).HasMaxLength(14);
            builder.Property(k => k.Sexo).HasMaxLength(20);

            builder.HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(fk => fk.UsuarioId)
                .HasConstraintName("FK_CLIENTE_USUARIO")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.Property(p => p.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        }
    }
}