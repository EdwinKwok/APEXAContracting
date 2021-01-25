using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DTO = APEXAContracting.Model.DTO;
using System.Linq;
namespace APEXAContracting.WebAPI.Controllers
{

    public class BaseController : ControllerBase
    {
        private readonly ILogger<BaseController> _logger;

        public BaseController(ILogger<BaseController> logger)
        {
            this._logger = logger;
        }

        protected ILogger Logger { get { return _logger; } }

    }
}