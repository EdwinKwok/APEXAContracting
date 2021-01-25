using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace APEXAContracting.Web.Common
{
    /// <summary>
    /// default sorting direction settings
    /// </summary>
    public enum PageSortDirection
    {
        /// <summary>
        /// Ascending
        /// </summary>
        [Description("Ascending")]
        Asc = 0,
        /// <summary>
        /// Descending
        /// </summary>
        [Description("Descending")]
        Desc = 1
    }

    /// <summary>
    /// Common interface for defining Search Criteria with common paging parameters, property names are based on OData v3 format
    /// </summary>
    public interface IPageCriteria
    {
        /// <summary>
        /// same as Take or PageSize
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// how many records to skip ahead
        /// </summary>
        int Skip { get; set; }

        string FilterString { get; set; }

        string SortField { get; set; }

        PageSortDirection SortDirection { get; set; }

        string OrderBy { get; }
    }
}
