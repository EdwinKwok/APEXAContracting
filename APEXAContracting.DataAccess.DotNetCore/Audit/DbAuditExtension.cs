using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APEXAContracting.DataAccess.DotNetCore.Audit
{
    /// <summary>
    ///  Work for database data update auditing.
    ///  Table is dbo.AuditHistory.
    /// </summary>
    public static class DbAuditExtension
    {
        /// <summary>
        /// Ensures the automatic history.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logonUser">Logon user account name. Optional.</param>
        public static void EnsureAudit(this DbContext context, string logonUser)
        {
            Guid auditGroupId = Guid.NewGuid();  // Work as a batch id. Represent the auditings in same save transaction.

            EnsureAudit<AuditHistory>(context, () => new AuditHistory(logonUser, auditGroupId));
        }

        /// <summary>
        ///  Add auditing records to table dbo.AuditHistory.
        ///  Note: Here dbContext not commit yet.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="createAuditHistoryFactory"></param>
        public static void EnsureAudit<T>(this DbContext context, Func<T> createAuditHistoryFactory)
            where T : AuditHistory
        {
            var entries = context.ChangeTracker.Entries().Where(e => (e.State == EntityState.Modified || 
                                                                      e.State == EntityState.Added || 
                                                                      e.State == EntityState.Deleted) 
                                                                      && e.Metadata.GetTableName() != "AuditHistory").ToArray();
            if (entries != null && entries.Count() > 0)
            {
                foreach (var entry in entries)
                {
                    context.AddRange(entry.AuditHistory<AuditHistory>(createAuditHistoryFactory).ToArray());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        /// <param name="createAuditHistoryFactory">Generic function definition. Create new entity instance with AuditHistory class.</param>
        /// <returns></returns>
        internal static IList<T> AuditHistory<T>(this EntityEntry entry, Func<T> createAuditHistoryFactory)
            where T : AuditHistory 
        {
            IList<T> audits = new List<T>();

            string tableName = entry.Metadata.GetTableName();
            string primaryKeyName = PrimaryKeyName(entry);
            string primaryKeyValue = PrimaryKeyValue(entry);

            // Get the mapped properties for the entity type.
            // (include shadow properties, not include navigations & references)
            var properties = entry.Properties;

            switch (entry.State)
            {
                // When add new record.
                case EntityState.Added:
                    foreach (var prop in properties)
                    {
                        T audit = createAuditHistoryFactory();

                        //if (prop.Metadata.IsKey() || prop.Metadata.IsForeignKey())
                        //{
                        //    continue;
                        //}

                        audit.CurrentValue = prop.CurrentValue != null ? prop.CurrentValue.ToString() : null;
                        audit.TableName = tableName;
                        audit.FieldName = prop.Metadata.Name;
                        audit.PrimaryKeyField = primaryKeyName;
                        audit.PrimaryKeyValue = primaryKeyValue;
                        audit.EntityState = EntityState.Added;
                        audits.Add(audit);       
                    }

                    break;

                // When update existing record.
                case EntityState.Modified:
               
                    foreach (var prop in properties)
                    {
                        if (prop.IsModified)
                        {
                            T audit = createAuditHistoryFactory();
                            
                            audit.CurrentValue = prop.CurrentValue != null ? prop.CurrentValue.ToString() : null;
                            audit.OriginalValue = prop.OriginalValue != null ? prop.OriginalValue.ToString() : null;
                            audit.TableName = tableName;
                            audit.FieldName = prop.Metadata.Name;
                            audit.PrimaryKeyField = primaryKeyName;
                            audit.PrimaryKeyValue = primaryKeyValue;
                            audit.EntityState = EntityState.Modified;
                            audits.Add(audit);
                        }
                    }                   
                    break;

                // When delete existing record.
                case EntityState.Deleted:
                    foreach (var prop in properties)
                    {
                        T audit = createAuditHistoryFactory();

                        audit.OriginalValue = prop.OriginalValue != null ? prop.CurrentValue.ToString() : null;
                        audit.TableName = tableName;
                        audit.FieldName = prop.Metadata.Name;
                        audit.PrimaryKeyField = primaryKeyName;
                        audit.PrimaryKeyValue = primaryKeyValue;
                        audit.EntityState = EntityState.Deleted;
                        audits.Add(audit);
                    }
                  
                    break;
                case EntityState.Detached:
                    //Do nothing.
                    break;
                case EntityState.Unchanged:
                    // Do nothing.
                    break;
                default:
                    // Do nothing.
                    break;
                    
            }

            return audits;
        }

        /// <summary>
        ///  For most cases, it will be only one primary key field. But it supports composed keys also.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private static string PrimaryKeyValue(this EntityEntry entry)
        {
            var key = entry.Metadata.FindPrimaryKey();

            var values = new List<object>();
            foreach (var property in key.Properties)
            {
                var value = entry.Property(property.Name).CurrentValue;
                if (value != null)
                {
                    values.Add(value);
                }
            }

            return string.Join(",", values);
        }

        /// <summary>
        ///  For most cases, it will be one one primary key field. But it supports composed keys.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private static string PrimaryKeyName(this EntityEntry entry)
        {
            var key = entry.Metadata.FindPrimaryKey();

            var values = new List<string>();
            foreach (var property in key.Properties)
            {
                var value = property.Name;
                if (value != null)
                {
                    values.Add(value);
                }
            }

            return string.Join(",", values);
        }

    }
}
