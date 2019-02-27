using System;

namespace DataImport.Entities
{
    public class Shipment
    {
        public string Id { get; set; }

        public string Identifier { get; set; }

        public string Name { get; set; }

        public string DocumentNumber { get; set; }

        public string RequestNumber { get; set; }

        public string DriverName { get; set; }

        public string Comment { get; set; }

        public string DeviceId { get; set; }

        public DateTime CreateDateUtc { get; set; }

        public DateTime UpdateDateUtc { get; set; }

        public string CompanyId { get; set; }

        public string RecipientCompanyId { get; set; }

        public string StorageId { get; set; }
    }
}
