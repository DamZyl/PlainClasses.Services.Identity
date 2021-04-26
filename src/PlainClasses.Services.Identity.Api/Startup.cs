using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlainClasses.Services.Identity.Api.Configurations.Extensions;
using PlainClasses.Services.Identity.Api.Utils;
using PlainClasses.Services.Identity.Infrastructure.Databases;
using PlainClasses.Services.Identity.Infrastructure.IoC;

namespace PlainClasses.Services.Identity.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSqlConfiguration(Configuration, Consts.DbConfigurationSection);
            services.AddDbContext<IdentityContext>();
            
            services.AddJwtConfiguration(Configuration, Consts.JwtConfigurationSection);
            services.AddControllers();
            services.AddSwagger();
           
            // services.AddProblemDetails(x =>
            // {
            //     x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
            //     x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            // });
            
            var builder = new ContainerBuilder();
            
            builder.Populate(services);
            builder.RegisterModule(new InfrastructureModule(Configuration));
            
            var container = builder.Build();
            return new AutofacServiceProvider(container); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // else
            // {
            //     app.UseProblemDetails();
            // }

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
