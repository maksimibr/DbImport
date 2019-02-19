using System;

namespace DataImport.Entities
{
    internal class Shipment
    {
        internal Guid Id { get; set; }

        internal string Identifier { get; set; }

        internal string Name { get; set; }

        internal string DocumentNumber { get; set; }

        internal string RequestNumber { get; set; }

        internal string DriverName { get; set; }

        internal string Comment { get; set; }

        internal Guid? DeviceId { get; set; }

        internal DateTime CreateDateUtc { get; set; }

        internal DateTime UpdateDateUtc { get; set; }

        internal Guid CompanyId { get; set; }

        internal Guid? RecipientCompanyId { get; set; }

        internal Guid StorageId { get; set; }
    }
}
