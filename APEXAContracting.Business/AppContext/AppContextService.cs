using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using APEXAContracting.Business.Interface;
using APEXAContracting.Common;
using APEXAContracting.Common.Interfaces;
using APEXAContracting.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using DTO = APEXAContracting.Model.DTO;

namespace APEXAContracting.Business.AppContext
{
    /// <summary>
    ///  Handle global and shared parameters on runtime.
    /// </summary>
    public class AppContextService: BaseService, IAppContextService
    {
        public AppContextService(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor, IConfigSettings config, ILogger<AppContextService> logger) : base(uow, httpContextAccessor, config, logger)
        {
        }

        /// <summary>
        ///  Initialize the AppContext settings.
        /// </summary>
        /// <returns></returns>
        public BusinessResult<DTO.Application.AppContext> InitAppContext()
        {
            BusinessResult<DTO.Application.AppContext> result = new BusinessResult<DTO.Application.AppContext>(ResultStatus.Success);
            DTO.Application.AppContext appContext = new DTO.Application.AppContext();

            appContext.UserName = "No User Name";
            
            result.Item = appContext;
            return result;
        }
    }
}
