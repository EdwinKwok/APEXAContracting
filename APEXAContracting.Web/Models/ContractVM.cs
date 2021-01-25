using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace APEXAContracting.Web.Models
{
    public class ContractVM
    {
        public Guid Id { get; set; }
        [Display(Prompt = "6DB5A9E3-DC5D-EB11-B383-2016B97D0838")]
        public Guid OfferedById { get; set; }
        [Display(Prompt = "6DB5A9E3-DC5D-EB11-B383-2016B97D0838")]
        public Guid AcceptedById { get; set; }
        [Display(Name = "Contract offered by", Prompt = "CAR<00001>")]
        public string OfferedByKey { get; set; }
        [Display(Name = "Contract accepted by", Prompt = "CAR<00001>")]
        public string AcceptedBykey { get; set; }
        public string ContractName { get; set; }
        public DateTime EffectedOn { get; set; }
        public DateTime ExpiredOn { get; set; }
        [Display(Name = "IsTerminated")]
        public bool IsDeleted { get; set; }
        public bool IsExpired { get; set; }
        public string ContractPath { get; set; }
    }
}
