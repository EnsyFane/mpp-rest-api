using Microsoft.Extensions.Configuration;
using System.IO;

namespace BasketballClient
{
    internal class Config
    {
        public string BasketballBaseUrl { get; set; }

        public static Config ReadConfig(string path)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path)
                .Build();

            return configuration.Get<Config>();
        }
    }
}