using Autofac;
using Features.Ambiente;

namespace csharp_scalar.Warmup.DependencyInjection
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(Ambiente).Assembly)
                .AsClosedTypesOf(typeof(IService<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}