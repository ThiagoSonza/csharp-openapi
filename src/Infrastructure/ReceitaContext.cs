using System.Reflection;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace csharp_scalar.Infrastructure
{
    public class ReceitaContext(DbContextOptions<ReceitaContext> options) : DbContext(options)
    {
        public DbSet<Avaliacao> Avaliacoes { get; } = null!;
        public DbSet<Categoria> Categorias { get; } = null!;
        public DbSet<CategoriaProduto> CategoriaProdutos { get; } = null!;
        public DbSet<Cliente> Clientes { get; } = null!;
        public DbSet<Empresa> Empresas { get; } = null!;
        public DbSet<UsuarioFavorito> FavoritoUsuarios { get; } = null!;
        public DbSet<Pedido> Pedidos { get; } = null!;
        public DbSet<PedidoProduto> PedidoProdutos { get; } = null!;
        public DbSet<Produto> Produtos { get; } = null!;
        public DbSet<Usuario> Usuarios { get; } = null!;

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