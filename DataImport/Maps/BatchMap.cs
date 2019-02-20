using CsvHelper.Configuration;
using DataImport.Converters;
using DataImport.Entities;

namespace DataImport.Maps
{
    internal sealed class BatchMap : ClassMap<Batch>
    {
        public BatchMap()
        {
            Map(x => x.Id).Name("id");
            Map(x => x.Identifier).Name("identifier");
            Map(x => x.Name).Name("name");
            Map(x => x.Comment).Name("comment");
            Map(x => x.PalletIdentifier).Name("pallet_identifier");
            Map(x => x.PartNumber).Name("party");
            Map(x => x.CompanyId).Name("company_id");
            Map(x => x.DeviceId).Name("device_id");
            Map(x => x.RecipientCompanyId).Name("consignee_company_id");
            Map(x => x.ProductId).Name("product_id");
            Map(x => x.ProductionLineId).Name("production_line_id");
            Map(x => x.StorageId).Name("storage_id");
            Map(x => x.WorkplaceId).Name("device_workplace_id");
            Map(x => x.Status).TypeConverter<StatusConverter>().Name("status");
            Map(x => x.CreateDateUtc).TypeConverter<DateTimeConverter>().Name("created_at");
            Map(x => x.UpdateDateUtc).TypeConverter<DateTimeConverter>().Name("updated_at");
            //ToDo Map(x => x.ShipmentId).Name("");
        }

        /*
        ToDo Differences:
        - local: IsEmpty
        - cloud: inserted_at, modified_at, shipment_identifier, employee_id, product_name, bottles_size, alc_code
        */
    }
}
