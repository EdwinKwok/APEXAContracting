using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APEXAContracting.Model.DTO
{

    public class BusinessUnitDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string HierarchyPrefix { get; set; }
        public string HierarchyKey { get; set; }
        public byte HealthStatusId { get; set; }
        public byte BusinessTypeId { get; set; }
        public bool IsDeleted { get; set; } = false;

        //non database fields
        public bool IsNewRecord { get; set; } = true;

    }
}
