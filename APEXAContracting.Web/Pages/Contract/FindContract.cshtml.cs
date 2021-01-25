using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APEXAContracting.Web.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using APEXAContracting.Common;
using APEXAContracting.Web.Common.Helper;
using APEXAContracting.Model.DTO;


namespace APEXAContracting.Web.Pages.Contract
{
    public class FindContractModel : BasePageModel
    {
        private readonly ILogger<FindContractModel> _logger;
        private IConfiguration _configSettings;

        public FindContractModel(IConfiguration config, ILogger<FindContractModel> logger) : base(config, logger)
        {
            this._logger = logger;
            this._configSettings = config;
        }

        public ContractVM ContractVM { get; set; }

        //public async Task<IActionResult> OnGetAsync()
        //{
        //    string queryString = string.Format("api/Contract/GetContractById?id={0}", "");

        //    BusinessResult<ContractVM> br = await HttpHelper.Get<ContractVM>(ApiRootUrl, queryString);
        //    ContractVM = br.Item;
        //    return Page();

        //}

        public async Task<IActionResult> OnPostAsync()
        {

            var txtOfferedByKey = Request.Form["txtOfferedByKey"];
            var txtAcceptedBykey = Request.Form["txtAcceptedBykey"];
            string queryString = string.Format("api/Contract/GetContractByKeys?OfferedByKey={0}&AcceptedByKey={1}", txtOfferedByKey, txtAcceptedBykey);

            BusinessResult<ContractVM> br = await HttpHelper.Get<ContractVM>(ApiRootUrl, queryString);
            ContractVM = br.Item;


            if (ContractVM.Id == Guid.Empty)
            {
                @ViewData["msg"] = "No Contract Found!";
            }
            else
            {
                @ViewData["msg"] = "Contract Found";
            }
            
            return Page();
        }
    }
}
