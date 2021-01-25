using AutoMapper;
using APEXAContracting.DataAccess.DotNetCore.Audit;
using APEXAContracting.DataAccess.DotNetCore.Interface;
using APEXAContracting.DataAccess.DotNetCore.Repository;
using Microsoft.Extensions.Logging;
using APEXAContracting.Model.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using APEXAContracting.Common;

namespace APEXAContracting.DataAccess
{
    /// <summary>
    /// Connect to MySql database.
    /// </summary>
    public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {

        private ILogger<UnitOfWork> _logger;

        /// <summary>
        ///  Dependency Injection dbContext from outside.
        ///  
        ///  Note: One UnitOfWork is only able to work for one database (dbContext).
        ///  
        ///  If the solution needs to support multiple databases, developer needs to define multiple UnitOfWork entities.
        /// </summary>
        /// <param name="dbContext">Represent one region database.</param>
        /// <param name="mapper">Dependency inject auto mapp.</param>
        /// <param name="logger">Dependency inject file logger.</param>
        public UnitOfWork(DatabaseContext dbContext, IMapper mapper, ILogger<UnitOfWork> logger) : base(dbContext, mapper)
        {
            _logger = logger;
        }


        public IBaseRepository<BusinessType> BusinessTypes { get { return this.GetRepository<BusinessType>(); } }
        public IBaseRepository<BusinessUnit> BusinessUnits { get { return this.GetRepository<BusinessUnit>(); } }
        public IBaseRepository<Contract> Contracts { get { return this.GetRepository<Contract>(); } }
        public IBaseRepository<HealthStatus> HealthStatuss { get { return this.GetRepository<HealthStatus>(); } }
        public IBaseRepository<Language> Languages { get { return this.GetRepository<Language>(); } }
        public IBaseRepository<UvHealthStatusWeight> uv_HealthStatusWeights { get { return this.GetRepository<UvHealthStatusWeight>(); } }
        public IBaseRepository<UvContractLength> uv_ContractLengths { get { return this.GetRepository<UvContractLength>(); } }

    }
}
