using Autofac;
using Microsoft.Extensions.Configuration;
using PlainClasses.Services.Identity.Infrastructure.IoC.Modules;

namespace PlainClasses.Services.Identity.Infrastructure.IoC
{
    public class InfrastructureModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public InfrastructureModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new AuthModule());
            builder.RegisterModule(new DataAccessModule(_configuration));
            builder.RegisterModule(new MediatrModule());
            builder.RegisterModule(new RepositoryModule());
        }
    }
}