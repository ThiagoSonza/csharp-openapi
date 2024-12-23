using Autofac;

namespace csharp_scalar.Warmup.DependencyInjection
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(Program).Assembly)
                .AsClosedTypesOf(typeof(IService<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}