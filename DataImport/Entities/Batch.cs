using System;

namespace DataImport.Entities
{
    internal class Batch
    {
        internal Guid Id { get; set; }

        internal string Identifier { get; set; }

        internal string Name { get; set; }

        internal string Comment { get; set; }

        internal int Status { get; set; }

        internal DateTime CreateDateUtc { get; set; }

        internal DateTime UpdateDateUtc { get; set; }

        internal bool IsEmpty { get; set; }

        internal string PalletIdentifier { get; set; }

        internal string PartNumber { get; set; }

        internal Guid CompanyId { get; set; }

        internal Guid? DeviceId { get; set; }

        internal Guid? RecipientCompanyId { get; set; }

        internal Guid? ShipmentId { get; set; }

        internal Guid? ProductId { get; set; }

        internal Guid? ProductionLineId { get; set; }

        internal Guid? StorageId { get; set; }

        internal int? WorkplaceId { get; set; }
    }
}
