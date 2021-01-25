using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace APEXAContracting.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActionController : BaseController
    {
        public ActionController(ILogger<LookupController> logger): base(logger)
        {
        }

        /// <summary>
        ///  Test web api.
        /// </summary>
        /// <returns></returns>
        [Route("[Action]")]
        [HttpGet]
        public ActionResult HelloWorld()
        {
            return Content("Web API End Point is active.");
        }
    }
}
