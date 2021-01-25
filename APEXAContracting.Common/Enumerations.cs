using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace APEXAContracting.Common
{
    public static class Enumerations
    {
        /// <summary>
        ///  Reference to table Gender.
        /// </summary>
        public enum Gender
        {
            [Description("Female")]
            Female = 1,
            [Description("Male")]
            Maile = 2
        }

        /// <summary>
        ///
        ///  Note: Developer is able to modify following definitions based on business logic.
        ///  
        ///  Corporate with OneUIAuthorizeAttribute.cs
        ///  
        ///   Reference to IAM server database table [dbo].[Role].
        /// </summary>
        public enum Role
        {
            /// <summary>
            ///  System administrator has permissions to access/disable users. But no claims access permission.
            ///  Note: Create/Update/Delete user account need to go through Azure AD.
            /// </summary>
            [Description("Administrator")]
            Administrator = 1,

            /// <summary>
            ///  Manager has permissions to access/update/create/delete claims and access users info (but no disabled users permissions)
            /// </summary>
            [Description("Manager")]
            Manager = 2,

            /// <summary>
            ///  Client only has permissions to access his/her own claims and submit his/her claims. 
            /// </summary>
            [Description("Client")]
            Client = 3
        }

        /// <summary>
        /// Note: Developer is able to modify following definitions based on business logic.
        /// 
        /// Corporate with OneUIAuthorizeAttribute.cs
        ///    
        /// Reference to IAM server database table [Permission].
        /// </summary>
        public enum Permission
        {           
            // TODO.
        }

        /// <summary>
        ///  Reference to records in table dbo.Subject.
        /// </summary>
        public enum Subject
        {

            [Description("COVID-19")]
            Covid19 = 1,
        }

        /// <summary>
        ///  Multiple languages support. Corporate with dbo.Localization table.
        ///  Note: Records same value as table [dbo].[Language].
        /// </summary>
        public enum Language
        {
            /// <summary>
            /// Default is en-us. United State English. en-us
            /// </summary>
            [Description("English")]
            EN = 1,
            /// <summary>
            ///  Canadian French. fr-ca
            /// </summary>
            [Description("French")]
            FR = 2,
        }

        /// <summary>
        ///  Reference to table IsolationStatus.
        /// </summary>
        public enum IsolationStatus
        {
            [Description("Safe")]
            Safe = 1,
            [Description("No Signal")]
            NoSignal = 2,
            [Description("Suspicious")]
            Suspicious = 3,
            [Description("Escape")]
            Escape = 4
        }

        /// <summary>
        ///  Referene to table IdentityType.
        /// </summary>
        public enum IdentityType {
            [Description("Driver License")]
            DriverLicense = 1,
            [Description("Health Card")]
            HealthCard = 2,
            [Description("Passport")]
            Passport = 3,
        }

    }
}
