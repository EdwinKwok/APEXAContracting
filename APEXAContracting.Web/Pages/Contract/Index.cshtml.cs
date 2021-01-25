using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using APEXAContracting.Model.DTO;
using APEXAContracting.Web.Common.Helper;
using APEXAContracting.Common;
using System.Net.Http;

using Newtonsoft.Json.Linq;
using APEXAContracting.Web.Models;
using Newtonsoft.Json;
using System.Configuration;
using APEXAContracting.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;


namespace APEXAContracting.Web.Pages.Contract
{
    public class IndexModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IConfiguration _configSettings;

        public IndexModel(IConfiguration config, ILogger<IndexModel> logger) : base(config, logger)
        {
            this._logger = logger;
            this._configSettings = config;
        }

        public IList<ContractVM> ContractVM { get; set; }

        public async Task OnGetAsync()
        {
            
            string queryString = "api/Contract/GetContractList";

            BusinessResult<List<ContractVM>> br = await HttpHelper.Get<List<ContractVM>>(ApiRootUrl, queryString);
            ContractVM = br.Item;
        }
    }
}
