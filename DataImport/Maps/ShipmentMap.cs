using CsvHelper.Configuration;
using DataImport.Converters;
using DataImport.Entities;

namespace DataImport.Maps
{
    internal sealed class ShipmentMap : ClassMap<Shipment>
    {
        public ShipmentMap()
        {
            Map(x => x.Id).Name("id");
            Map(x => x.Identifier).Name("identifier");
            Map(x => x.Name).Name("name");
            Map(x => x.DocumentNumber).Name("document_number");
            Map(x => x.RequestNumber).Name("request_number");
            Map(x => x.DriverName).Name("driver_name");
            Map(x => x.Comment).Name("comment");
            Map(x => x.CompanyId).Name("company_id");
            Map(x => x.RecipientCompanyId).Name("consignee_company_id");
            Map(x => x.StorageId).Name("storage_id");
            Map(x => x.CreateDateUtc).TypeConverter<DateTimeConverter>().Name("created_at");
            Map(x => x.UpdateDateUtc).TypeConverter<DateTimeConverter>().Name("updated_at");
        }

        /*
        ToDo Differences:
        - local: DeviceId 
        - cloud: inserted_at, modified_at, acceptance_status, employee_id
        */
    }
}
