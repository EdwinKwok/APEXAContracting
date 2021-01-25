using System;
using System.Collections.Generic;

namespace APEXAContracting.Model.Entity
{
    public partial class Contract
    {
        public Guid Id { get; set; }
        public Guid OfferedById { get; set; }
        public Guid AcceptedById { get; set; }
        public string OfferedByKey { get; set; }
        public string AcceptedBykey { get; set; }
        public string ContractName { get; set; }
        public DateTime EffectedOn { get; set; }
        public DateTime ExpiredOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsExpired { get; set; }
        public string ContractPath { get; set; }

        public virtual BusinessUnit AcceptedBy { get; set; }
        public virtual BusinessUnit OfferedBy { get; set; }
    }
}
