using DemoMasiv.Config.Api.Keys;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoMasiv.Config.Api.Configuration
{
    public interface IConfigProvider
    {
        string Read(string KeyName);

        string Read(string KeyName, string Section);

        string[] ReadArray(string KeyName, char Splitter = ';');

        string[] ReadArray(string KeyName, string Section, char Splitter = ';');

        string GetConnectionString(KeysDataBases.DataBases Selected);
    }
}
