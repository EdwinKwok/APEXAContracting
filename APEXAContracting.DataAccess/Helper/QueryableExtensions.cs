using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APEXAContracting.DataAccess
{
    public static class QueryableExtensions
    {
        /// <summary>
        ///  Apply pagination in sql server side. Note: Before call method "Paged", developer need to sort the IQueryable<T> collection first.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="page">Current Page's Index. Default value = 1. Note: When doing paginnation, system will do page-1.</param>
        /// <param name="pageSize">Default value = 20.</param>
        /// <returns></returns>
        public static IQueryable<T> Paged<T>(this IQueryable<T> source, int page, int pageSize=20)
        {
            if (pageSize <= 0)
            {
                pageSize = 20;
            }

            return source
              .Skip((page-1) * pageSize)
              .Take(pageSize);
        }

    }
}
