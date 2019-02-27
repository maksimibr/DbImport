using System;

namespace DataImport.Entities
{
    public class Shipment
    {
        public Guid Id { get; set; }

        public string Identifier { get; set; }

        public string Name { get; set; }

        public string DocumentNumber { get; set; }

        public string RequestNumber { get; set; }

        public string DriverName { get; set; }

        public string Comment { get; set; }

        public Guid? DeviceId { get; set; }

        public DateTime CreateDateUtc { get; set; }

        public DateTime UpdateDateUtc { get; set; }

        public Guid CompanyId { get; set; }

        public Guid? RecipientCompanyId { get; set; }

        public Guid StorageId { get; set; }
    }
}
