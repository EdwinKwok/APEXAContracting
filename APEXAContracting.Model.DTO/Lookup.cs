using System;
using System.Collections.Generic;
using System.Text;

namespace APEXAContracting.Model.DTO
{
    public class Lookup
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string ParentKey { get; set; }
        public string Code { get; set; }
    }

    public class LookupNum
    {
        public Nullable<int> Key { get; set; }
        public string Value { get; set; }
        
    }
    public class LookupMultiple
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public IEnumerable<LookupMultiple> Data { get; set; }
    }
}
