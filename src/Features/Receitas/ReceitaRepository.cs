using csharp_scalar.Warmup.DependencyInjection;

namespace csharp_scalar.Features.Receitas
{
    public class ReceitaRepository : IService<ReceitaRepository>
    {
        public async Task CadatrarReceita()
        {
            await Task.CompletedTask;
        }
    }
}