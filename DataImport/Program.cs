using System;
using System.IO;
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

            if (!Enum.TryParse(Configuration["DatabaseType"], out DatabaseType databaseType))
            {
                Console.WriteLine("Invalid database type");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"ConnectionString: {connectionString}\nDatabaseType: {databaseType.ToString()}");

            var dataWriter = new DataWriter(connectionString, databaseType);

            dataWriter.DisableAllTablesIndexes();

            var shipmentCsvPath = Path.Combine(currentFolder, "shipments.csv");
            if (File.Exists(shipmentCsvPath))
            {
                var shipmentRecords = CsvRecordsReader.Read<Shipment, ShipmentMap>(shipmentCsvPath);
                using (var shipmentEnumerator = shipmentRecords.GetEnumerator())
                {
                    Console.WriteLine("\n\n#--------------------Shipments beginning--------------------#\n");

                    var shipmentCount = 0;
                    Console.Write(shipmentCount);

                    while (shipmentEnumerator.MoveNext())
                    {
                        var segment = shipmentEnumerator.Current;
                        dataWriter.Insert(segment);

                        shipmentCount += segment.Count;
                        ClearCurrentConsoleLine();
                        Console.Write(shipmentCount);
                    }

                    Console.WriteLine("\n\n#-----------------------Shipments end-----------------------#\n\n");
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
                    Console.WriteLine("\n\n#--------------------Palets beginning--------------------#\n");

                    var paletsCount = 0;
                    Console.Write(paletsCount);

                    while (palletEnumerator.MoveNext())
                    {
                        var segment = palletEnumerator.Current;
                        dataWriter.Insert(segment);

                        paletsCount += segment.Count;
                        ClearCurrentConsoleLine();
                        Console.Write(paletsCount);
                    }

                    Console.WriteLine("\n\n#-----------------------Palets end-----------------------#\n\n");
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
                    Console.WriteLine("\n\n#--------------------Batches beginning--------------------#\n");

                    var batchesCount = 0;
                    Console.Write(batchesCount);

                    while (batchEnumerator.MoveNext())
                    {
                        var segment = batchEnumerator.Current;
                        dataWriter.Insert(segment);

                        batchesCount += segment.Count;
                        ClearCurrentConsoleLine();
                        Console.Write(batchesCount);
                    }

                    Console.WriteLine("\n\n#-----------------------Batches end-----------------------#\n\n");
                }
            }
            else
            {
                Console.WriteLine($"file 'batches.csv' not found! Path: {batchCsvPath}");
                Console.ReadKey();
                return;
            }

            var bottleCsvPath = Path.Combine(currentFolder, "bottles.csv");
            if (File.Exists(bottleCsvPath))
            {
                var bottlesRecords = CsvRecordsReader.Read<Bottle, BottleMap>(bottleCsvPath);
                using (var bottleEnumerator = bottlesRecords.GetEnumerator())
                {
                    Console.WriteLine("\n\n#--------------------Bottles beginning--------------------#\n");

                    var bottlesCount = 0;
                    Console.Write(bottlesCount);

                    while (bottleEnumerator.MoveNext())
                    {
                        var segment = bottleEnumerator.Current;
                        dataWriter.Insert(segment);

                        bottlesCount += segment.Count;
                        ClearCurrentConsoleLine();
                        Console.Write(bottlesCount);
                    }

                    Console.WriteLine("\n\n#-----------------------Bottles end-----------------------#\n\n");
                }
            }
            else
            {
                Console.WriteLine($"file bottles.csv' not found! Path: {bottleCsvPath}");
                Console.ReadKey();
                return;
            }

            dataWriter.ReenableAllTablesIndexes();

            Console.WriteLine("Ok");
            Console.ReadKey();
        }

        private static void ClearCurrentConsoleLine()
        {
            var currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
