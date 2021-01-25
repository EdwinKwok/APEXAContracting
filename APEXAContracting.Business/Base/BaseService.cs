using System;
using System.Collections.Generic;
using APEXAContracting.DataAccess;
using DTO = APEXAContracting.Model.DTO;
using Microsoft.AspNetCore.Http;
using APEXAContracting.Common;
using System.Linq;
using IdentityModel;
using Microsoft.Extensions.Logging;
using APEXAContracting.Common.Interfaces;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;

namespace APEXAContracting.Business
{
    /// <summary>
    ///  Business layer base class. shared methods and properties for child business service stay here. 
    /// </summary>
    public class BaseService : IDisposable
    {
        private IUnitOfWork _unitOfWork;

        protected IHttpContextAccessor _httpContextAccessor = null;
        private readonly ILogger<BaseService> _logger = null;
        private readonly IConfigSettings _config = null;

        /// <summary>
        ///  Dependency inject unit of work. Work for web api environment.
        /// </summary>
        /// <param name="uow">UnitOfWork dependency injection for database.</param>
        /// <param name="httpContextAccessor">HttpContext dependency injection.</param>
        /// <param name="logger">File logger dependency injection.</param>
        public BaseService(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor, IConfigSettings config, ILogger<BaseService> logger)
        {
            this._unitOfWork = uow;

            this._httpContextAccessor = httpContextAccessor;
            this._logger = logger;
            this._config = config;
        }

        /// <summary>
        ///  Dependency inject unit of work. Work for none web environment such as window service, window console apps.
        /// </summary>
        /// <param name="uow">UnitOfWork dependency injection for database.</param>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        public BaseService(IUnitOfWork uow, IConfigSettings config, ILogger<BaseService> logger)
        {
            this._unitOfWork = uow;

            this._logger = logger;
            this._config = config;
        }

        public void Dispose()
        {
            if (this._unitOfWork != null)
            {
                this._unitOfWork.Dispose();
            }
        }

        /// <summary>
        ///  File logger. Corporate with appsettings.json, setcion "Logging:File".
        /// </summary>
        protected ILogger Logger { get { return this._logger; } }


        /// <summary>
        /// It is Unit Of Work for database "FOS_Common". 
        /// </summary>
        protected IUnitOfWork UOW { get { return this._unitOfWork; } }


        protected IConfigSettings Config { get { return this._config; } }

        /// <summary>
        /// TODO.
        ///  Get current processing web site's shared information. Such as companyId, customerId, ShipToId etc.
        /// </summary>
        protected DTO.Application.AppContext CurrentAppContext
        {
            get
            {
                DTO.Application.AppContext result = null;

                if (_httpContextAccessor != null && _httpContextAccessor.HttpContext.Items.TryGetValue(Constants.CURRENT_APP_CONTEXT, out var tmpResult))
                {
                    if (tmpResult != null)
                    {
                        result = (DTO.Application.AppContext)tmpResult;
                    }
                }

                return result;
            }
        }

        
        /// <summary>
        ///  Current UTC datetime. 
        /// </summary>
        protected DateTime CurrentDateTime
        {
            get { return DateTime.UtcNow; }
        }

        //protected DateTime CurrentLocalDateTime
        //{
        //    get { return DateTime.UtcNow.AddHours(CurrentAppContext.TimeZoneOffset); }
        //}

       

    }
}
