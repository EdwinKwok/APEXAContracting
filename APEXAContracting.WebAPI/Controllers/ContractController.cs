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

    public class ContractController : BaseController
    {
        private IContractService _contractService;
        private ILookupService _lookupService;
        public ContractController(ILogger<ContractController> logger, IContractService contractService, ILookupService lookupService) : base(logger)
        {
            _contractService = contractService;
            _lookupService = lookupService;
        }


        [Route("[Action]")]
        [HttpGet]
        public IActionResult GetContractList()
        {
            BusinessResult<List<ContractDTO>> result = new BusinessResult<List<ContractDTO>>();
            try
            {
                result = _contractService.GetContractList();
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
        public IActionResult GetContractById(Guid id)
        {
            BusinessResult<ContractDTO> result = new BusinessResult<ContractDTO>();
            try
            {
                result = _contractService.GetContractById(id);
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
        public IActionResult GetContractByKeys(string OfferedByKey, string AcceptedByKey)
        {
            BusinessResult<ContractDTO> result = new BusinessResult<ContractDTO>();
            try
            {
                result = _contractService.GetContractByKeys(OfferedByKey, AcceptedByKey);
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
        public IActionResult AddContract(ContractDTO formData)
        {
            //formData.Id must have value before calling this endpoint
            BusinessResult<ContractDTO> result = new BusinessResult<ContractDTO>();
            result = _contractService.AddContract(formData);
            return Ok(result);
        }

        [HttpPut]
        public IActionResult UpdateContract(ContractDTO formData)
        {
            BusinessResult<ContractDTO> result = new BusinessResult<ContractDTO>();
            result = _contractService.UpdateContract(formData);
            return Ok(result);
        }

        [HttpDelete]
        public IActionResult DeleteContract(Guid id)
        {
            BusinessResult<ContractDTO> result = new BusinessResult<ContractDTO>();

            result = _contractService.DeleteContract(id);
            return Ok(result);
        }


    }
}
