using APEXAContracting.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace APEXAContracting.Model.DTO.Application
{
    /// <summary>
    ///  Note: This context constains processing data for current logon user and his/her current processing required basic parameters.
    ///  Web api application should not maintain processing state in backend. Stateless.
    /// </summary>
    public class AppContext
    {
        public AppContext()
        {
            this.Language = Enumerations.Language.EN;
            this.TimeZoneOffset = 0;
        }
    
        /// <summary>
        /// Current language's enumeration definition.
        /// 
        /// Default language is en-us
        /// </summary>
        public Enumerations.Language Language { get; set; }

        /// <summary>
        ///  string langauge code.
        ///  value = "en-us", "fr-ca", "es-us", "zh-chs".
        ///  Reference to field "Code" in table dbo.Langauge.
        ///  Same as Language.
        /// </summary>
        public string LanguageCode { get; set; }


        /// <summary>
        /// client browser timezone offset.
        /// It is value of hours offset for timezone. negative value. for example value = -5 for Eastern Standard Time to UTC.
        /// based on javascript client side method "-(new Date().getTimezoneOffset()/60)".
        /// Default value = 0.
        /// </summary>
        public int TimeZoneOffset { get; set; }

        public string UserName { get; set; }
     
    }
}
