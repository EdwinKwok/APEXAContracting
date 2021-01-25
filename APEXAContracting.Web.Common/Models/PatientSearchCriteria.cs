using System;
using System.Collections.Generic;
using System.Text;

namespace APEXAContracting.Web.Common.Models
{
    public class PatientSearchCriteria : BasePageCriteria
    {
        public Guid? NurseId { get; set; }
        public Guid? DoctorId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string SearchKey { get; set; }
        public bool IncludeDischarge { get; set; }
        public bool SendAllActivePatients { get; set; }
    }
}
