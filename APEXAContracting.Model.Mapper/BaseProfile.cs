using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace APEXAContracting.Model.Mapper
{
    /// <summary>
    ///  Reference: http://docs.automapper.org/en/stable/Configuration.html#profile-instances
    /// </summary>
    public class BaseProfile : Profile
    {
        /// <summary>
        ///  Return current culture code. suchas "en-ca".
        ///  Corporate with RequestCultureMiddleware.cs. 
        ///  Working for multiple languages support.
        ///  Corporate with Lookup values.
        /// </summary>
        public string CurrentCultureCode {
            get {
                return CultureInfo.CurrentCulture.Name ;
            }
        }
    }
}
