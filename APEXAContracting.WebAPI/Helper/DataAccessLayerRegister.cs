using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using APEXAContracting.Common.Interfaces;
using APEXAContracting.DataAccess;
using System;

namespace APEXAContracting.WebAPI.Helper
{
    /// <summary>
    ///  Register data access layer.
    /// </summary>
    public static class DataAccessLayerRegister
    {
        /// <summary>
        ///  Dependency Injection register DataAccess layer DbContexts, UnitOfWorks in Web application's Startup.cs.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configSettings"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterDataAccessLayer(this IServiceCollection services, IConfigSettings configSettings)
        {

            // Register DbContext instance. Database connection. Which will be instance to Unit Of Work.
            //
            services.AddDbContext<DatabaseContext>(options =>
               options.UseSqlServer(configSettings.DatabaseConnection,
               opts =>
               {
                   opts.CommandTimeout(configSettings.DatabaseCommandTimeout);
                   opts.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
               } //  resilient EF Core connections https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/implement-resilient-entity-framework-core-sql-connections
               ) // Register SQL Server database.
               .UseLazyLoadingProxies(true) // Enable or disable lazyloading function. Note: Enable lazyloading will convinent Auto Mapper mapping children entities to DTOs.               
            );

                      
            //
            // Register custom services (Business layers).
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1
            //

            services.AddTransient<IUnitOfWork, UnitOfWork>();                   

            return services;
        }
    }
}
