using System.Runtime.Serialization;

namespace DataImport.Entities.Enums
{
    public enum Status
    {
        [EnumMember(Value = "produced")] Produced = 1,
        [EnumMember(Value = "shipped")] Shipped = 2,
        [EnumMember(Value = "removed")] Removed = 3
    }
}
