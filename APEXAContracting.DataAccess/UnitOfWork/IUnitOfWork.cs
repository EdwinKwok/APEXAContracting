using APEXAContracting.DataAccess.DotNetCore.Audit;
using APEXAContracting.DataAccess.DotNetCore.Interface;
using APEXAContracting.Model.Entity;
using System.Collections.Generic;
using System.Data;
namespace APEXAContracting.DataAccess
{
    /// <summary>
    ///  Connect MySql databases  
    /// </summary>
    public interface IUnitOfWork : IBaseUnitOfWork
    {
        IBaseRepository<BusinessType> BusinessTypes { get; }
        IBaseRepository<BusinessUnit> BusinessUnits { get; }
        IBaseRepository<Contract> Contracts { get; }
        IBaseRepository<HealthStatus> HealthStatuss { get; }
        IBaseRepository<Language> Languages { get; }
        IBaseRepository<UvHealthStatusWeight> uv_HealthStatusWeights { get; }
        IBaseRepository<UvContractLength> uv_ContractLengths { get; }
    }
}
