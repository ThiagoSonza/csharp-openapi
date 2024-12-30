using System.Reflection;
using csharp_scalar.Entities;
using Microsoft.EntityFrameworkCore;

namespace csharp_scalar.Infrastructure
{
    public class ReceitaContext(DbContextOptions<ReceitaContext> options) : DbContext(options)
    {
        public DbSet<Receita> Receitas { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<string>()
                .AreUnicode(false)
                .HaveMaxLength(150);
        }
    }
}