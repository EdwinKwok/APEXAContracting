//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using CacheManager.Core;
//using EFSecondLevelCache.Core;

//namespace APEXAContracting.DataAccess.DotNetCore.Repository
//{
//    /// <summary>
//    ///  Shared methods for register Entity Framework Second Level Cache.
//    /// </summary>
//    public static class BaseUnitOfWorkHelper
//    {
//        /// <summary>
//        /// Add an in-memory cache service provider.
//        /// Note: Either call RegisterEFSecondLevelCacheInMemory or RegisterEFSecondLevelCacheInRids. 
//        /// Only need to call one of them.
//        /// </summary>
//        /// <param name="services"></param>
//        /// <param name="expirationMode">
//        /// Suggest to apply Absolute mode. 
//        /// Cache need to be expired in fix time span to get data auto refresh.
//        /// Default value = Absolute.
//        /// </param>
//        /// <param name="expirationFromMinutes">
//        ///  Integer value of minutes for cache expiration. Default value = 10 minutes.
//        /// </param>
//        /// <returns></returns>
//        public static IServiceCollection RegisterEFSecondLevelCacheInMemory(this IServiceCollection services, 
//            ExpirationMode expirationMode = ExpirationMode.Absolute, int expirationFromMinutes = 10)
//        {
//            services.AddEFSecondLevelCache();

//            // Add an in-memory cache service provider
//            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));
//            services.AddSingleton(typeof(ICacheManagerConfiguration),
//                new CacheManager.Core.ConfigurationBuilder()
//                        .WithJsonSerializer()
//                        .WithMicrosoftMemoryCacheHandle()
//                        .WithExpiration(expirationMode, TimeSpan.FromMinutes(expirationFromMinutes))
//                        .Build());

//            return services;
//        }

//        /// <summary>
//        /// Add Redis cache service provider.
//        /// Note: Either call RegisterEFSecondLevelCacheInMemory or RegisterEFSecondLevelCacheInRids. 
//        /// Only need to call one of them.
//        /// </summary>
//        /// <param name="services"></param>
//        /// <param name="expirationMode">
//        /// Suggest to apply Absolute mode. 
//        /// Cache need to be expired in fix time span to get data auto refresh.
//        /// Default value = Absolute.
//        /// </param>
//        /// <param name="expirationFromMinutes">
//        ///  Integer value of minutes for cache expiration. Default value = 10 minutes.
//        /// </param>
//        /// <returns></returns>
//        public static IServiceCollection RegisterEFSecondLevelCacheInRids(this IServiceCollection services,
//            string redisHost, int redisPort,
//            ExpirationMode expirationMode = ExpirationMode.Absolute, int expirationFromMinutes = 10)
//        {
//            // Add Redis cache service provider
//            var jss = new JsonSerializerSettings
//            {
//                NullValueHandling = NullValueHandling.Ignore,
//                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//            };

//            const string redisConfigurationKey = "redis";
//            services.AddSingleton(typeof(ICacheManagerConfiguration),
//                new CacheManager.Core.ConfigurationBuilder()
//                    .WithJsonSerializer(serializationSettings: jss, deserializationSettings: jss)
//                    .WithUpdateMode(CacheUpdateMode.Up)
//                    .WithRedisConfiguration(redisConfigurationKey, config =>
//                    {
//                        config.WithAllowAdmin()
//                            .WithDatabase(0)
//                            .WithEndpoint(redisHost, redisPort);
//                    })
//                    .WithMaxRetries(100)
//                    .WithRetryTimeout(50)
//                    .WithRedisCacheHandle(redisConfigurationKey)
//                    .WithExpiration(expirationMode, TimeSpan.FromMinutes(expirationFromMinutes))
//                    .Build());
//            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));

//            return services;
//        }
//    }
//}
