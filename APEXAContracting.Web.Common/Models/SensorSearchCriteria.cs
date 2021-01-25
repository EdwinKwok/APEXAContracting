using System;
using System.Collections.Generic;
using System.Text;

namespace APEXAContracting.Web.Common.Models
{
    public class SensorSearchCriteria: BasePageCriteria
    {
        public string SearchKey { get; set; }
        public string SignalTypeIds { get; set; }
        public bool ShowAvailableOnly { get; set; }
    }
}
