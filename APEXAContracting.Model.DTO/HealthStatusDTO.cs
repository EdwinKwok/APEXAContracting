using System;
using System.Collections.Generic;
using System.Text;

namespace APEXAContracting.Model.DTO
{
    public class HealthStatusDTO
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public byte Weight { get; set; }
        public bool IsDeleted { get; set; }
    }
}
