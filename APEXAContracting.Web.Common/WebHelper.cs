using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using APEXAContracting.Common;

namespace APEXAContracting.Web.Common
{
    public static class WebHelper
    {
        /// <summary>
        ///  Collect errors from ModelState.
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetModelStateErrors(this ModelStateDictionary modelState)
        {
            if (modelState != null)
            {
                var errors = new Dictionary<string, string>();
                modelState.Where(k => k.Value.Errors.Count > 0).ToList().ForEach(i =>
                {
                    var er = string.Join(" ", i.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                    errors.Add(i.Key, er);
                });
                return errors;
            }
            else
                return null;
        }       
    }
}
