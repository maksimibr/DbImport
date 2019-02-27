using System.Runtime.Serialization;

namespace DataImport.Entities.Enums
{
    public enum DatabaseType
    {
        [EnumMember(Value = "SqlServer")] SqlServer = 1,
        [EnumMember(Value = "PostgreSql")] PostgreSql = 2
    }
}
