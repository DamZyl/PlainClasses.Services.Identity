using Autofac;
using PlainClasses.Services.Identity.Application;

namespace PlainClasses.Services.Identity.Infrastructure.IoC.Modules
{
    public class AuthModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PasswordHasher>()
                .As<IPasswordHasher>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<JwtHandler>()
                .As<IJwtHandler>()
                .InstancePerLifetimeScope();
        }
    }
}