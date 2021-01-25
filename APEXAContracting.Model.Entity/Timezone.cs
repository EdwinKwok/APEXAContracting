using System;
using System.Collections.Generic;

namespace APEXAContracting.Model.Entity
{
    public partial class Timezone
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Offset { get; set; }
    }
}
