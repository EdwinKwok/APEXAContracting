using APEXAContracting.Common.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace APEXAContracting.Web.Pages
{

    public class BasePageModel : PageModel
    {
        private readonly ILogger<BasePageModel> _logger;
        private IConfiguration _configSettings;

        public BasePageModel(IConfiguration config, ILogger<BasePageModel> logger)
        {
            this._configSettings = config;
        }

        protected IConfiguration ConfigSettings { get { return _configSettings; } }

        protected ILogger Logger { get { return _logger; } }

        public string ApiRootUrl
        {
            get
            {
                return this._configSettings.GetSection("App")["ApiRootUrl"];
            }
        }
    }

}
