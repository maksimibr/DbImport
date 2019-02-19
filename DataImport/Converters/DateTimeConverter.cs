using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using DataImport.Extensions;

namespace DataImport.Converters
{
    internal class DateTimeConverter : ITypeConverter
    {
        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) =>
            ((DateTime) value).ToString("o").RemoveLast();

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) =>
            DateTime.Parse(text.Remove(text.Length - 4));
    }
}
