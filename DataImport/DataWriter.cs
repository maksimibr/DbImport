using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataImport.Entities;
using DataImport.Entities.Enums;
using DataImport.Extensions;

namespace DataImport
{
    internal class DataWriter
    {
        internal string ConnectionString { get; }

        public DataWriter(string connectionString)
        {
            ConnectionString = connectionString;
        }

        internal async Task InsertAsync<T>(ICollection<T> data)
        {
            using (var connection = new SqlConnection(ConnectionString))
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
                            sql = TmpBatchTableAction.Create.AffectOnTmpBatchTableSqlQuery() +
                                  ((IEnumerable<Batch>)data).CreateSqlQuery() +
                                  TmpBatchTableAction.TransferDataAndDrop.AffectOnTmpBatchTableSqlQuery();
                            break;
                        case Type palletType when palletType == typeof(Pallet):
                            sql = ((IEnumerable<Pallet>)data).CreateSqlQuery();
                            break;
                        case Type shipmentType when shipmentType == typeof(Shipment):
                            sql = ((IEnumerable<Shipment>)data).CreateSqlQuery();
                            break;
                    }

                    Console.WriteLine($"{sql}\n");

                    var command = new SqlCommand(sql, connection, transaction);
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
