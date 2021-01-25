using System;
using Microsoft.EntityFrameworkCore;

namespace APEXAContracting.DataAccess.DotNetCore.Audit
{
    /// <summary>
    ///  Code first to create Auditing associated table called "dbo.AuditHistory" in database.
    /// </summary>
    public static class DbAuditModelBuilderExtension
    {
        /// <summary>
        /// Enables the automatic recording change history.
        /// Here, system will create new table dbo.AuditHistory to database.
        /// Work for EntityFramework Core Code First.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> to enable auto history feature.</param>
        /// <returns>The <see cref="ModelBuilder"/> had enabled auto history feature.</returns>
        public static ModelBuilder EnableAuditHistory(this ModelBuilder modelBuilder)
        {
            return EnableAuditHistory<AuditHistory>(modelBuilder);
        }

        /// <summary>
        ///  AuditHistory entity model builder. Disabled.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder EnableAuditHistory<T>(this ModelBuilder modelBuilder)
            where T : class
        {            
            modelBuilder.Entity<T>(b =>
            {
              // Do nothing here.    
            });

            return modelBuilder;
        }

    }
}
