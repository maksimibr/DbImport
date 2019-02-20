using System;
using System.IO;
using System.Threading.Tasks;
using DataImport.Entities;
using DataImport.Maps;
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

            var currentFolder = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName;
            if (string.IsNullOrEmpty(currentFolder))
            {
                Console.WriteLine("Error! Could not get current path");
                return;
            }

            var dataWriter = new DataWriter(connectionString);

            var shipmentCsvPath = Path.Combine(currentFolder, "shipments.csv");
            if (File.Exists(shipmentCsvPath))
            {
                var shipmentRecords = CsvRecordsReader.Read<Shipment, ShipmentMap>(shipmentCsvPath, 2);
                using (var shipmentEnumerator = shipmentRecords.GetEnumerator())
                {
                    while (shipmentEnumerator.MoveNext())
                    {
                        var segment = shipmentEnumerator.Current;
                        Task.Run(async () =>
                            await dataWriter.InsertAsync(segment).ConfigureAwait(true)).Wait();
                    }
                }
            }
            else
            {
                Console.WriteLine($"file 'shipments.csv' not found! Path: {shipmentCsvPath}");
            }

            Console.ReadKey();
        }
    }
}
