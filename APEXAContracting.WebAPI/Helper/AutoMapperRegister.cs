using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using APEXAContracting.Model.Mapper;

namespace APEXAContracting.WebAPI.Helper
{
    public static class AutoMapperRegister
    {
        /// <summary>
        ///  Register AutoMapper with sepcified mapper profiles.
        ///  http://docs.automapper.org/en/stable/Configuration.html#profile-instances
        ///  http://docs.automapper.org/en/stable/Dependency-injection.html
        ///  
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(LookupProfile));
            services.AddAutoMapper(typeof(BusinessUnitProfile));
            services.AddAutoMapper(typeof(ContractProfile));
            services.AddAutoMapper(typeof(BusinessTypeProfile));
            services.AddAutoMapper(typeof(HealthStatusProfile));
            return services;
        }
    }
}
