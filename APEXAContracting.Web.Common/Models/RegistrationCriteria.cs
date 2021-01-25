using System;
using System.Collections.Generic;
using System.Text;

namespace APEXAContracting.Web.Common.Models
{
    public class RegistrationCriteria
    {
        public Guid RegId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
