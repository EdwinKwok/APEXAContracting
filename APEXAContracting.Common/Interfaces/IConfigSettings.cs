using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace APEXAContracting.Common.Interfaces
{
    /// <summary>
    ///  Reference: https://blogs.technet.microsoft.com/dariuszporowski/tip-of-the-week-how-to-access-configuration-from-controller-in-asp-net-core-2-0/
    /// </summary>
    public interface IConfigSettings
    {

        IConfiguration Config { get; }

        /// <summary>
        /// Auzre AD authentication setup.
        /// </summary>
        string AzureADAuthority { get; }

        /// <summary>
        /// Auzre AD authentication client id.
        /// </summary>
        string AzureADClientId { get; }

        string AzureADTenant { get; }
        /// <summary>
        ///  
        /// Azure AD registered application's preset client secret.
        /// Check this value under "Certificates & secrets" section under "Identity Server" application in Azure AD.
        ///
        /// </summary>
        string AzureADClientSecret { get; }

        /// <summary>
        /// Work for call Azure Graph APIs. It defined API version.
        /// Note:  For B2C user managment, be sure to use the 1.6 Graph API version.
        /// Reference: https://github.com/AzureADQuickStarts/B2C-GraphAPI-DotNet/blob/master/B2CGraphClient/B2CGraphClient.cs
        /// https://github.com/AzureADQuickStarts/B2C-GraphAPI-DotNet/blob/master/B2CGraphClient/Globals.cs
        /// </summary>
        string AzureADGraphVersion { get; }

        #region SQL Server Database Connections

        /// <summary>
        ///  Database connection for MySql/Sql Server database.
        /// </summary>
        string DatabaseConnection { get; }
       
        /// <summary>
        /// IAM database connection string in appsettings.json.
        /// </summary>
        string IAMDatabaseConnection { get; }


        #endregion

        /// <summary>
        ///  Reference to field "Code" in table dbo.Application.
        ///  Current Application's specified code.
        /// </summary>
        string ApplicationCode { get; }

        /// <summary>
        ///  Default langauge setting for application.
        /// </summary>
        string DefaultLanguage { get; }


        /// <summary>
        /// Integer value of Minutes. Default value = 5 minutes. Note: This is important to solve problem in client's infinit looping in login which caused by client's local machine datetime setting is not correct.
        /// Set in Web API resource portal.
        /// Reference: https://github.com/IdentityServer/IdentityServer4.AccessTokenValidation/issues/90
        /// https://github.com/IdentityServer/IdentityServer4/issues/497
        /// 
        /// </summary>
        int IdentityServerJwtValidationClockSkew { get; }


        /// <summary>
        ///  Identity Server address. Work for user authentication.
        /// </summary>
        string IAMUrl { get; }

        /// <summary>
        ///  Public WEB Api Server address.
        /// </summary>
        string ApiRootUrl { get; }

        /// <summary>
        ///  Allow accessing web api portal domains. Same settings as records in table dbo.ClientDomain.
        /// </summary>
        string[] AllowedDomains { get; }

        /// <summary>
        ///  Called in IdentityServer project.
        ///  It is portal url for IsolationPeople.Tracker.Web
        /// Corporate with Config.cs clients setup.
        /// </summary>
        string MVCClientDomain { get; }
        ///// <summary>
        ///// Setting only for Identity Server. 
        ///// Integer value of days. 
        ///// When checked "Remember me" in login UI, system will apply this setting to cookie expiration UTC date. 
        ///// Now, It is 30 days before login session got expired.
        ///// </summary>
        //TimeSpan RememberMeLoginDuration { get; }

        ///// <summary>
        ///// Control if the test data in the "if DEBUG" block added to the result set, by default is true can be disable in personal setting file
        ///// </summary>
        //bool AddDataWhenDebuggerRunning { get; }
             

        /// <summary>
        /// Work for local host test only. Check domain from field "TestDomain" in table dbo.ClientDomain.
        /// Work for accessing domain validation in identity server.
        /// Note: For QA, UAT, Production server, this setting should be false.
        /// Only work for developer debuging codes in localhost.
        /// </summary>
        bool IsLocalHostTest { get; }

        /// <summary>
        ///  Solution for resolving issues in each time identity server republish,
        ///  the previous login user's data protection keys will be gone. and those end user cannot login again.
        ///  TODO. This solution could only working on one IIS node. If your application is running under load balance setup, it will not work.
        /// </summary>
        string DataProtectionKeysFolderPath { get; }


        #region Email Setting
        string FromEmail { get; }

        string MailServer { get; }

        int SMTPServerPort { get; }

        string SMTPUser { get; }

        string SMTPPassword { get; }

        bool SMTPEnableSsL { get; }

        /// <summary>
        ///  For forget password, reset password process, email with generated password also will send to this administrator email address.
        ///  setup dynamic tire help desk email here. helpdesk@dynamictire.com
        ///  It can support multiple emails with format email1;email2;email3;email4
        /// </summary>
        string AdministratorEmail { get; }


        /// <summary>
        /// If IsNotificationTest = true, system will send email to "AdministratorEmail" only and will not send email to real email receiver.
        /// Work for TaskScheduler.Notification and other notifications.
        /// Corporate with EmailHelper.
        /// </summary>
        bool IsNotificationTest { get; }


        /// <summary>
        ///  local database connection timeout setting.
        ///  It is integer value of seconds.
        ///  Default value = 360 seconds.
        /// </summary>
        int DatabaseCommandTimeout { get; }

        #endregion

        #region Google ReCaptcha
        /// <summary>
        ///  Define enable or disable Google reCaptcha Secret. Work for forgot password UI. Note: For Localhost debugging, set value = false.
        /// </summary>
        bool EnableReCaptcha { get; }

        /// <summary>
        ///  Google reCaptcha Secret. Work for forgot password UI.
        /// </summary>
        string ReCaptchaSecret { get; }

        /// <summary>
        /// Google reCaptcha SiteKey. Work for forgot password UI.
        /// </summary>
        string ReCaptchaSiteKey { get; }

        #endregion


        #region Run with Debugger valuese
        /// <summary>
        /// WarrantyEmailTo when run with Debugger
        /// </summary>
        string WithDebuggerWarrantyEmailTo { get; }
        #endregion


        /// <summary>
        ///   Note: This setting only work for one specified client (angular 7).
        /// </summary>
        string SPAClientDomain { get; }


        #region Azure Storage Account Settings. Get Information from Azure Portal, Storage Accounts -> Access keys.  Note: Disabled in First ON Site Solution.
        string AzureStorageAccount { get; }        

        string AzureStorageKey1 { get; }
        
        string AzureStorageConnection1 { get; }
        
        string AzureStorageKey2 { get; }
        
        string AzureStorageConnection2 { get; }

        string AzureStorageContainerName { get; }
        // string SynchronizeFileDirectory { get; }
        #endregion


        #region Azure Storage Sync Job Service settings
        /// <summary>
        ///  Predefine what regions need to sync file. Corporate with FOS_Common.dbo.Region table records.
        /// Hardcode setting. Split regionId with ",". 
        /// Work for syn job debuging and testing environment.
        /// When go live, we can set this setting as empty string or include all regionIds which need to sync files to Azure Blob Storage.
        /// </summary>
        int[] SyncRegionIds { get; }

        int NumberOfCTFilePerSyncLimition { get; }
        #endregion



    }
}
