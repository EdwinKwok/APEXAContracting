using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace APEXAContracting.Web.Models
{
    public class BusinessUnitVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string Address { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter a valid Phone!")]
        public string Phone { get; set; }
        public string HierarchyPrefix { get; set; }
        public string HierarchyKey { get; set; }
        public byte HealthStatusId { get; set; }
        public byte BusinessTypeId { get; set; }
        public bool IsDeleted { get; set; } = false;
        [Display(Name = "Business Type")]
        public string BusinessTypeName { get; set; }
        [Display(Name = "Health Status")]
        public string HealthStatusColor { get; set; }
        //non database fields
        public bool IsNewRecord { get; set; } = true;
    }
}
