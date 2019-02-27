using System;
using DataImport.Entities.Enums;

namespace DataImport.Entities
{
    public class Bottle
    {
        public string Id { get; set; }

        public string Identifier { get; set; }

        public string ExciseDutyNumber { get; set; }

        public string ExciseDutyNumber2 { get; set; }

        public Status Status { get; set; }

        public DateTime CreateDateUtc { get; set; }

        public DateTime UpdateDateUtc { get; set; }

        public string CompanyId { get; set; }

        public string RecipientCompanyId { get; set; }

        public string BatchIdentifier { get; set; }

        public string ProductionLineId { get; set; }

        public string BatchId { get; set; }

        public string ProductId { get; set; }
    }
}
