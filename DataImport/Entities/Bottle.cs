using System;
using DataImport.Entities.Enums;

namespace DataImport.Entities
{
    public class Bottle
    {
        public Guid Id { get; set; }

        public string Identifier { get; set; }

        public string ExciseDutyNumber { get; set; }

        public string ExciseDutyNumber2 { get; set; }

        public Status Status { get; set; }

        public DateTime CreateDateUtc { get; set; }

        public DateTime UpdateDateUtc { get; set; }

        public Guid CompanyId { get; set; }

        public Guid? RecipientCompanyId { get; set; }

        public string BatchIdentifier { get; set; }

        public Guid? ProductionLineId { get; set; }

        public Guid? BatchId { get; set; }

        public Guid? ProductId { get; set; }
    }
}
