using System;
using Autofac.Extensions.DependencyInjection;
using GreenPipes;
using Hellang.Middleware.ProblemDetails;
using MassTransit;
using MicroserviceLibrary.Api.Configurations.Extensions;
using MicroserviceLibrary.Api.Utils;
using MicroserviceLibrary.Infrastructure.Databases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlainClasses.Services.Identity.Api.Utils;
using PlainClasses.Services.Identity.Application.Utils.Consumers;
using PlainClasses.Services.Identity.Infrastructure.Databases;

namespace PlainClasses.Services.Identity.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSqlConfiguration(Configuration, Consts.DbConfigurationSection);
            services.AddDbContext<IdentityContext>();
            services.AddScoped(typeof(IApplicationDbContext), typeof(IdentityContext));
            
            services.AddJwtConfiguration(Configuration, Consts.JwtConfigurationSection);
            services.AddControllers();
            services.AddSwagger();
           
            services.AddErrorHandler();
            
            services.AddMassTransit(x =>
            {
                x.AddConsumer<PersonCreatedIntegrationEventConsumer>();
                x.AddConsumer<AuthAddedIntegrationEventConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cur =>
                {
                    cur.UseHealthCheck(provider);
                    cur.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cur.ReceiveEndpoint("personQueue", oq =>
                    {
                        oq.PrefetchCount = 20;
                        oq.UseMessageRetry(r => r.Interval(2, 100));
                        oq.ConfigureConsumer<PersonCreatedIntegrationEventConsumer>(provider);
                        oq.ConfigureConsumer<AuthAddedIntegrationEventConsumer>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();
            
            return new AutofacServiceProvider(AutofacServiceExtension.CreateContainer(services, Configuration, 
                AssembliesConst.MigrationAssembly, AssembliesConst.ApplicationAssembly)); 
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseProblemDetails();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint(Consts.ApiSwaggerUrl, Consts.ApiName);
            });
        }
    }
}
