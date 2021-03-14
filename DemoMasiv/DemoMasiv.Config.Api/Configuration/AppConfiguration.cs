using DemoMasiv.Config.Api.Keys;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoMasiv.Config.Api.Configuration
{
    public class AppConfiguration : IConfigProvider
    {
        private static readonly AppConfiguration obj = new AppConfiguration();
        readonly IConfiguration ConfigRoot = new ConfigurationBuilder()
                                            .AddJsonFile("appsettings.json")
                                            .AddEnvironmentVariables()
                                            .Build();

        public static AppConfiguration Instance
        {
            get { return obj; }
        }

        public string Read(AppKeys.Keys KeyItem)
            => ConfigRoot.GetSection("Settings").GetSection(KeyItem.ToString()).Value;

        
        public string Read(String KeyName)
            => ConfigRoot.GetSection("Settings").GetSection(KeyName).Value;
        
        public string Read(String KeyName, String SectionName)
            => ConfigRoot.GetSection(SectionName).GetSection(KeyName).Value;
        
        public string Read(AppKeys.Keys KeyItem, String SectionName)
            => ConfigRoot.GetSection(SectionName).GetSection(KeyItem.ToString()).Value;
        
        public string[] ReadArray(String KeyName, char Splitter = ';')
            => ConfigRoot.GetSection("Settings").GetSection(KeyName).Value.Split(Splitter);
        
        public string[] ReadArray(AppKeys.Keys KeyItem, char Splitter = ';')
            => ConfigRoot.GetSection("Settings").GetSection(KeyItem.ToString()).Value.Split(Splitter);
        
        public string[] ReadArray(AppKeys.Keys KeyItem, String SectionName, char Splitter = ';')
            => ConfigRoot.GetSection(SectionName).GetSection(KeyItem.ToString()).Value.Split(Splitter);
        
        public string[] ReadArray(String KeyName, String SectionName, char Splitter = ';')
        => ConfigRoot.GetSection(SectionName).GetSection(KeyName).Value.Split(Splitter);
        
        public string GetConnectionString(KeysDataBases.DataBases Selected)
            => Read(Selected.ToString(), "ConnectionStrings");
    }
}
