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
                var shipmentRecords = CsvRecordsReader.Read<Shipment, ShipmentMap>(shipmentCsvPath);
                using (var shipmentEnumerator = shipmentRecords.GetEnumerator())
                {
                    Console.WriteLine("\n#-----------------#");
                    Console.WriteLine("Shipments..");
                    Console.WriteLine("#-----------------#\n");
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

            var palletCsvPath = Path.Combine(currentFolder, "pallets.csv");
            if (File.Exists(palletCsvPath))
            {
                var palletRecords = CsvRecordsReader.Read<Pallet, PalletMap>(palletCsvPath);
                using (var palletEnumerator = palletRecords.GetEnumerator())
                {
                    Console.WriteLine("\n#-----------------#");
                    Console.WriteLine("Palets..");
                    Console.WriteLine("#-----------------#\n");

                    while (palletEnumerator.MoveNext())
                    {
                        var segment = palletEnumerator.Current;
                        Task.Run(async () =>
                            await dataWriter.InsertAsync(segment).ConfigureAwait(true)).Wait();
                    }
                }
            }
            else
            {
                Console.WriteLine($"file 'pallets.csv' not found! Path: {palletCsvPath}");
            }

            /* ToDo Insert batches here */

            var bottleCsvPath = Path.Combine(currentFolder, "bottles.csv");
            if (File.Exists(bottleCsvPath))
            {
                var bottlesRecords = CsvRecordsReader.Read<Bottle, BottleMap>(bottleCsvPath);
                using (var bottleEnumerator = bottlesRecords.GetEnumerator())
                {
                    Console.WriteLine("\n#-----------------#");
                    Console.WriteLine("Bottles..");
                    Console.WriteLine("#-----------------#\n");
                    while (bottleEnumerator.MoveNext())
                    {
                        var segment = bottleEnumerator.Current;
                        Task.Run(async () =>
                            await dataWriter.InsertAsync(segment).ConfigureAwait(true)).Wait();
                    }
                }
            }
            else
            {
                Console.WriteLine($"file bottles.csv' not found! Path: {bottleCsvPath}");
            }

            Console.ReadKey();
        }
    }
}
