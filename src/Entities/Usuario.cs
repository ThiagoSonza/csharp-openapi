using CSharpFunctionalExtensions;

namespace Entities
{
    public sealed class Usuario : Entity<int>
    {
        public byte[] RowVersion { get; private set; } = null!;
        public string Login { get; } = string.Empty;
        public string Senha { get; } = string.Empty;
    }
}