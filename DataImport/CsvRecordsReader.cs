using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace DataImport
{
    internal static class CsvRecordsReader
    {
        internal static IEnumerable<T> Read<T, TMap>(string path) where TMap : ClassMap
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.RegisterClassMap<TMap>();
                var records = csv.GetRecords<T>();

                return records;
            }
        }
    }
}
