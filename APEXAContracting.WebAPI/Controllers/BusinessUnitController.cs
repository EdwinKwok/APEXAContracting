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
    public class BusinessUnitController : BaseController
    {

        private IBusinessUnitService _businessUnitService;
        private ILookupService _lookupService;
        public BusinessUnitController(ILogger<BusinessUnitController> logger, IBusinessUnitService businessUnitService, ILookupService lookupService) : base(logger)
        {
            _businessUnitService = businessUnitService;
            _lookupService = lookupService;
        }

        [Route("[Action]")]
        [HttpGet]
        public IActionResult GetBusinessUnitList()
        {
            BusinessResult<List<BusinessUnitExtDTO>> result = new BusinessResult<List<BusinessUnitExtDTO>>();
            try
            {
                result = _businessUnitService.GetBusinessUnitList();
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

        [Route("[Action]")]
        [HttpGet]
        public IActionResult GetBusinessUnitById(Guid id)
        {
            BusinessResult<BusinessUnitExtDTO> result = new BusinessResult<BusinessUnitExtDTO>();
            try
            {
                result = _businessUnitService.GetBusinessUnitById(id);
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

        [Route("[Action]")]
        [HttpGet]
        public IActionResult GetBusinessUnitByTypeList(int typeId)
        {
            BusinessResult<List<BusinessUnitExtDTO>> result = new BusinessResult<List<BusinessUnitExtDTO>>();
            try
            {
                result = _businessUnitService.GetBusinessUnitByTypeList(typeId);
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
        [HttpPost]
        public IActionResult AddBusinessUnit(BusinessUnitDTO formData)
        {
            //formData.Id must have value before calling this endpoint
            BusinessResult<BusinessUnitDTO> result = new BusinessResult<BusinessUnitDTO>();
            result = _businessUnitService.AddBusinessUnit(formData);
            return Ok(result);
        }

        [HttpPut]
        public IActionResult UpdateBusinessUnit(BusinessUnitDTO formData)
        {
            BusinessResult<BusinessUnitDTO> result = new BusinessResult<BusinessUnitDTO>();
            result = _businessUnitService.UpdateBusinessUnit(formData);
            return Ok(result);
        }

        [HttpDelete]
        public IActionResult DeleteBusinessUnit(Guid id)
        {
            BusinessResult<BusinessUnitDTO> result = new BusinessResult<BusinessUnitDTO>();

            result = _businessUnitService.DeleteBusinessUnit(id);
            return Ok(result);
        }


        //APEXAContracting.DataAccess

    }
}
