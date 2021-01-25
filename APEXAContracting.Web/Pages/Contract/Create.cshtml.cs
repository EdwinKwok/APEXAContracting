using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using APEXAContracting.Model.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using APEXAContracting.Web.Models;
using APEXAContracting.Common;
using APEXAContracting.Web.Common.Helper;

namespace APEXAContracting.Web.Pages.Contract
{
    public class CreateModel : BasePageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private IConfiguration _configSettings;

        public CreateModel(IConfiguration config, ILogger<CreateModel> logger) : base(config, logger)
        {
            this._logger = logger;
            this._configSettings = config;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ContractVM ContractVM { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string queryString = string.Format("api/Contract");
            ContractDTO dto = new ContractDTO();
            dto.Id = Guid.NewGuid();
            dto.OfferedById = ContractVM.OfferedById;
            dto.AcceptedById = ContractVM.AcceptedById;
            dto.OfferedByKey = ContractVM.OfferedByKey;
            dto.AcceptedBykey = ContractVM.AcceptedBykey;
            dto.ContractName = ContractVM.ContractName;
            dto.EffectedOn = ContractVM.EffectedOn;
            dto.ExpiredOn = ContractVM.ExpiredOn;
            dto.IsDeleted = ContractVM.IsDeleted;
            dto.IsExpired = ContractVM.IsExpired;

            BusinessResult<ContractDTO> br = await HttpHelper.Post<ContractDTO, ContractDTO>(ApiRootUrl, queryString, dto);

            return RedirectToPage("./Index");
        }
    }
}
