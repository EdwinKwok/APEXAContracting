using System;
using System.Collections.Generic;

namespace APEXAContracting.Model.Entity
{
    public partial class HealthStatus
    {
        public HealthStatus()
        {
            BusinessUnit = new HashSet<BusinessUnit>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }
        public byte Weight { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<BusinessUnit> BusinessUnit { get; set; }
    }
}
