using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using DataImport.Entities;

namespace DataImport.Maps
{
    internal sealed class BottleMap : ClassMap<Bottle>
    {
        public BottleMap()
        {
            Map(x => x.Id).Name("id");
            Map(x => x.Identifier).Name("identifier");
            Map(x => x.ExciseDutyNumber).Name("excise_duty_number");
            Map(x => x.ExciseDutyNumber2).Name("excise_duty_number2");
            Map(x => x.Status).Name("status");
            Map(x => x.CompanyId).Name("company_id");
            Map(x => x.RecipientCompanyId).Name("consignee_company_id");
            Map(x => x.ProductionLineId).Name("production_line_id");
            Map(x => x.BatchId).Name("batch_id");
            Map(x => x.ProductId).Name("product_id");
            Map(x => x.CreateDateUtc).TypeConverter<DateTimeConverter>().Name("created_at");
            Map(x => x.UpdateDateUtc).TypeConverter<DateTimeConverter>().Name("updated_at");
        }

        /*
        ToDo Differences:
        - local: BatchIdentifier
        - cloud: inserted_at, modified_at, storage_id, party, employee_id, picture, product_name, device_id, note
        */
    }
}
