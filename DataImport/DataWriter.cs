using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DataImport.Entities;
using DataImport.Entities.Enums;
using DataImport.Extensions;
using Npgsql;

namespace DataImport
{
    internal class DataWriter
    {
        internal string ConnectionString { get; }
        internal DatabaseType DatabaseType { get; }

        public DataWriter(string connectionString, DatabaseType databaseType)
        {
            ConnectionString = connectionString;
            DatabaseType = databaseType;
        }

        internal void DisableAllTablesIndexes()
        {
            var sql = string.Empty;
            switch (DatabaseType)
            {
                case DatabaseType.SqlServer:
                    return;
                case DatabaseType.PostgreSql:
                    sql = PostgreSqlQueryExtension.DisableAllPostgreTablesIndexesSqlQuery();
                    break;
            }

            ExecuteNonQuery(sql);
        }

        internal void ReenableAllTablesIndexes()
        {
            var sql = string.Empty;
            switch (DatabaseType)
            {
                case DatabaseType.SqlServer:
                    return;
                case DatabaseType.PostgreSql:
                    sql = PostgreSqlQueryExtension.ReenableAllPostgreTablesIndexesSqlQuery();
                    break;
            }

            ExecuteNonQuery(sql);
        }

        internal void Insert<T>(ICollection<T> data)
        {
            var sql = CtrateSqlQuery(data);
            ExecuteNonQuery(sql);
        }

        private void ExecuteNonQuery(string sql)
        {
            switch (DatabaseType)
            {
                case DatabaseType.SqlServer:
                    {
                        using (var connection = new SqlConnection(ConnectionString))
                        {
                            connection.Open();
                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    var command = new SqlCommand(sql, connection, transaction);
                                    command.ExecuteNonQuery();

                                    transaction.Commit();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("\n\n#--------Exception beginning--------#");
                                    Console.WriteLine(e.Message);
                                    Console.WriteLine("#--------Exception end--------#\n");
                                    transaction.Rollback();
                                }
                            }
                        }

                        break;
                    }
                case DatabaseType.PostgreSql:
                    {
                        using (var connection = new NpgsqlConnection(ConnectionString))
                        {
                            connection.Open();
                            var transaction = connection.BeginTransaction();

                            try
                            {
                                var command = new NpgsqlCommand(sql, connection, transaction);
                                command.ExecuteNonQuery();

                                transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("\n\n#--------Exception beginning--------#");
                                Console.WriteLine(e.Message);
                                Console.WriteLine("#--------Exception end--------#\n");
                                transaction.Rollback();
                            }
                            finally
                            {
                                transaction.Dispose();
                            }
                        }

                        break;
                    }
            }
        }

        private string CtrateSqlQuery<T>(ICollection<T> data)
        {
            var sql = string.Empty;
            switch (typeof(T))
            {
                case Type bottleType
                    when bottleType == typeof(Bottle) && DatabaseType == DatabaseType.SqlServer:
                    sql = ((IEnumerable<Bottle>)data).CreateSqlServerSqlQuery();
                    break;
                case Type bottleType
                    when bottleType == typeof(Bottle) && DatabaseType == DatabaseType.PostgreSql:
                    sql = ((IEnumerable<Bottle>)data).CreatePostgreSqlSqlQuery();
                    break;
                case Type batchType
                    when batchType == typeof(Batch) && DatabaseType == DatabaseType.SqlServer:
                    sql = TmpBatchTableAction.Create.AffectOnSqlServerTmpBatchTableSqlQuery() +
                          ((IEnumerable<Batch>)data).CreateSqlServerSqlQuery() +
                          TmpBatchTableAction.TransferDataAndDrop.AffectOnSqlServerTmpBatchTableSqlQuery();
                    break;
                case Type batchType
                    when batchType == typeof(Batch) && DatabaseType == DatabaseType.PostgreSql:
                    sql = TmpBatchTableAction.Create.AffectOnPostgreSqlTmpBatchTableSqlQuery() +
                          ((IEnumerable<Batch>)data).CreatePostgreSqlSqlQuery() +
                          TmpBatchTableAction.TransferDataAndDrop.AffectOnPostgreSqlTmpBatchTableSqlQuery();
                    break;
                case Type palletType
                    when palletType == typeof(Pallet) && DatabaseType == DatabaseType.SqlServer:
                    sql = TmpBatchTableAction.Create.AffectOnSqlServerTmpPalletTableSqlQuery() +
                          ((IEnumerable<Pallet>)data).CreateSqlServerSqlQuery() +
                          TmpBatchTableAction.TransferDataAndDrop.AffectOnSqlServerTmpPalletTableSqlQuery();
                    break;
                case Type palletType
                    when palletType == typeof(Pallet) && DatabaseType == DatabaseType.PostgreSql:
                    sql = TmpBatchTableAction.Create.AffectOnPostgreSqlTmpPalletTableSqlQuery() +
                          ((IEnumerable<Pallet>)data).CreatePostgreSqlSqlQuery() +
                          TmpBatchTableAction.TransferDataAndDrop.AffectOnPostgreSqlTmpPalletTableSqlQuery();
                    break;
                case Type shipmentType
                    when shipmentType == typeof(Shipment) && DatabaseType == DatabaseType.SqlServer:
                    sql = ((IEnumerable<Shipment>)data).CreateSqlServerSqlQuery();
                    break;
                case Type shipmentType
                    when shipmentType == typeof(Shipment) && DatabaseType == DatabaseType.PostgreSql:
                    sql = ((IEnumerable<Shipment>)data).CreatePostgreSqlSqlQuery();
                    break;
            }

            return sql;
        }
    }
}
