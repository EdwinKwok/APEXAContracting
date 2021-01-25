using Microsoft.Extensions.DependencyInjection;
using APEXAContracting.Business.Interface;
using APEXAContracting.Common.Interfaces;
using APEXAContracting.Business.AppContext;
using APEXAContracting.Business;
//using APEXAContracting.Business.Identity;

namespace APEXAContracting.WebAPI.Helper
{
    /// <summary>
    /// Register Business Layers services.
    /// </summary>
    public static class BusinessLayerRegister
    {
        /// <summary>
        ///  Dependency Injection register Business layer services in Startup.cs.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configSettings"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterBusinessLayer(this IServiceCollection services, IConfigSettings configSettings)
        {
            // Work for system level AppContext.
            services.AddTransient<IAppContextService, AppContextService>();
            services.AddTransient<ILookupService, LookupService>();
            services.AddTransient<IBusinessUnitService, BusinessUnitService>();
            services.AddTransient<IContractService, ContractService>();

            return services;
        }
    }
}
