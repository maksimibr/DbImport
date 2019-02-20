using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using DataImport.Entities.Enums;

namespace DataImport.Converters
{
    internal class StatusConverter : ITypeConverter
    {
        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) =>
            ((Status) value).ToString();

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            switch (text.ToUpper())
            {
                case "PRODUCED":
                case "SHIPMENT_PENDING":
                    return Status.Produced;
                case "SHIPPED":
                    return Status.Shipped;
                case "REMOVED":
                    return Status.Removed;
                default:
                    throw new Exception("Conversion error");
            }
        }
    }
}
