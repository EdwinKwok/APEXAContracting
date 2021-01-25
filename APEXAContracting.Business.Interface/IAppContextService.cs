using APEXAContracting.Common;
using System;
using System.Collections.Generic;
using System.Text;
using DTO = APEXAContracting.Model.DTO;

namespace APEXAContracting.Business.Interface
{
    public interface IAppContextService
    {

        /// <summary>
        ///  Initialize the AppContext settings.
        /// </summary>
        /// <returns></returns>
        BusinessResult<DTO.Application.AppContext> InitAppContext();
    }
}