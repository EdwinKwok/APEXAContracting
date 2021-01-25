using System;
using System.Collections.Generic;

namespace APEXAContracting.Model.Entity
{
    public partial class BusinessUnit
    {
        public BusinessUnit()
        {
            ContractAcceptedBy = new HashSet<Contract>();
            ContractOfferedBy = new HashSet<Contract>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string HierarchyPrefix { get; set; }
        public string HierarchyKey { get; set; }
        public byte HealthStatusId { get; set; }
        public byte BusinessTypeId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual BusinessType BusinessType { get; set; }
        public virtual HealthStatus HealthStatus { get; set; }
        public virtual ICollection<Contract> ContractAcceptedBy { get; set; }
        public virtual ICollection<Contract> ContractOfferedBy { get; set; }
    }
}
