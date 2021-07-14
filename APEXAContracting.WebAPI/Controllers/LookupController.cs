/*
using System;
using System.Collections.Generic;
using APEXAContracting.Business.Interface;
using APEXAContracting.Common;
using APEXAContracting.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace APEXAContracting.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LookupController : BaseController
    {
        private ILookupService _lookupService;
        public LookupController(ILookupService registrationService, ILogger<LookupController> logger): base(logger)
        {
            _lookupService = registrationService;
        }

        [Route("[Action]")]
        [HttpGet]
        public ActionResult GetLanguages()
        {
            BusinessResult<List<LookupNum>> result = new BusinessResult<List<LookupNum>>();
            try
            {
                result.Item = _lookupService.GetLanguageLookup(false);
                result.ResultStatus = ResultStatus.Success;

            }
            catch (Exception ex)
            {
                result.ResultStatus = ResultStatus.Failure;
                result.Message = "Get Languages Failure.";
                this.Logger.LogError("GetLanguagesLookup exception: {0}", ex.ToString());
            }
            //
            //Note: Controller web api methods always return Ok even there is any exceptions. Return exceptions messages from BusinessResult.Message property.
            //
            return Ok(result);
        }

        [Route("[Action]")]
        [HttpGet]
        public ActionResult GetHealthStatus()
        {
            BusinessResult<List<LookupNum>> result = new BusinessResult<List<LookupNum>>();
            try
            {
                result.Item = _lookupService.GetHealthStatusLookup(false);
                result.ResultStatus = ResultStatus.Success;

            }
            catch (Exception ex)
            {
                result.ResultStatus = ResultStatus.Failure;
                result.Message = System.Reflection.MethodBase.GetCurrentMethod().Name + " Failure.";
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
            }
            //
            //Note: Controller web api methods always return Ok even there is any exceptions. Return exceptions messages from BusinessResult.Message property.
            //
            return Ok(result);
        }

    }
}

*/