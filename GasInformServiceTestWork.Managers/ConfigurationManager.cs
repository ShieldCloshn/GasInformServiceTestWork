using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace GasInformServiceTestWork.Managers
{
    public class ConfigurationManager
    {
        public static IConfigurationRoot Build()
        {
            return new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("settings.json", true, true)
                  .AddEnvironmentVariables()
                  .Build();     
        }

    }
}
