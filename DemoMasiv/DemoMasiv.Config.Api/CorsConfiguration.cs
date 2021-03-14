using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoMasiv.Config.Api
{
    public static class CorsConfiguration
    {        
        public const string DevelopPolicyName = "AllowAllOrigin";
        public const string CorsPolicyName = "AllowOrigin";

        public static void AddCorsValidation(this IServiceCollection services, IConfiguration configuration)
        {
            string HostsSection = System.Environment.GetEnvironmentVariable("AvailableOrigins");
            if (HostsSection == null) { throw new Exception("Environment Variable: AvailableOrigins not fount"); }
            string[] HostArray = HostsSection.Split(";");
            services.AddCors(options => 
            {
                options.AddPolicy(CorsPolicyName, builder => 
                {
                    builder
                    .WithOrigins(HostArray)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
                options.AddPolicy(DevelopPolicyName, builder => 
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
        }
    }
}
