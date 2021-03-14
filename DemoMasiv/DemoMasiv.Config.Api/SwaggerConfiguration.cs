using DemoMasiv.Config.Api.Class;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace DemoMasiv.Config.Api
{
    public static class SwaggerConfiguration
    {
        private const string KEYApiName = "Swagger:ApiName";
        private const string KEYApiVersion = "Swagger:ApiVersion";

        public static void AddSwagger(this IServiceCollection services)
            => AddSwagger(services, string.Empty, string.Empty);

        public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            string ApiName = "DemoMasiv.Core.WebApi";
            IConfigurationSection SectionApiName = configuration.GetSection(KEYApiName);
            if (SectionApiName.Exists()) { ApiName = SectionApiName.Value; }
            string ApiVersion = "V1";
            IConfigurationSection SectionApiVersion = configuration.GetSection(KEYApiVersion);
            if (SectionApiVersion.Exists()) { ApiVersion = SectionApiVersion.Value; }
            AddSwagger(services, ApiName, ApiVersion);
        }

        private static void AddSwagger(this IServiceCollection services, string ApiName, string ApiVersion)
        {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = ApiName, Version = ApiVersion });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme 
                { 
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.OperationFilter<AddRequiredHeaderParameter>();
            });
        }
    }
}
