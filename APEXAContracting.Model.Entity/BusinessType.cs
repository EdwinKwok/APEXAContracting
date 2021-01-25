using System;
using System.Collections.Generic;

namespace APEXAContracting.Model.Entity
{
    public partial class BusinessType
    {
        public BusinessType()
        {
            BusinessUnit = new HashSet<BusinessUnit>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<BusinessUnit> BusinessUnit { get; set; }
    }
}
