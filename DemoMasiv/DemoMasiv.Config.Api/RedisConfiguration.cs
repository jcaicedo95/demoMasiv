using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoMasiv.Config.Api
{
    public static class RedisConfiguration
    {
        private const string KeyAvaliableHosts = "redisConnection";

        public static void AddRedisConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            string redisConnection = System.Environment.GetEnvironmentVariable("redisConnection");            
            services.AddSingleton<IConnectionMultiplexer>(x =>
                ConnectionMultiplexer.Connect(redisConnection));

        }
    }
}
