using Microsoft.VisualStudio.TestTools.UnitTesting;
using APEXAContracting.Common;
using APEXAContracting.DataAccess;
using System.IO;
using APEXAContracting.Model.Mapper;
using APEXAContracting.Model.Entity;
using APEXAContracting.Business.Interface;
using DTO = APEXAContracting.Model.DTO;
using Microsoft.AspNetCore.Http;
using APEXAContracting.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

namespace APEXAContracting.Business.Test
{
    /// <summary>
    /// Base unit test class.
    /// </summary>
    /// <typeparam name="T">T is type of Service you are going to test. Note: we only support test on service in one unit test this time.</typeparam>
    public class BaseTest<T> where T: class
    {
        protected IConfiguration config;
        protected IConfigSettings configSettings;
        protected string dbConnection = string.Empty;
        protected IUnitOfWork uow = null;
        protected DatabaseContext dbContext = null;
        protected IMapper mapper = null;
        protected IHttpContextAccessor httpContextAccessor = null;
        protected Mock<IHostingEnvironment> environment;
        protected Mock<ILogger<T>> logger ;

        /// <summary>
        ///  Constructor. Register all required dependency injections.
        /// </summary>
        public BaseTest()
        {
            //
            // Try to get database connection string setting from appsettings.json.
            //
            string direct = Directory.GetCurrentDirectory();
            this.config = ConfigurationHelper.GetApplicationConfiguration(direct, "appsettings.json");
            this.configSettings = new ConfigSettings(this.config);

            this.environment = new Mock<IHostingEnvironment>();

            this.dbConnection = this.configSettings.DatabaseConnection; 
            var optionBuilder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<DatabaseContext>();

            // Try to connect sql server database.
            optionBuilder.UseSqlServer(this.dbConnection).UseLazyLoadingProxies(false);

            this.httpContextAccessor = new HttpContextAccessor();

            this.logger = new Mock<ILogger<T>>();
            var uowLogger = new Mock<ILogger<UnitOfWork>>();
            var dbLogger = new Mock<ILogger<DatabaseContext>>();
            //
            // Walk around dependency injection.
            //
            this.dbContext = new DatabaseContext(optionBuilder.Options, config);
            this.uow = new UnitOfWork(this.dbContext, this.mapper, uowLogger.Object);
        }
    }
}
