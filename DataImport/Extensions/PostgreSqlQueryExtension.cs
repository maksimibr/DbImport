using System;
using System.Collections.Generic;
using System.Linq;
using DataImport.Entities;
using DataImport.Entities.Enums;

namespace DataImport.Extensions
{
    internal static class PostgreSqlQueryExtension
    {
        internal static string CreatePostgreSqlSqlQuery(this IEnumerable<Shipment> shipments)
        {
            var sql = shipments.Aggregate(
                              "INSERT INTO public.\"Shipments\"(\"Id\", \"Identifier\", \"Name\", \"DocumentNumber\", \"RequestNumber\", \"DriverName\", \"Comment\", \"DeviceId\", \"CreateDateUtc\", \"UpdateDateUtc\", \"CompanyId\", \"RecipientCompanyId\", \"StorageId\") VALUES",
                              (current, shipment) =>
                                  current +
                                  $"({shipment.Id.GetValue()}, {shipment.Identifier.GetValue()}, {shipment.Name.GetValue()}, {shipment.DocumentNumber.GetValue()}, {shipment.RequestNumber.GetValue()}, {shipment.DriverName.GetValue()}, {shipment.Comment.GetValue()}, {shipment.DeviceId.GetValue()}, {shipment.CreateDateUtc.GetValue()}, {shipment.UpdateDateUtc.GetValue()}, {shipment.CompanyId.GetValue()}, {shipment.RecipientCompanyId.GetValue()}, {shipment.StorageId.GetValue()}),")
                          .RemoveLast() + ";";

            return sql;
        }

        internal static string CreatePostgreSqlSqlQuery(this IEnumerable<Pallet> pallets)
        {
            var sql = pallets.Aggregate(
                              "INSERT INTO public.\"TmpPallets\"(\"Id\", \"Identifier\", \"Name\", \"IsEmpty\", \"CreateDateUtc\", \"UpdateDateUtc\", \"Status\", \"DeviceId\", \"ShipmentIdentifier\", \"ProductionLineId\", \"StorageId\", \"ProductId\", \"WorkplaceId\", \"PartNumber\") VALUES",
                              (current, pallet) =>
                                  current +
                                  $"({pallet.Id.GetValue()}, {pallet.Identifier.GetValue()}, {pallet.Name.GetValue()}, {pallet.IsEmpty.GetValue(DatabaseType.PostgreSql)}, {pallet.CreateDateUtc.GetValue()}, {pallet.UpdateDateUtc.GetValue()}, {pallet.Status.GetValue()}, {pallet.DeviceId.GetValue()}, {pallet.ShipmentIdentifier.GetValue()}, {pallet.ProductionLineId.GetValue()}, {pallet.StorageId.GetValue()}, {pallet.ProductId.GetValue()}, {pallet.WorkplaceId.GetValue()}, {pallet.PartNumber.GetValue()}),")
                          .RemoveLast() + ";";

            return sql;
        }

        internal static string CreatePostgreSqlSqlQuery(this IEnumerable<Batch> batches)
        {
            var sql = batches.Aggregate(
                              "INSERT INTO public.\"TmpBatches\"(\"Id\", \"Identifier\", \"Name\", \"Comment\", \"Status\", \"CreateDateUtc\", \"UpdateDateUtc\", \"IsEmpty\", \"PalletIdentifier\", \"PartNumber\", \"CompanyId\", \"DeviceId\", \"RecipientCompanyId\", \"ShipmentIdentifier\", \"ProductId\", \"ProductionLineId\", \"StorageId\", \"WorkplaceId\") VALUES",
                              (current, batch) =>
                                  current +
                                  $"({batch.Id.GetValue()}, {batch.Identifier.GetValue()}, {batch.Name.GetValue()}, {batch.Comment.GetValue()}, {batch.Status.GetValue()}, {batch.CreateDateUtc.GetValue()}, {batch.UpdateDateUtc.GetValue()}, {batch.IsEmpty.GetValue(DatabaseType.PostgreSql)}, {batch.PalletIdentifier.GetValue()}, {batch.PartNumber.GetValue()}, {batch.CompanyId.GetValue()}, {batch.DeviceId.GetValue()}, {batch.RecipientCompanyId.GetValue()}, {batch.ShipmentIdentifier.GetValue()}, {batch.ProductId.GetValue()}, {batch.ProductionLineId.GetValue()}, {batch.StorageId.GetValue()}, {batch.WorkplaceId.GetValue()}),")
                          .RemoveLast() + ";";

            return sql;
        }

        internal static string AffectOnPostgreSqlTmpBatchTableSqlQuery(this TmpBatchTableAction action)
        {
            switch (action)
            {
                case TmpBatchTableAction.Create:
                    {
                        const string createSqlQuery = "CREATE TABLE public.\"TmpBatches\"(" +
                                                      "\"Id\" uuid NOT NULL," +
                                                      "\"Comment\" text COLLATE pg_catalog.\"default\"," +
                                                      "\"CompanyId\" uuid NOT NULL," +
                                                      "\"CreateDateUtc\" timestamp without time zone NOT NULL," +
                                                      "\"DeviceId\" uuid," +
                                                      "\"Identifier\" character varying(36) COLLATE pg_catalog.\"default\"," +
                                                      "\"IsEmpty\" boolean NOT NULL," +
                                                      "\"Name\" text COLLATE pg_catalog.\"default\"," +
                                                      "\"PalletIdentifier\" character varying(36) COLLATE pg_catalog.\"default\"," +
                                                      "\"ProductId\" uuid," +
                                                      "\"RecipientCompanyId\" uuid," +
                                                      "\"ShipmentIdentifier\" character varying(36) COLLATE pg_catalog.\"default\"," +
                                                      "\"Status\" integer NOT NULL," +
                                                      "\"UpdateDateUtc\" timestamp without time zone NOT NULL," +
                                                      "\"ProductionLineId\" uuid," +
                                                      "\"StorageId\" uuid," +
                                                      "\"WorkplaceId\" integer," +
                                                      "\"PartNumber\" text COLLATE pg_catalog.\"default\");";

                        return createSqlQuery;
                    }
                case TmpBatchTableAction.TransferDataAndDrop:
                    {
                        const string transferDataWithShipmentsSqlQuery =
                            "INSERT INTO public.\"Batches\"(\"Id\", \"Identifier\", \"Name\", \"Comment\", \"Status\", \"CreateDateUtc\", \"UpdateDateUtc\", \"IsEmpty\", \"PalletIdentifier\", \"PartNumber\", \"CompanyId\", \"DeviceId\", \"RecipientCompanyId\", \"ShipmentId\", \"ProductId\", \"ProductionLineId\", \"StorageId\", \"WorkplaceId\") " +
                            "SELECT b.\"Id\", b.\"Identifier\", b.\"Name\", b.\"Comment\", b.\"Status\", b.\"CreateDateUtc\", b.\"UpdateDateUtc\", b.\"IsEmpty\", b.\"PalletIdentifier\", b.\"PartNumber\", b.\"CompanyId\", b.\"DeviceId\", b.\"RecipientCompanyId\", s.\"Id\", b.\"ProductId\", b.\"ProductionLineId\", b.\"StorageId\", b.\"WorkplaceId\" " +
                            "FROM public.\"TmpBatches\" b INNER JOIN public.\"Shipments\" s ON b.\"ShipmentIdentifier\" = s.\"Identifier\" WHERE b.\"ShipmentIdentifier\" IS NOT NULL;";

                        const string transferDataWithoutShipmentsSqlQuery =
                            "INSERT INTO public.\"Batches\"(\"Id\", \"Identifier\", \"Name\", \"Comment\", \"Status\", \"CreateDateUtc\", \"UpdateDateUtc\", \"IsEmpty\", \"PalletIdentifier\", \"PartNumber\", \"CompanyId\", \"DeviceId\", \"RecipientCompanyId\", \"ProductId\", \"ProductionLineId\", \"StorageId\", \"WorkplaceId\") " +
                            "SELECT \"Id\", \"Identifier\", \"Name\", \"Comment\", \"Status\", \"CreateDateUtc\", \"UpdateDateUtc\", \"IsEmpty\", \"PalletIdentifier\", \"PartNumber\", \"CompanyId\", \"DeviceId\", \"RecipientCompanyId\", \"ProductId\", \"ProductionLineId\", \"StorageId\", \"WorkplaceId\" FROM public.\"TmpBatches\" " +
                            "WHERE \"ShipmentIdentifier\" IS NULL;";

                        const string dropSqlQuery = "DROP TABLE public.\"TmpBatches;\"";

                        return transferDataWithShipmentsSqlQuery + transferDataWithoutShipmentsSqlQuery + dropSqlQuery;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }

        }

        internal static string AffectOnPostgreSqlTmpPalletTableSqlQuery(this TmpBatchTableAction action)
        {
            switch (action)
            {
                case TmpBatchTableAction.Create:
                    {
                        const string createSqlQuery = "CREATE TABLE public.\"TmpPallets\"(" +
                                                      "\"Id\" uuid NOT NULL," +
                                                      "\"CreateDateUtc\" timestamp without time zone NOT NULL," +
                                                      "\"DeviceId\" uuid," +
                                                      "\"Identifier\" character varying(36) COLLATE pg_catalog.\"default\" NOT NULL," +
                                                      "\"IsEmpty\" boolean NOT NULL," +
                                                      "\"Name\" text COLLATE pg_catalog.\"default\"," +
                                                      "\"ShipmentIdentifier\" text COLLATE pg_catalog.\"default\"," +
                                                      "\"Status\" integer NOT NULL," +
                                                      "\"UpdateDateUtc\" timestamp without time zone NOT NULL," +
                                                      "\"ProductionLineId\" uuid," +
                                                      "\"StorageId\" uuid," +
                                                      "\"WorkplaceId\" integer," +
                                                      "\"PartNumber\" text COLLATE pg_catalog.\"default\"," +
                                                      "\"ProductId\" uuid);";

                        return createSqlQuery;
                    }
                case TmpBatchTableAction.TransferDataAndDrop:
                    {
                        const string transferDataWithShipmentsSqlQuery =
                            "INSERT INTO public.\"Pallets\"(\"Id\", \"Identifier\", \"Name\", \"IsEmpty\", \"CreateDateUtc\", \"UpdateDateUtc\", \"Status\", \"DeviceId\", \"ShipmentId\", \"ProductionLineId\", \"StorageId\", \"ProductId\", \"WorkplaceId\", \"PartNumber\") " +
                            "SELECT p.\"Id\", p.\"Identifier\", p.\"Name\", p.\"IsEmpty\", p.\"CreateDateUtc\", p.\"UpdateDateUtc\", p.\"Status\", p.\"DeviceId\", s.\"Id\", p.\"ProductionLineId\", p.\"StorageId\", p.\"ProductId\", p.\"WorkplaceId\", p.\"PartNumber\" " +
                            "FROM public.\"TmpPallets\" p INNER JOIN public.\"Shipments\" s ON p.\"ShipmentIdentifier\" = s.\"Identifier\" WHERE p.\"ShipmentIdentifier\" IS NOT NULL;";

                        const string transferDataWithoutShipmentsSqlQuery =
                            "INSERT INTO public.\"Pallets\"(\"Id\", \"Identifier\", \"Name\", \"IsEmpty\", \"CreateDateUtc\", \"UpdateDateUtc\", \"Status\", \"DeviceId\", \"ProductionLineId\", \"StorageId\", \"ProductId\", \"WorkplaceId\", \"PartNumber\") " +
                            "SELECT \"Id\", \"Identifier\", \"Name\", \"IsEmpty\", \"CreateDateUtc\", \"UpdateDateUtc\", \"Status\", \"DeviceId\", \"ProductionLineId\", \"StorageId\", \"ProductId\", \"WorkplaceId\", \"PartNumber\" FROM public.\"TmpPallets\" " +
                            "WHERE \"ShipmentIdentifier\" IS NULL;";

                        const string dropSqlQuery = "DROP TABLE public.\"TmpPallets\";";

                        return transferDataWithShipmentsSqlQuery + transferDataWithoutShipmentsSqlQuery + dropSqlQuery;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}
