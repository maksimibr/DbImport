using System;
using DataImport.Entities.Enums;

namespace DataImport.Extensions
{
    internal static class SqlQueryExtension
    {
        internal static string GetValue(this Guid? input) => !input.HasValue ? "NULL" : $"'{input.Value}'";
        internal static string GetValue(this Guid input) => $"'{input}'";
        internal static string GetValue(this DateTime input) => $"'{input.ToString("o").RemoveLast()}'";
        internal static string GetValue(this int? input) => !input.HasValue ? "NULL" : input.Value.ToString();
        internal static string GetValue(this Status input) => ((int)input).ToString();
        internal static string GetValue(this string input) => string.IsNullOrEmpty(input) ? "NULL" : $"N'{input}'";
        internal static string GetValue(this bool input, DatabaseType databaseType) =>
            databaseType == DatabaseType.SqlServer
                ? input ? "1" : "0"
                : input ? "True" : "False";
    }
}
