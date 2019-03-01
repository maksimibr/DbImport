using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace DataImport
{
    internal static class CsvRecordsReader
    {
        internal static IEnumerable<ICollection<T>> Read<T, TMap>(string path, int segmentSize = 100)
            where TMap : ClassMap
        {
            var segment = new List<T>();
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.RegisterClassMap<TMap>();
                using (var recordsEnumerator = csv.GetRecords<T>().GetEnumerator())
                {
                    while (recordsEnumerator.MoveNext())
                    {
                        segment.Add(recordsEnumerator.Current);
                        if (segment.Count == segmentSize)
                        {
                            yield return segment;
                            segment.Clear();
                        }
                    }

                    if (segment.Any())
                    {
                        yield return segment;
                    }
                }
            }
        }
    }
}
