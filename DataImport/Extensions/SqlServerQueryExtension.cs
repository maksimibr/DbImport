using System;
using System.Collections.Generic;
using System.Linq;
using DataImport.Entities;
using DataImport.Entities.Enums;

namespace DataImport.Extensions
{
    internal static class SqlServerQueryExtension
    {
        internal static string CreateSqlServerSqlQuery(this IEnumerable<Shipment> shipments)
        {
            var sql = shipments.Aggregate(
                "INSERT INTO Shipments(Id, Identifier, Name, DocumentNumber, RequestNumber, DriverName, Comment, DeviceId, CreateDateUtc, UpdateDateUtc, CompanyId, RecipientCompanyId, StorageId) VALUES",
                (current, shipment) =>
                    current +
                    $"({shipment.Id.GetValue()}, {shipment.Identifier.GetValue()}, {shipment.Name.GetValue()}, {shipment.DocumentNumber.GetValue()}, {shipment.RequestNumber.GetValue()}, {shipment.DriverName.GetValue()}, {shipment.Comment.GetValue()}, {shipment.DeviceId.GetValue()}, {shipment.CreateDateUtc.GetValue()}, {shipment.UpdateDateUtc.GetValue()}, {shipment.CompanyId.GetValue()}, {shipment.RecipientCompanyId.GetValue()}, {shipment.StorageId.GetValue()}),")
                .RemoveLast() + ";";

            return sql;
        }

        internal static string CreateSqlServerSqlQuery(this IEnumerable<Pallet> pallets)
        {
            var sql = pallets.Aggregate(
                              "INSERT INTO TmpPallets(Id, Identifier, Name, IsEmpty, CreateDateUtc, UpdateDateUtc, Status, DeviceId, ShipmentIdentifier, ProductionLineId, StorageId, ProductId, WorkplaceId, PartNumber) VALUES",
                              (current, pallet) =>
                                  current +
                                  $"({pallet.Id.GetValue()}, {pallet.Identifier.GetValue()}, {pallet.Name.GetValue()}, {pallet.IsEmpty.GetValue(DatabaseType.SqlServer)}, {pallet.CreateDateUtc.GetValue()}, {pallet.UpdateDateUtc.GetValue()}, {pallet.Status.GetValue()}, {pallet.DeviceId.GetValue()}, {pallet.ShipmentIdentifier.GetValue()}, {pallet.ProductionLineId.GetValue()}, {pallet.StorageId.GetValue()}, {pallet.ProductId.GetValue()}, {pallet.WorkplaceId.GetValue()}, {pallet.PartNumber.GetValue()}),")
                          .RemoveLast() + ";";

            return sql;
        }

        internal static string CreateSqlServerSqlQuery(this IEnumerable<Batch> batches)
        {
            var sql = batches.Aggregate(
                              "INSERT INTO TmpBatches(Id, Identifier, Name, Comment, Status, CreateDateUtc, UpdateDateUtc, IsEmpty, PalletIdentifier, PartNumber, CompanyId, DeviceId, RecipientCompanyId, ShipmentIdentifier, ProductId, ProductionLineId, StorageId, WorkplaceId) VALUES",
                              (current, batch) =>
                                  current +
                                  $"({batch.Id.GetValue()}, {batch.Identifier.GetValue()}, {batch.Name.GetValue()}, {batch.Comment.GetValue()}, {batch.Status.GetValue()}, {batch.CreateDateUtc.GetValue()}, {batch.UpdateDateUtc.GetValue()}, {batch.IsEmpty.GetValue(DatabaseType.SqlServer)}, {batch.PalletIdentifier.GetValue()}, {batch.PartNumber.GetValue()}, {batch.CompanyId.GetValue()}, {batch.DeviceId.GetValue()}, {batch.RecipientCompanyId.GetValue()}, {batch.ShipmentIdentifier.GetValue()}, {batch.ProductId.GetValue()}, {batch.ProductionLineId.GetValue()}, {batch.StorageId.GetValue()}, {batch.WorkplaceId.GetValue()}),")
                          .RemoveLast() + ";";

            return sql;
        }

        internal static string CreateSqlServerSqlQuery(this IEnumerable<Bottle> bottles)
        {
            var sqlInsert = bottles.Aggregate(
                              "INSERT INTO Bottles(Id, Identifier, ExciseDutyNumber, ExciseDutyNumber2, Status, CreateDateUtc, UpdateDateUtc, CompanyId, RecipientCompanyId, BatchIdentifier, ProductionLineId, BatchId, ProductId) VALUES",
                              (current, bottle) =>
                                  current +
                                  $"({bottle.Id.GetValue()}, {bottle.Identifier.GetValue()}, {bottle.ExciseDutyNumber.GetValue()}, {bottle.ExciseDutyNumber2.GetValue()}, {bottle.Status.GetValue()}, {bottle.CreateDateUtc.GetValue()}, {bottle.UpdateDateUtc.GetValue()}, {bottle.CompanyId.GetValue()}, {bottle.RecipientCompanyId.GetValue()}, {bottle.BatchIdentifier.GetValue()}, {bottle.ProductionLineId.GetValue()}, {bottle.BatchId.GetValue()}, {bottle.ProductId.GetValue()}),")
                          .RemoveLast() + ";";

            const string sqlUpdate = "UPDATE Bottles SET BatchIdentifier = (SELECT TOP 1 Identifier FROM Batches WHERE Bottles.BatchId=Batches.Id);";

            return sqlInsert + sqlUpdate;
        }

        internal static string AffectOnSqlServerTmpBatchTableSqlQuery(this TmpBatchTableAction action)
        {
            switch (action)
            {
                case TmpBatchTableAction.Create:
                {
                    const string createSqlQuery = "CREATE TABLE TmpBatches(" +
                                                  "[Id] [uniqueidentifier] NOT NULL," +
                                                  "[Comment][nvarchar](max) NULL," +
                                                  "[CompanyId] [uniqueidentifier] NOT NULL," +
                                                  "[CreateDateUtc] [datetime2] (7) NOT NULL," +
                                                  "[DeviceId] [uniqueidentifier] NULL," +
                                                  "[Identifier] [nvarchar] (36) NULL," +
                                                  "[ShipmentIdentifier] [nvarchar] (36) NULL," +
                                                  "[IsEmpty] [bit] NOT NULL," +
                                                  "[Name] [nvarchar] (max) NULL," +
                                                  "[PalletIdentifier] [nvarchar] (36) NULL," +
                                                  "[ProductId] [uniqueidentifier] NULL," +
                                                  "[RecipientCompanyId] [uniqueidentifier] NULL," +
                                                  "[Status] [int] NOT NULL," +
                                                  "[UpdateDateUtc] [datetime2] (7) NOT NULL," +
                                                  "[ProductionLineId] [uniqueidentifier] NULL," +
                                                  "[StorageId] [uniqueidentifier] NULL," +
                                                  "[WorkplaceId] [int] NULL," +
                                                  "[PartNumber] [nvarchar] (max) NULL);";

                    return createSqlQuery;
                }
                case TmpBatchTableAction.TransferDataAndDrop:
                {
                    const string transferDataWithShipmentsSqlQuery =
                        "INSERT INTO Batches(Id, Identifier, Name, Comment, Status, CreateDateUtc, UpdateDateUtc, IsEmpty, PalletIdentifier, PartNumber, CompanyId, DeviceId, RecipientCompanyId, ShipmentId, ProductId, ProductionLineId, StorageId, WorkplaceId) " +
                        "SELECT b.Id, b.Identifier, b.Name, b.Comment, b.Status, b.CreateDateUtc, b.UpdateDateUtc, b.IsEmpty, b.PalletIdentifier, b.PartNumber, b.CompanyId, b.DeviceId, b.RecipientCompanyId, s.Id, b.ProductId, b.ProductionLineId, b.StorageId, b.WorkplaceId " +
                        "FROM TmpBatches b INNER JOIN Shipments s ON b.ShipmentIdentifier = s.Identifier WHERE ShipmentIdentifier <> '';";

                    const string transferDataWithoutShipmentsSqlQuery =
                        "INSERT INTO Batches(Id, Identifier, Name, Comment, Status, CreateDateUtc, UpdateDateUtc, IsEmpty, PalletIdentifier, PartNumber, CompanyId, DeviceId, RecipientCompanyId, ProductId, ProductionLineId, StorageId, WorkplaceId) " +
                        "SELECT Id, Identifier, Name, Comment, Status, CreateDateUtc, UpdateDateUtc, IsEmpty, PalletIdentifier, PartNumber, CompanyId, DeviceId, RecipientCompanyId, ProductId, ProductionLineId, StorageId, WorkplaceId FROM TmpBatches " +
                        "WHERE ShipmentIdentifier IS NULL OR ShipmentIdentifier = '';";

                    const string dropSqlQuery = "DROP TABLE TmpBatches;";

                    return transferDataWithShipmentsSqlQuery + transferDataWithoutShipmentsSqlQuery + dropSqlQuery;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }

        }

        internal static string AffectOnSqlServerTmpPalletTableSqlQuery(this TmpBatchTableAction action)
        {
            switch (action)
            {
                case TmpBatchTableAction.Create:
                    {
                        const string createSqlQuery = "CREATE TABLE TmpPallets(" +
                                                      "[Id] [uniqueidentifier] NOT NULL," +
                                                      "[CreateDateUtc] [datetime2](7) NOT NULL," +
                                                      "[DeviceId] [uniqueidentifier] NULL," +
                                                      "[Identifier] [nvarchar](36) NOT NULL," +
                                                      "[IsEmpty] [bit] NOT NULL," +
                                                      "[Name] [nvarchar](max) NULL," +
                                                      "[ShipmentIdentifier] [nvarchar] (36) NULL," +
                                                      "[Status] [int] NOT NULL," +
                                                      "[UpdateDateUtc] [datetime2](7) NOT NULL," +
                                                      "[ProductionLineId] [uniqueidentifier] NULL," +
                                                      "[StorageId] [uniqueidentifier] NULL," +
                                                      "[WorkplaceId] [int] NULL," +
                                                      "[PartNumber] [nvarchar](max) NULL," +
                                                      "[ProductId] [uniqueidentifier] NULL);";

                        return createSqlQuery;
                    }
                case TmpBatchTableAction.TransferDataAndDrop:
                    {
                        const string transferDataWithShipmentsSqlQuery =
                            "INSERT INTO Pallets(Id, Identifier, Name, IsEmpty, CreateDateUtc, UpdateDateUtc, Status, DeviceId, ShipmentId, ProductionLineId, StorageId, ProductId, WorkplaceId, PartNumber) " +
                            "SELECT p.Id, p.Identifier, p.Name, p.IsEmpty, p.CreateDateUtc, p.UpdateDateUtc, p.Status, p.DeviceId, s.Id, p.ProductionLineId, p.StorageId, p.ProductId, p.WorkplaceId, p.PartNumber " +
                            "FROM TmpPallets p INNER JOIN Shipments s ON p.ShipmentIdentifier = s.Identifier WHERE ShipmentIdentifier <> '';";

                        const string transferDataWithoutShipmentsSqlQuery =
                            "INSERT INTO Pallets(Id, Identifier, Name, IsEmpty, CreateDateUtc, UpdateDateUtc, Status, DeviceId, ProductionLineId, StorageId, ProductId, WorkplaceId, PartNumber) " +
                            "SELECT Id, Identifier, Name, IsEmpty, CreateDateUtc, UpdateDateUtc, Status, DeviceId, ProductionLineId, StorageId, ProductId, WorkplaceId, PartNumber FROM TmpPallets " +
                            "WHERE ShipmentIdentifier IS NULL OR ShipmentIdentifier = '';";

                        const string dropSqlQuery = "DROP TABLE TmpPallets;";

                        return transferDataWithShipmentsSqlQuery + transferDataWithoutShipmentsSqlQuery + dropSqlQuery;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}
