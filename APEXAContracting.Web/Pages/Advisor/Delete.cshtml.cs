using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using APEXAContracting.Model.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using APEXAContracting.Web.Models;
using APEXAContracting.Common;
using APEXAContracting.Web.Common.Helper;

namespace APEXAContracting.Web.Pages.Advisor
{
    public class DeleteModel : BasePageModel
    {
        private readonly ILogger<DeleteModel> _logger;
        private IConfiguration _configSettings;

        public DeleteModel(IConfiguration config, ILogger<DeleteModel> logger) : base(config, logger)
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

            string queryString = string.Format("api/BusinessUnit/GetBusinessUnitById?id={0}", id.ToString());

            BusinessResult<BusinessUnitVM> br = await HttpHelper.Get<BusinessUnitVM>(ApiRootUrl, queryString);
            if (br.Item == null)
            {
                return NotFound();
            }

            BusinessUnitVM = br.Item;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string queryString = string.Format("api/BusinessUnit?id={0}", id.ToString());
            BusinessResult<BusinessUnitDTO> br = await HttpHelper.Delete<BusinessUnitDTO>(ApiRootUrl, queryString);

            if (br != null)
            {
                //br.ResultStatus;
            }

            return RedirectToPage("./Index");
        }
    }
}
