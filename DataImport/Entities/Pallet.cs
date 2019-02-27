using System;
using DataImport.Entities.Enums;

namespace DataImport.Entities
{
    public class Pallet
    {
        public string Id { get; set; }

        public string Identifier { get; set; }

        public string Name { get; set; }

        public bool IsEmpty { get; set; }

        public DateTime CreateDateUtc { get; set; }

        public DateTime UpdateDateUtc { get; set; }

        public Status Status { get; set; }

        public string DeviceId { get; set; }

        public string ShipmentId { get; set; }

        public string ProductionLineId { get; set; }

        public string StorageId { get; set; }

        public string ProductId { get; set; }

        public int? WorkplaceId { get; set; }

        public string PartNumber { get; set; }
    }
}