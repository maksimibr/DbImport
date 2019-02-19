using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using DataImport.Entities;
using DataImport.Extensions;
using Microsoft.Extensions.Configuration;

namespace DataImport
{
    internal class Program
    {
        private static void Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            Task.Run(async () =>
            {
                //await InsertDataAsync(connectionString, shipments).ConfigureAwait(true);
                //await InsertDataAsync(connectionString, pallets).ConfigureAwait(true);
                //await InsertDataAsync(connectionString, batches).ConfigureAwait(true);
                //await InsertDataAsync(connectionString, bottles).ConfigureAwait(true);
            }).Wait();

            Console.ReadKey();
        }

        private static async Task InsertDataAsync<T>(string connectionString, IEnumerable<T> data)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                var transaction = connection.BeginTransaction();

                try
                {
                    var sql = string.Empty;
                    switch (typeof(T))
                    {
                        case Type bottleType when bottleType == typeof(Bottle):
                            sql = ((IEnumerable<Bottle>)data).CreateSqlQuery();
                            break;
                        case Type batchType when batchType == typeof(Batch):
                            sql = ((IEnumerable<Batch>)data).CreateSqlQuery();
                            break;
                        case Type palletType when palletType == typeof(Pallet):
                            sql = ((IEnumerable<Pallet>)data).CreateSqlQuery();
                            break;
                        case Type shipmentType when shipmentType == typeof(Shipment):
                            sql = ((IEnumerable<Shipment>)data).CreateSqlQuery();
                            break;
                    }

                    var command = new SqlCommand(sql, connection, transaction);
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    transaction.Rollback();
                }
            }
        }
    }
}
