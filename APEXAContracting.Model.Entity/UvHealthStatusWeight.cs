using System;
using System.Collections.Generic;

namespace APEXAContracting.Model.Entity
{
    public partial class UvHealthStatusWeight
    {
        public byte Id { get; set; }
        public int? TallyStart { get; set; }
        public int? TallyEnd { get; set; }
    }
}
