using System;
using System.IO;
using System.Threading.Tasks;
using DataImport.Entities;
using DataImport.Entities.Enums;
using DataImport.Maps;
using Microsoft.Extensions.Configuration;

namespace DataImport
{
    internal class Program
    {
        internal static IConfiguration Configuration { get; private set; }

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
                Console.ReadKey();
                return;
            }

            var currentFolder = Directory.GetCurrentDirectory();
            if (string.IsNullOrEmpty(currentFolder))
            {
                Console.WriteLine("Error! Could not get current path");
                Console.ReadKey();
                return;
            }

            var databaseType = Enum.Parse<DatabaseType>(Configuration["DatabaseType"]);
            Console.WriteLine($"ConnectionString: {connectionString}\nDatabaseType: {databaseType.ToString()}");

            var dataWriter = new DataWriter(connectionString, databaseType);

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
                Console.ReadKey();
                return;
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
                Console.ReadKey();
                return;
            }

            var batchCsvPath = Path.Combine(currentFolder, "batches.csv");
            if (File.Exists(batchCsvPath))
            {
                var batchRecords = CsvRecordsReader.Read<Batch, BatchMap>(batchCsvPath);
                using (var batchEnumerator = batchRecords.GetEnumerator())
                {
                    Console.WriteLine("\n#-----------------#");
                    Console.WriteLine("Batches..");
                    Console.WriteLine("#-----------------#\n");

                    while (batchEnumerator.MoveNext())
                    {
                        var segment = batchEnumerator.Current;
                        Task.Run(async () => await dataWriter.InsertAsync(segment).ConfigureAwait(true)).Wait();
                    }
                }
            }
            else
            {
                Console.WriteLine($"file 'batches.csv' not found! Path: {batchCsvPath}");
                Console.ReadKey();
                return;
            }

            return;

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
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Ok");
            Console.ReadKey();
        }
    }
}
