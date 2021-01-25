using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APEXAContracting.Model.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using APEXAContracting.Common;
using APEXAContracting.Web.Common.Helper;
using APEXAContracting.Web.Models;

namespace APEXAContracting.Web.Pages.Carrier
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
        public BusinessUnitVM BusinessUnitVM { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string queryString = string.Format("api/Contract/GetContractById?id={0}", id.ToString());

            BusinessResult<BusinessUnitVM> br = await HttpHelper.Get<BusinessUnitVM>(ApiRootUrl, queryString);
            if (br.Item == null)
            {
                return NotFound();
            }

            BusinessUnitVM = br.Item;
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
            BusinessUnitDTO dto = new BusinessUnitDTO();
            dto.Id = BusinessUnitVM.Id;
            dto.Name = BusinessUnitVM.Name;
            dto.Name2 = BusinessUnitVM.Name2;
            dto.Address = BusinessUnitVM.Address;
            dto.Phone = BusinessUnitVM.Phone;
            dto.BusinessTypeId = BusinessUnitVM.BusinessTypeId;
            dto.HierarchyKey = BusinessUnitVM.HierarchyKey;
            dto.HierarchyPrefix = BusinessUnitVM.HierarchyPrefix;
            dto.HealthStatusId = BusinessUnitVM.HealthStatusId;
            dto.IsDeleted = BusinessUnitVM.IsDeleted;

            BusinessResult<BusinessUnitDTO> br = await HttpHelper.Put<BusinessUnitDTO, BusinessUnitDTO>(ApiRootUrl, queryString, dto);

            return RedirectToPage("./Index");
        }


    }
}
