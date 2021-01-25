//using APEXAContracting.Business.Interface;
//using APEXAContracting.Model.DTO.Identity;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using AutoMapper;
//using APEXAContracting.DataAccess;
//using DTO = APEXAContracting.Model.DTO;
//using Entity = APEXAContracting.Model.Entity;
//using Microsoft.EntityFrameworkCore;
//using APEXAContracting.Common;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Hosting;
//using APEXAContracting.Common.Interfaces;
//using APEXAContracting.Common.Helpers;
//using System.IO;
//using Microsoft.Extensions.Logging;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.Graph;
//using Newtonsoft.Json;
//using Microsoft.IdentityModel.Clients.ActiveDirectory;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Net;

//namespace APEXAContracting.Business.Identity
//{
//    /// <summary>
//    ///  Work for Identity server.
//    /// </summary>
//    public class IdentityService : BaseService, IIdentityService
//    {
//        private readonly IMapper _mapper;
//        private readonly IHostingEnvironment _environment;

//        /// <summary>
//        ///  Dependency inject IUnitOfWork which accessing FirstOnSite.DocumentManagementOrders database.
//        /// </summary>
//        /// <param name="uow"></param>
//        public IdentityService(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfigSettings config, IHostingEnvironment environment, ILogger<IdentityService> logger)
//            : base(uow, httpContextAccessor, config, logger)
//        {
//            this._mapper = mapper;
//            this._environment = environment;
//        }

//        /// <summary>
//        ///  Check login user with username and password (normal login) or temp password (forget password or password reset).
//        ///  Work for login with Identity Server. Input user user name and password in Identity Server UI.
//        /// </summary>
//        /// <param name="username">user name. Reference to field "UserName" in table [dbo].[User]</param>
//        /// <param name="password">Reference to field "Password" in table [dbo].[User].</param>
//        /// <returns>
//        /// true, user authorized.
//        /// false, user unauthorized.
//        /// </returns>
//        public DTO.Identity.User ValidateCredentials(string username, string password)
//        {
//            Entity.User user = this.UOW.Users.FirstOrDefault(u => u.Username == username,i=>i.Include(u=>u.UserRole), orderBy: null);
//            bool result = false;
//            DTO.Identity.User resultUser = null;

//            if (user != null)
//            {
//                //
//                // Check user's password with salt.
//                //
//                byte[] saltFromDB = user.Salt;
//                byte[] passwordFromDB = user.Password;
//                byte[] saltpassword = Utilities.HashPassword(password, saltFromDB);
//                result = Utilities.PasswordMatch(saltpassword, passwordFromDB);

//                if (result)
//                {
//                    resultUser = new DTO.Identity.User { UserName = username, Id = user.Id, Roles = user.UserRole.Select(m => (Enumerations.Role)m.RoleId).ToList() };
//                }
//            }

//            return resultUser;
//        }

//        /// <summary>
//        ///  Update login user with new password. 
//        ///  Work for forget password.
//        ///  Finally send out email.
//        ///  Note: The email also will send to administrator email which setup in "AdministratorEmail" in appsetting.json.
//        /// </summary>
//        /// <param name="username">login user name</param>
//        /// <param name="password">new password</param>
//        /// <param name="isTempPassword">
//        ///  Reference to field "IsTempPassword" in dbo.User table. 
//        ///  Work for forget password and reset password.</param>
//        /// <returns>true means password updated.</returns>
//        public bool UpdateCredentials(string username, string password, bool isTempPassword)
//        {
//            Entity.User user = this.UOW.Users.FirstOrDefault(u => u.Username == username);
//            bool result = false;

//            if (user != null)
//            {                
//                user.Salt = Utilities.GenerateSalt();
//                user.Password = Utilities.HashPassword(password, user.Salt);
//                result = this.UOW.Save() > 0;
//            }

//            //
//            // After update password in database, need to send email to client.
//            //
//            //if (result)
//            //{
//            //    //
//            //    // Send forgot password email.
//            //    //
//            //    if (isTempPassword)
//            //    {

//            //        // Send out email with new password.
//            //        var userResponse = this.GetUser(username);

//            //        if (userResponse != null && userResponse.ResultStatus == ResultStatus.Success)
//            //        {
//            //            DTO.Identity.User userProfile = userResponse.Item;
//            //            userProfile.Password = password;

//            //            string templateFolder = Path.Combine(this._environment.ContentRootPath, "EmailTemplates");

//            //            Dictionary<string, string> parameters = new Dictionary<string, string>();
//            //            //parameters.Add("PortalUrl", this.GetClientDomain(clientId));
//            //            //parameters.Add("ClientName", this.GetClientName(clientId));
//            //            //parameters.Add("CompanyName", this.GetClientName(clientId));

//            //            EmailHelper emailHelper = new EmailHelper(this.Config, templateFolder, EmailType.ForgotPasswordEmail, EmailTemplateType.xslt);

//            //            //
//            //            // Send email with password to user's email account and system administrator email account.
//            //            //
//            //            if (!this.Config.IsNotificationTest)
//            //            {
//            //                emailHelper.Send(userProfile.Email + ";" + this.Config.AdministratorEmail, this.Config.FromEmail, userProfile, parameters, null);
//            //            }
//            //            else
//            //            {
//            //                //
//            //                // For test purpose, only send email to AdministratorEmails.
//            //                //
//            //                emailHelper.Send(this.Config.AdministratorEmail, this.Config.FromEmail, userProfile, parameters, null);
//            //            }
//            //        }
//            //    }
//            //}

//            return result;
//        }

//        public bool CreateCredential(DTO.Identity.UserPassword inputUser)
//        {
//            bool result = false;

//            if (inputUser != null && !string.IsNullOrEmpty(inputUser.UserName) &&
//                !string.IsNullOrEmpty(inputUser.Password) && inputUser.Password == inputUser.ConfirmedPassword)
//            {

//                if (!this.UOW.Users.Any(u => u.Username == inputUser.UserName))
//                {

//                    Entity.User user = new Entity.User();
//                    user.Username = inputUser.UserName;
//                    user.FirstName = inputUser.FirstName;
//                    user.LastName = inputUser.LastName;
//                    user.Email = inputUser.Email;
//                    user.Salt = Utilities.GenerateSalt();
//                    user.Password = Utilities.HashPassword(inputUser.Password, user.Salt);
//                    user.IsTempPasswordEnabled = false;
//                    user.IsActive = true;
//                    this.UOW.Users.Add(user);

//                    result = this.UOW.Save() > 0;

//                }
//            }

//            return result;
//        }



//        ///// <summary>
//        /////  Get user profile information from Azure AD based on logon user's object id.
//        ///// </summary>
//        ///// <returns></returns>
//        //public async Task<BusinessResult<DTO.Identity.UserProfile>> GetUserProfile()
//        //{
//        //    BusinessResult<DTO.Identity.UserProfile> result = new BusinessResult<DTO.Identity.UserProfile>();

//        //    string user = await this.SendGraphGetRequest("/users/" + this.CurrentUser.ObjectId, null);

//        //    if (!string.IsNullOrEmpty(user))
//        //    {

//        //        dynamic jsonUser = JsonConvert.DeserializeObject(user);

//        //        if (jsonUser != null)
//        //        {

//        //            DTO.Identity.UserProfile userProfile = new UserProfile();
//        //            userProfile.ObjectId = this.CurrentUser.ObjectId;
//        //            userProfile.UserName = this.CurrentUser.UserName;
//        //            userProfile.FamilyName = jsonUser.surname;
//        //            userProfile.GivenName = jsonUser.givenName;
//        //            userProfile.JobTitle = jsonUser.jobTitle;
//        //            userProfile.EmployeeId = jsonUser.employeeId;
//        //            userProfile.Department = jsonUser.department;
//        //            userProfile.StreetAddress = jsonUser.streetAddress;
//        //            userProfile.City = jsonUser.city;
//        //            userProfile.State = jsonUser.state;
//        //            userProfile.PostalCode = jsonUser.postalCode;
//        //            userProfile.Country = jsonUser.country;
//        //            userProfile.TelephoneNumber = jsonUser.telephoneNumber;

//        //            result.Item = userProfile;
//        //            result.ResultStatus = ResultStatus.Success;
//        //        }
//        //    }

//        //    return result;
//        //}

//        /// <summary>
//        ///  Shared method to access Azure AD user profiles with Azure Graph APIs.
//        ///  
//        ///  Application level access Azure AD to get all user profiles or one specified logon user's profile.
//        ///  
//        ///  Reference: https://github.com/AzureADQuickStarts/B2C-GraphAPI-DotNet/blob/master/B2CGraphClient/B2CGraphClient.cs
//        /// </summary>
//        /// <param name="api">Api url in Azure Graph. Format: "/###/"</param>
//        /// <param name="query"></param>
//        /// <returns></returns>
//        protected async Task<string> SendGraphGetRequest(string api, string query)
//        {
//            AuthenticationContext authContext = new AuthenticationContext(this.Config.AzureADAuthority);

//            ClientCredential credential = new Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential(this.Config.AzureADClientId, this.Config.AzureADClientSecret);

//            // First, use ADAL to acquire a token using the app's identity (the credential)

//            //
//            // The first parameter is the resource we want an access_token for; in this case, the Graph API.
//            //
//            AuthenticationResult result = await authContext.AcquireTokenAsync("https://graph.windows.net", credential);

//            // For B2C user managment, be sure to use the 1.6 Graph API version.
//            HttpClient http = new HttpClient();

//            string url = "https://graph.windows.net/" + this.Config.AzureADTenant + api + "?" + this.Config.AzureADGraphVersion;

//            if (!string.IsNullOrEmpty(query))
//            {
//                url += "&" + query;
//            }

//            // Append the access token for the Graph API to the Authorization header of the request, using the Bearer scheme.

//            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

//            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

//            HttpResponseMessage response = await http.SendAsync(request);

//            if (!response.IsSuccessStatusCode)
//            {
//                string error = await response.Content.ReadAsStringAsync();

//                object formatted = JsonConvert.DeserializeObject(error);

//                throw new WebException("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));
//            }

//            return await response.Content.ReadAsStringAsync();
//        }


//        /// <summary>
//        ///  Get user additional authorization settings from local database.
//        ///  Work for get logon user's custom authorization from local database.
//        /// </summary>
//        /// <param name="userName">Reference to field "UserName" in table dbo.User. It must be unique.</param>
//        /// <returns></returns>
//        public BusinessResult<DTO.Identity.User> GetUser(string userName)
//        {
//            BusinessResult<DTO.Identity.User> result = new BusinessResult<DTO.Identity.User>();
//            var user = this.UOW.Users.FirstOrDefault<DTO.Identity.User>(u => u.Username == userName);

//            if (user != null)
//            {
//                result.Item = user;
//                result.ResultStatus = ResultStatus.Success;
//            }

//            return result;
//        }

//        ///// <summary>
//        /////  Try to get base user info with user name and email together.
//        /////  work for forget password setting.
//        ///// </summary>
//        ///// <param name="userName"></param>
//        ///// <param name="email"></param>
//        ///// <returns></returns>
//        //public DTO.Identity.UserPassword GetUser(string userName, string email)
//        //{
//        //    DTO.Identity.UserPassword result = null;

//        //    result = this.UOW.Users.FirstOrDefault<DTO.Identity.UserPassword>(u => u.UserName == userName && u.Email == email);

//        //    return result;
//        //}


//        ///// <summary>
//        /////  Get specified user allow to access specified company portal or admin portal.
//        /////  If the user password is correct, we need to check if user has permission to login current company portal or admin portal.
//        /////  If User Role is "User" and "CompanyAdmin", need to check CompanyUser table if current company has been assigned to the logon user.
//        /////  If User Role is "User", logon user no permission to login admin portal.
//        /////  If User Role is "SystemAdmin", the logon user has permission to access all company portals and admin portal.
//        /////  Note: Sap portal using the same logic.
//        /////
//        ///// </summary>
//        ///// <param name="userId">Logon user's primary key. Id field in table [dbo].[User].</param>
//        ///// <param name="clientId">Reference to field "Id" in table Company if access company portals. or value = "admin", which will be admin portal.</param>
//        ///// <returns>
//        /////   true, allow access.
//        /////   false, do not allow to access.
//        ///// </returns>
//        //public bool CheckIfUserAllowAccessClientDomain(string clientId, Guid userId)
//        //{
//        //    bool result = false;
//        //    DTO.Identity.User user = this.UOW.Users.FirstOrDefault<DTO.Identity.User>(u => u.Id == userId, 
//        //        u => u.Include(ur => ur.UserRole).ThenInclude(r => r.Role));

//        //    if (user != null)
//        //    {                

//        //        if (clientId != "admin")
//        //        {
//        //            int intClientId = 0;

//        //            int.TryParse(clientId, out intClientId);

//        //            if (user.Roles == Enumerations.Role.SysAdmin.ToString())
//        //            {
//        //                // For System Administrator. allow to access any company portal.
//        //                result = true;
//        //            }
//        //        }
//        //        else {
//        //            //
//        //            // For Admin Portal access.
//        //            //
//        //            if (user.Roles == Enumerations.Role.SysAdmin.ToString())
//        //            {
//        //                // Company Admin and System Admin allow to access admin portal.
//        //                result = true;
//        //            }
//        //        }
//        //    }

//        //    return result;
//        //}

//        ///// <summary>
//        /////  Return company portal url based on client Id (companyId).
//        /////  Work for send forget password email.
//        ///// </summary>
//        ///// <param name="clientId"></param>
//        ///// <returns>domain url of company portal or admin portal. Reference to field "Domain" in table dbo.ClientDomain.</returns>
//        //public string GetClientDomain(string clientId)
//        //{
//        //    string result = string.Empty;
//        //    int intClientId = 0;

//        //    if (!string.IsNullOrEmpty(clientId))
//        //    {
//        //        Entity.ClientDomain clientDomain = null;
//        //        clientDomain = this.UOW.ClientDomains.FirstOrDefault(c => c.ClientId == clientId);

//        //        if (clientDomain != null)
//        //        {
//        //            result = clientDomain.Domain;
//        //        }
//        //    }

//        //    return result;
//        //}

//        ///// <summary>
//        /////  Generate an Azure Graph Service client instance. 
//        /////  Work for Auzre AD user profile access.
//        ///// </summary>
//        ///// <returns></returns>
//        //private async Task<GraphServiceClient> GetAzureGraphClient()
//        //{

//        //    string clientId = this.Config.AzureADClientId;
//        //    string clientSecret = this.Config.AzureADClientSecret;

//        //    // Configure app builder
//        //    var authority = this.Config.AzureADAuthority;

//        //    var app = ConfidentialClientApplicationBuilder
//        //        .Create(clientId)
//        //        .WithClientSecret(clientSecret)
//        //        .WithAuthority(new Uri(authority))
//        //        .Build();
//        //    //
//        //    // Acquire tokens for Graph API
//        //    //
//        //    var scopes = new[] { "https://graph.microsoft.com/.default" };
//        //    //    var authenticationResult = await app.AcquireTokenForClient(scopes).ExecuteAsync();

//        //    string accessToke = await this.GetAzureADAccessToken() ;// authenticationResult.AccessToken;

//        //    // Create GraphClient and attach auth header to all request (acquired on previous step)
//        //    var graphClient = new GraphServiceClient(
//        //        new DelegateAuthenticationProvider(requestMessage =>
//        //        {
//        //            requestMessage.Headers.Authorization =
//        //                new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", accessToke);

//        //            return Task.FromResult(0);
//        //        }));

//        //    return graphClient;
//        //}

//        //private async Task<string> GetAzureADAccessToken() {

//        //    IConfidentialClientApplication app;
//        //    app = ConfidentialClientApplicationBuilder.Create(this.Config.AzureADClientId)
//        //                                              .WithClientSecret(this.Config.AzureADClientSecret)
//        //                                              .WithAuthority(new Uri(this.Config.AzureADAuthority))
//        //                                              .Build();

//        //    var tokenRequestResult = await app.AcquireTokenForClient(new string[] { "https://ontariocreatesdev.onmicrosoft.com/915595e2-76aa-4637-8770-99609241f8db/.default" }).ExecuteAsync();

//        //    var aadAccessToken = tokenRequestResult.AccessToken;

//        //    return aadAccessToken;
//        //}
//    }
//}
