using System;
using DataImport.Entities.Enums;

namespace DataImport.Entities
{
    public class Pallet
    {
        public Guid Id { get; set; }

        public string Identifier { get; set; }

        public string Name { get; set; }

        public bool IsEmpty { get; set; }

        public DateTime CreateDateUtc { get; set; }

        public DateTime UpdateDateUtc { get; set; }

        public Status Status { get; set; }

        public Guid? DeviceId { get; set; }

        public Guid? ProductionLineId { get; set; }

        public Guid? StorageId { get; set; }

        public Guid? ProductId { get; set; }

        public int? WorkplaceId { get; set; }

        public string PartNumber { get; set; }

        public string ShipmentIdentifier { get; set; }
    }
}