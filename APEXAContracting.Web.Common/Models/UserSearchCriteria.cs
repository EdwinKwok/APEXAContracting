using System;
using System.Collections.Generic;
using System.Text;

namespace APEXAContracting.Web.Common.Models
{
    public class UserSearchCriteria: BasePageCriteria
    {
        public string SearchKey { get; set; }
        public int StatusId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
