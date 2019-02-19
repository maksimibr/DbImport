using System;

namespace DataImport.Entities
{
    internal class Bottle
    {
        internal Guid Id { get; set; }

        internal string Identifier { get; set; }

        internal string ExciseDutyNumber { get; set; }

        internal string ExciseDutyNumber2 { get; set; }

        internal int Status { get; set; }

        internal DateTime CreateDateUtc { get; set; }

        internal DateTime UpdateDateUtc { get; set; }

        internal Guid CompanyId { get; set; }

        internal Guid? RecipientCompanyId { get; set; }

        internal string BatchIdentifier { get; set; }

        internal Guid? ProductionLineId { get; set; }

        internal Guid? BatchId { get; set; }

        internal Guid? ProductId { get; set; }
    }
}
