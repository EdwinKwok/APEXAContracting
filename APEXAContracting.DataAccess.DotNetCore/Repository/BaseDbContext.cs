using APEXAContracting.DataAccess.DotNetCore.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APEXAContracting.DataAccess.DotNetCore.Repository
{
    /// <summary>
    /// Base DbContext.
    /// </summary>
    public abstract class BaseDbContext : DbContext
    {
        private IConfiguration _config;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">
        /// Dependency inject a config object into class. Using to access appsettings.json.
        /// Assign object to Configuration property.
        /// It is optional.
        /// </param>        
        /// <param name="options"></param>
        public BaseDbContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            this._config = config;
        }

        /// <summary>
        ///  Inject a Config object into Custom DbContext. Developer can call this property to access custom settings.
        ///  Corporate with EnableSecondLevelCache override process.
        /// </summary>
        public IConfiguration Configuration { get { return this._config; } }
        
       
    }
}
