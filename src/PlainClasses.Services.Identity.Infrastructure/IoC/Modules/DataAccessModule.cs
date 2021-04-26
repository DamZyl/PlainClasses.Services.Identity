using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlainClasses.Services.Identity.Application.Configurations.Data;
using PlainClasses.Services.Identity.Application.Configurations.Options;
using PlainClasses.Services.Identity.Infrastructure.Databases;
using PlainClasses.Services.Identity.Infrastructure.Utils;

namespace PlainClasses.Services.Identity.Infrastructure.IoC.Modules
{
    public class DataAccessModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public DataAccessModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var dbConfig = _configuration.GetSection(Consts.DbConfigurationSection).Get<SqlOption>();
            
            builder.Register(p => dbConfig).SingleInstance();
            
            builder.RegisterType<SqlConnectionFactory>()
                .As<ISqlConnectionFactory>()
                .WithParameter("connectionString", dbConfig.ConnectionString)
                .InstancePerLifetimeScope();


            builder
                .Register(c =>
                {
                    var dbContextOptionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
                    dbContextOptionsBuilder.UseSqlServer(dbConfig.ConnectionString, options => 
                        options.MigrationsAssembly("PlainClasses.Services.Identity.Api"));

                    return new IdentityContext(dbContextOptionsBuilder.Options);
                })
                .AsSelf()
                .As<DbContext>()
                .InstancePerLifetimeScope();
        }
    }
}