using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DataImport
{
    internal class Program
    {
        internal static IConfigurationRoot Configuration { get; private set; }

        private static void Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("connectionString cannot be empty!");
                return;
            }

            Console.ReadKey();
        }
    }
}
