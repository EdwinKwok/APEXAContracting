using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace APEXAContracting.Common.Logging
{
    public static class FileLoggerFactoryExtensions
    {
        /// <summary>
        ///  Register FileLoggerProvider as ILoggerProvider singleton instance.
        ///  Work for auto logging system running information and exceptions. 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, FileLoggerProvider>();
            return builder;
        }
               
        /// <summary>
        ///  Register FileLoggerProvider as ILoggerProvider singleton instance with custom config.
        ///  Work for auto logging system running information and exceptions. 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, Action<FileLoggerOptions> configure)
        {
            builder.AddFile();
            builder.Services.Configure(configure);
            
            return builder;
        }
        
    }
}
