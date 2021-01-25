using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using APEXAContracting.Common.Interfaces;
using System.Linq;
namespace APEXAContracting.Common
{
    /// <summary>
    ///  Note: This class need to corporate with Dependency inject IConfiguration which is registered in Web API project's StartUp.cs.
    ///  That is why it cannot be static.
    ///  Reference: https://blogs.technet.microsoft.com/dariuszporowski/tip-of-the-week-how-to-access-configuration-from-controller-in-asp-net-core-2-0/
    /// </summary>
    public class ConfigSettings : IConfigSettings
    {
        private IConfiguration _config;

        public ConfigSettings(IConfiguration config)
        {
            _config = config;
        }

        public IConfiguration Config { get { return this._config; } }

        /// <summary>
        /// Auzre AD authentication setup.
        /// </summary>
        public string AzureADAuthority
        {
            get
            {
                return _config.GetSection("App")["AzureADAuthority"];
            }
        }

        /// <summary>
        /// Auzre AD authentication client id.
        /// </summary>
        public string AzureADClientId
        {
            get
            {
                return _config.GetSection("App")["AzureADClientId"];
            }
        }

        public string AzureADTenant 
        {
            get 
            {
                return _config.GetSection("App")["AzureADTenant"];
            }
        }
 

        /// <summary>
        ///  
        /// Azure AD registered application's preset client secret.
        /// Check this value under "Certificates & secrets" section under "Identity Server" application in Azure AD.
        ///
        /// </summary>
        public string AzureADClientSecret 
        {
            get {
                return _config.GetSection("App")["AzureADClientSecret"];
            }
        }

        /// <summary>
        /// Work for call Azure Graph APIs. It defined API version.
        /// Note:  For B2C user managment, be sure to use the 1.6 Graph API version.
        /// Reference: https://github.com/AzureADQuickStarts/B2C-GraphAPI-DotNet/blob/master/B2CGraphClient/B2CGraphClient.cs
        /// https://github.com/AzureADQuickStarts/B2C-GraphAPI-DotNet/blob/master/B2CGraphClient/Globals.cs
        /// </summary>
        public string AzureADGraphVersion 
        {
            get 
            {
                return _config.GetSection("App")["AzureADGraphVersion"];
            }
        }

        #region SQL Server Database connections
                
        /// <summary>
        ///  Database connection for MySql/SqlServer database.
        /// </summary>
        public string DatabaseConnection
        {
            get
            {
                return _config.GetConnectionString("DatabaseConnection");
            }
        }


             /// <summary>
        /// IAM database connection string in appsettings.json.
        /// </summary>
        public string IAMDatabaseConnection
        {
            get
            {
                return _config.GetConnectionString("IAMDatabase");
            }
        }

   
        #endregion


        /// <summary>
        ///  Reference to field "Code" in table dbo.Application.
        ///  Current Application's specified code.
        ///  Corporate with IAM.
        /// </summary>
        public string ApplicationCode
        {
            get
            {
                return _config.GetSection("App")["ApplicationCode"];
            }
        }

        /// <summary>
        ///  Default langauge setting for application.
        /// </summary>
        public string DefaultLanguage
        {
            get
            {
                return _config.GetSection("App")["DefaultLanguage"];
            }
        }

        /// <summary>
        /// Integer value of Minutes. Default value = 5 minutes. Note: This is important to solve problem in client's infinit looping in login which caused by client's local machine datetime setting is not correct.
        /// Set in Web API resource portal.
        /// Reference: https://github.com/IdentityServer/IdentityServer4.AccessTokenValidation/issues/90
        /// https://github.com/IdentityServer/IdentityServer4/issues/497
        /// 
        /// </summary>
        public int IdentityServerJwtValidationClockSkew
        {
            get
            {
                int result = 5;

                int.TryParse(_config.GetSection("App")["IdentityServerJwtValidationClockSkew"], out result);

                return result;
            }
        }

        /// <summary>
        ///  Identity Server address. Work for user authentication.
        /// </summary>
        public string IAMUrl
        {
            get
            {
                return _config.GetSection("App")["IAMUrl"];
            }
        }


        /// <summary>
        ///  Called in IdentityServer project.
        ///  It is portal url for IsolationPeople.Tracker.Web
        /// Corporate with Config.cs clients setup.
        /// </summary>
        public string MVCClientDomain { get { return _config.GetSection("App")["MVCClientDomain"]; } }

        /// <summary>
        ///  Public WEB Api Server address.
        /// </summary>
        public string ApiRootUrl
        {
            get
            {
                return _config.GetSection("App")["ApiRootUrl"];
            }
        }

        /// <summary>
        ///  allow accessing api portals domains. Same settings as records in table dbo.ClientDomain.
        /// </summary>
        public string[] AllowedDomains
        {
            get
            {
                return _config.GetSection("App")["AllowedDomains"].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
        }


        ///// <summary>
        ///// Setting only for Identity Server. 
        ///// Integer value of days. 
        ///// When checked "Remember me" in login UI, system will apply this setting to cookie expiration UTC date. 
        ///// Now, It is 30 days before login session got expired.
        ///// </summary>
        //public TimeSpan RememberMeLoginDuration
        //{
        //    get
        //    {
        //        int result = 30;

        //        int.TryParse(_config.GetSection("App")["RememberMeLoginDuration"], out result);
        //        return TimeSpan.FromDays(result);
        //    }
        //}

        ///// <summary>
        ///// Control if the test data in the "if DEBUG" block added to the result set, by default is true can be disable in personal setting file
        ///// </summary>
        //public bool AddDataWhenDebuggerRunning
        //{
        //    get
        //    {
        //        bool result = false;

        //        bool.TryParse(_config.GetSection("App")["AddDataWhenDebuggerRunning"], out result);
        //        return result;

        //    }
        //}
        

        #region Email Setting. Work for company portal and Identity server portal.
        public string FromEmail
        {
            get
            {
                return _config.GetSection("App")["FromEmail"];
            }
        }

        public string MailServer
        {
            get
            {
                return _config.GetSection("App")["MailServer"];
            }
        }

        public int SMTPServerPort
        {
            get
            {
                int port = 0;

                string portString = _config.GetSection("App")["SMTPServerPort"];

                if (int.TryParse(portString, out port))
                    return port;
                else
                    return 25;
            }
        }

        public string SMTPUser
        {
            get
            {
                return _config.GetSection("App")["SMTPUser"];
            }
        }

        public string SMTPPassword
        {
            get
            {
                return _config.GetSection("App")["SMTPPassword"];
            }
        }

        public bool SMTPEnableSsL
        {
            get
            {
                bool enableSSL = false;
                string enableSSLString = _config.GetSection("App")["SMTPEnableSsL"];
                if (bool.TryParse(enableSSLString, out enableSSL))
                    return enableSSL;
                else
                    return false;
            }
        }

        /// <summary>
        /// If IsNotificationTest = true, system will send email to "AdministratorEmail" only and will not send email to real email receiver.
        /// Work for APEXAContracting.TaskScheduler.Notification and other notifications.
        /// Corporate with EmailHelper.
        /// </summary>
        public bool IsNotificationTest
        {
            get
            {
                bool result = false;

                string isNotificationTest = _config.GetSection("App")["IsNotificationTest"];

                if (!string.IsNullOrEmpty(isNotificationTest))
                {
                    bool.TryParse(isNotificationTest, out result);
                }

                return result;
            }
        }

        /// <summary>
        ///   For forget password, reset password process, email with generated password also will send to this administrator email address.
        ///   setup dynamic tire help desk email here. helpdesk@dynamictire.com
        /// It can support multiple emails with format email1;email2;email3;email4
        /// </summary>
        public string AdministratorEmail
        {
            get
            {
                return _config.GetSection("App")["AdministratorEmail"];
            }
        }        

        #endregion
               
     

        /// <summary>
        ///  local database connection timeout setting.
        ///  It is integer value of seconds.
        ///  Default value = 360 seconds.
        /// </summary>
        public int DatabaseCommandTimeout
        {
            get
            {

                int result = 360;

                int.TryParse(_config.GetSection("App")["DatabaseCommandTimeout"], out result);

                return result;
            }
        }

        /// <summary>
        /// Work for local host test only. Check domain from field "TestDomain" in table dbo.ClientDomain.
        /// Work for accessing domain validation in identity server.
        /// Note: For QA, UAT, Production server, this setting should be false.
        /// Only work for developer debuging codes in localhost.
        /// </summary>
        public bool IsLocalHostTest
        {
            get
            {
                bool result = false;

                bool.TryParse(_config.GetSection("App")["IsLocalHostTest"], out result);

                return result;
            }
        }

        /// <summary>
        ///  Solution for resolving issues in each time identity server republish,
        ///  the previous login user's data protection keys will be gone. and those end user cannot login again.
        ///  TODO. This solution could only working on one IIS node. If your application is running under load balance setup, it will not work.
        /// </summary>
        public string DataProtectionKeysFolderPath
        {
            get
            {
                return this._config.GetSection("App")["DataProtectionKeysFolderPath"];
            }
        }


        #region Google ReCaptcha
        /// <summary>
        ///  Define enable or disable Google reCaptcha Secret. Work for forgot password UI. Note: For Localhost debugging, set value = false.
        /// </summary>
        public bool EnableReCaptcha
        {
            get
            {
                bool result = false;
                string strResult = _config.GetSection("App")["EnableReCaptcha"];
                bool.TryParse(strResult, out result);
                return result;
            }
        }

        /// <summary>
        ///  Google reCaptcha Secret key. Work for forgot password UI.
        /// </summary>
        public string ReCaptchaSecret
        {
            get
            {
                return _config.GetSection("App")["ReCaptchaSecret"];
            }
        }

        /// <summary>
        /// Google reCaptcha SiteKey. Work for forgot password UI.
        /// </summary>
        public string ReCaptchaSiteKey
        {
            get
            {
                return _config.GetSection("App")["ReCaptchaSiteKey"];
            }
        }


        #endregion


        #region Run with Debugger valuese
        /// <summary>
        /// WarrantyEmailTo when run with Debugger
        /// </summary>
        public string WithDebuggerWarrantyEmailTo
        {
            get
            {
                return _config.GetSection("RunWithDebugger")["WarrantyEmailTo"];
            }
        }
        #endregion

        /// <summary>
        ///  Note: This setting only work for APEXAContracting client (angular 7).
        /// </summary>
        public string SPAClientDomain
        {
            get
            {
                return this._config.GetSection("App")["SPAClientDomain"];
            }
        }

        #region Azure Storage Account Settings. Get Information from Azure Portal, Storage Accounts -> Access keys.  Note: Diabled in First On site solution.
        public string AzureStorageAccount {
            get {
                return this._config.GetSection("App")["AzureStorage_Account"];
            }
        }

        public string AzureStorageKey1 {
            get {
                return this._config.GetSection("App")["AzureStorage_Key1"];
            }
        }

        public string AzureStorageConnection1 {
            get {
                return this._config.GetSection("App")["AzureStorage_Connection1"];
            }
        }

        public string AzureStorageKey2
        {
            get
            {
                return this._config.GetSection("App")["AzureStorage_Key2"];
            }
        }

        public string AzureStorageConnection2
        {
            get
            {
                return this._config.GetSection("App")["AzureStorage_Connection2"];
            }
        }

        public string AzureStorageContainerName
        {
            get
            {
                return this._config.GetSection("App")["AzureStorage_ContainerName"];
            }
        }
    
        #endregion


        #region Azure Storage Sync Job Service settings
        /// <summary>
        ///  Predefine what regions need to sync file. Corporate with FOS_Common.dbo.Region table records.
        /// Hardcode setting. Split regionId with ",". 
        /// Work for syn job debuging and testing environment.
        /// When go live, we can set this setting as empty string or include all regionIds which need to sync files to Azure Blob Storage.
        /// </summary>
        public int[] SyncRegionIds { 
            get {
                int[] result = null;

                if (!string.IsNullOrEmpty(this._config.GetSection("App")["SyncRegionIds"]))
                {
                    result = this._config.GetSection("App")["SyncRegionIds"].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                }
                else
                {
                    result = new int[0];
                }

                return result;
            } 
        }

        /// Number Of CTFile records Per Synchronization Limition .
        public int NumberOfCTFilePerSyncLimition
        {
            get
            {
                int result = 0;

                if (!string.IsNullOrEmpty(this._config.GetSection("App")["NumberOfCTFilePerSyncLimition"]))
                {
                    int.TryParse(this._config.GetSection("App")["NumberOfCTFilePerSyncLimition"], out result);
                }               

                return result;
            }
        }
        #endregion


    }
}

