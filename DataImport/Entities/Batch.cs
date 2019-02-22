using System;
using DataImport.Entities.Enums;

namespace DataImport.Entities
{
    public class Batch
    {
        public Guid Id { get; set; }

        public string Identifier { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }

        public Status Status { get; set; }

        public DateTime CreateDateUtc { get; set; }

        public DateTime UpdateDateUtc { get; set; }

        public bool IsEmpty { get; set; }

        public string PalletIdentifier { get; set; }

        public string PartNumber { get; set; }

        public Guid CompanyId { get; set; }

        public Guid? DeviceId { get; set; }

        public Guid? RecipientCompanyId { get; set; }

        public string ShipmentIdentifier { get; set; }

        public Guid? ProductId { get; set; }

        public Guid? ProductionLineId { get; set; }

        public Guid? StorageId { get; set; }

        public int? WorkplaceId { get; set; }
    }
}
