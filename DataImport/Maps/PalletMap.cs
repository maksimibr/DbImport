using CsvHelper.Configuration;
using DataImport.Entities;

namespace DataImport.Maps
{
    internal sealed class PalletMap : ClassMap<Pallet>
    {
        public PalletMap()
        {
            Map(x => x.Id).Name("id");
            Map(x => x.Identifier).Name("identifier");
            Map(x => x.Name).Name("name");
            Map(x => x.CreateDateUtc).Name("created_at");
            Map(x => x.UpdateDateUtc).Name("updated_at");
            Map(x => x.Status).Name("status");
            Map(x => x.DeviceId).Name("device_id");
            Map(x => x.ShipmentId).Name("storage_id");
            Map(x => x.ProductionLineId).Name("production_line_id");
            Map(x => x.StorageId).Name("storage_id");
            Map(x => x.ProductId).Name("product_id");
            Map(x => x.WorkplaceId).Name("device_workplace_id");
            Map(x => x.PartNumber).Name("party");
        }

        /*
        ToDo Differences:
        - local: IsEmpty 
        - cloud: company_id, inserted_at, modified_at, shipment_identifier, consignee_company_id, employee_id, product_name
        */
    }
}
