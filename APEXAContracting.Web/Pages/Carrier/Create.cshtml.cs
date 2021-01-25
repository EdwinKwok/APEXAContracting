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

namespace APEXAContracting.Web.Pages.Carrier
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
        public BusinessUnitVM BusinessUnitVM { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string queryString = string.Format("api/BusinessUnit");
            BusinessUnitDTO dto = new BusinessUnitDTO();
            dto.Id = BusinessUnitVM.Id;
            dto.Name = BusinessUnitVM.Name;
            dto.Name2 = BusinessUnitVM.Name2;
            dto.Address = BusinessUnitVM.Address;
            dto.Phone = BusinessUnitVM.Phone;
            dto.BusinessTypeId = 1;
            dto.HierarchyPrefix = BusinessUnitVM.Name.Substring(0,3);
            dto.HierarchyKey = dto.HierarchyPrefix + "<99999>";
            dto.HealthStatusId = 1;
            dto.IsDeleted = false;

            BusinessResult<BusinessUnitDTO> br = await HttpHelper.Post<BusinessUnitDTO, BusinessUnitDTO>(ApiRootUrl, queryString, dto);

            return RedirectToPage("./Index");
        }
    }
}
