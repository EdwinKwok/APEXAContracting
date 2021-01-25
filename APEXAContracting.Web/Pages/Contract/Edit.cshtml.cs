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
    public class EditModel : BasePageModel
    {
        private readonly ILogger<EditModel> _logger;
        private IConfiguration _configSettings;

        public EditModel(IConfiguration config, ILogger<EditModel> logger) : base(config, logger)
        {
            this._logger = logger;
            this._configSettings = config;
        }


        [BindProperty]
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
            return Page();
        }

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
            dto.Id = ContractVM.Id;
            dto.OfferedById = ContractVM.OfferedById;
            dto.AcceptedById = ContractVM.AcceptedById;
            dto.OfferedByKey = ContractVM.OfferedByKey;
            dto.AcceptedBykey = ContractVM.AcceptedBykey;
            dto.ContractName = ContractVM.ContractName;
            dto.EffectedOn = ContractVM.EffectedOn;
            dto.ExpiredOn = ContractVM.ExpiredOn;
            dto.IsDeleted = ContractVM.IsDeleted;
            dto.IsExpired = ContractVM.IsExpired;

            BusinessResult<ContractDTO> br = await HttpHelper.Put<ContractDTO, ContractDTO>(ApiRootUrl, queryString, dto);


            return RedirectToPage("./Index");
        }
         
    }
}
