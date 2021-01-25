using System;
using System.Collections.Generic;
using System.Text;

namespace APEXAContracting.Web.Common.Models
{
    public class BasePageCriteria : IPageCriteria
    {
        /// <summary>
        /// Skip # records for paging, for use in LINQ syntax
        /// </summary>
        public int Skip { get; set; }

        private int pageSize = 10;
        /// <summary>
        /// PageSize, sample as Top, a convention for LLBLGen
        /// </summary>
        public int PageSize { get { if (pageSize < 1) pageSize = 10; return pageSize; } set { pageSize = value; } }

        /// <summary>
        /// PageIndex is 1-based, i.e. first page = 1 (for LLBLGen FetchEntityCollection use)
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (PageSize == 0) return 1;
                return (int)(Skip / PageSize) + 1;
            }
        }


        public string FilterString { get; set; }

        public SortOperator Sort
        {
            get
            {
                return (this.SortDirection == PageSortDirection.Desc) ? SortOperator.Descending : SortOperator.Ascending;
            }
        }

        public string SortField { get; set; }

        public PageSortDirection SortDirection { get; set; }

        public string OrderBy
        {
            get
            {
                if (String.IsNullOrWhiteSpace(SortField))
                {
                    return null;
                }
                else
                {
                    return String.Format("{0} {1}", SortField, SortDirection.ToString().ToLowerInvariant());
                }
            }
        }

        /// <summary>
        /// use to handle sort field mapping when UI field name does not align with LLBLGen/Entity field name
        /// </summary>
        internal Dictionary<string, string> SortFieldNameMap;

        public BasePageCriteria()
        {
            this.SortFieldNameMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
    }

    public enum SortOperator
    {
        Ascending = 0,
        Descending = 1
    }
}