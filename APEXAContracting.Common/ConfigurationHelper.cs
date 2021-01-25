using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace APEXAContracting.Common
{
    /// <summary>
    ///  Access appsettings.json in UnitTest project.
    /// </summary>
    public static class ConfigurationHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputPath"></param>
        /// <param name="configSettingFileName">such as value = "appsettings.json".</param>
        /// <returns></returns>
        public static IConfigurationRoot GetIConfigurationRoot(string outputPath, string configSettingFileName)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile(configSettingFileName, optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        ///  Access appsettings.json.
        /// </summary>
        /// <param name="outputPath"></param>
        /// <param name="configSettingFileName">such as value = "appsettings.json".</param>
        /// <returns></returns>
        public static IConfiguration GetApplicationConfiguration(string outputPath, string configSettingFileName)
        {
            var config = GetIConfigurationRoot(outputPath, configSettingFileName);

            return config;
        }
    }
}
