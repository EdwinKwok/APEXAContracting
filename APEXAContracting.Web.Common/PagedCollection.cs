using System;
using System.Collections.Generic;
using System.Text;

namespace APEXAContracting.Web.Common
{
    /// <summary>
    /// For handling Collection Paging for Grid/List view
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedCollection<T> where T : class
    {
        public ICollection<T> Data { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int TotalPages
        {
            get
            {
                if (PageSize == 0) PageSize = 1;
                return (int)Math.Ceiling((decimal)Total / PageSize);
            }
        }
    }
}
