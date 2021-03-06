﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using APEXAContracting.Model.DTO;
using System.Net.Http;
using APEXAContracting.Web.Common.Helper;
using APEXAContracting.Web.Models;
using APEXAContracting.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace APEXAContracting.Web.Pages.Contract
{
    public class DetailsModel : BasePageModel
    {
        private readonly ILogger<DetailsModel> _logger;
        private IConfiguration _configSettings;


        public DetailsModel(IConfiguration config, ILogger<DetailsModel> logger) : base(config, logger)
        {
            this._logger = logger;
            this._configSettings = config;
        }

        public ContractVM ContractVM { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string queryString = string.Format("api/Contract/GetContractById?id={0}", id.ToString());

            BusinessResult<ContractVM> br = await HttpHelper.Get<ContractVM>(ApiRootUrl, queryString);
            if (br.Item == null)
            {
                return NotFound();
            }

            ContractVM = br.Item;
            ViewData["subTitle"] = br.Item.ContractName;
            return Page();
        }
    }
}
