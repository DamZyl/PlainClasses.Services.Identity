using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PlainClasses.Services.Identity.Api.Utils;
using PlainClasses.Services.Identity.Application.Configurations.Options;

namespace PlainClasses.Services.Identity.Api.Configurations.Extensions
{
    public static class ConfigureServiceExtension
    {
        public static void AddSqlConfiguration(this IServiceCollection services, IConfiguration configuration, string section)
            => services.Configure<SqlOption>(x => configuration.GetSection(section).Bind(x));
        
        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration, string section)
        {
            services.Configure<JwtOption>(x => configuration.GetSection(section).Bind(x));
            var jwtOption = new JwtOption();
            var jwtSection = configuration.GetSection(section);
            jwtSection.Bind(jwtOption);
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.SecretKey)),
                        ValidIssuer = jwtOption.Issuer,
                        ValidateAudience = false,
                        ValidateLifetime = jwtOption.ValidateLifetime
                    };
                });
        }
        
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc(Consts.ApiVersion, new OpenApiInfo
                {
                    Title = Consts.ApiTitle,
                    Version = Consts.ApiVersion
                });
                x.AddSecurityDefinition(Consts.SwaggerBearer, new OpenApiSecurityScheme
                {
                    Description = Consts.SwaggerAuthDescription,
                    Name = Consts.SwaggerAuth,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = Consts.SwaggerBearer
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = Consts.SwaggerBearer
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }
    }
}