using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace APEXAContracting.DataAccess.DotNetCore.Audit
{
    /// <summary>
    ///  It is table called "dbo.AuditHistory" in database.
    /// </summary>
    public class AuditHistory
    {
        /// <summary>
        ///  Constructor.
        /// </summary>
        public AuditHistory() {
            this.CreatedOn = DateTime.UtcNow;
            this.AuditGroupId = Guid.NewGuid();
        }

        /// <summary>
        ///  Need to pass current logon user's account name, and audit group id from caller.
        /// </summary>
        /// <param name="longUser"></param>
        /// <param name="auditGroupId"></param>
        public AuditHistory(string longUser, Guid auditGroupId) {
            this.CreatedBy = longUser;
            this.CreatedOn = DateTime.UtcNow;
            this.AuditGroupId = auditGroupId;
        }

        /// <summary>
        ///  Primary key.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        ///  Flag the fields batch in one save transaction.
        /// </summary>
        [Required]
        public Guid AuditGroupId { get; set; }

        /// <summary>
        ///  Microsoft.EntityFrameworkCore.EntityState.
        ///      
        /// Summary:
        ///     The entity is not being tracked by the context.
        ///Detached = 0,
        ///
        /// Summary:
        ///     The entity is being tracked by the context and exists in the database. Its property
        ///     values have not changed from the values in the database.
        ///Unchanged = 1,
        ///
        /// Summary:
        ///     The entity is being tracked by the context and exists in the database. It has
        ///     been marked for deletion from the database.
        ///Deleted = 2,
        ///
        /// Summary:
        ///     The entity is being tracked by the context and exists in the database. Some or
        ///     all of its property values have been modified.
        ///Modified = 3,
        ///
        /// Summary:
        ///     The entity is being tracked by the context but does not yet exist in the database.
        /// Added = 4
        /// </summary>
        [Required]
        public EntityState EntityState { get; set; }

        /// <summary>
        ///  Table name.
        /// </summary>
        [StringLength(50)]
        [Required]
        public string TableName { get; set; }

        /// <summary>
        ///  table's primary key field name.
        /// </summary>
        [StringLength(50)]
        // [Required]
        public string PrimaryKeyField { get; set; }

        /// <summary>
        ///  table's primar key value for the modified record.
        /// </summary>
        [StringLength(50)]
        // [Required]
        public string PrimaryKeyValue { get; set; }

        /// <summary>
        ///  table column name.
        /// </summary>
        [StringLength(50)]
        [Required]
        public string FieldName { get; set; }

        /// <summary>
        ///  Original value in the table column.
        /// </summary>
        public string OriginalValue { get; set; }
                
        /// <summary>
        ///  New update value in the table column.
        /// </summary>
        public string CurrentValue { get; set; }

        /// <summary>
        ///  It is UTC current date time.
        /// </summary>
        [Required]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        ///  Current logon user's account name. Optional.
        /// </summary>
        [StringLength(50)]
        public string CreatedBy { get; set; }
    }
}
