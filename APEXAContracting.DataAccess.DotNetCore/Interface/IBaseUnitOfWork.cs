using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Linq;
using System.Dynamic;

namespace APEXAContracting.DataAccess.DotNetCore.Interface
{
    /// <summary>
    ///  Base unit of work interface.
    /// </summary>
    public partial interface IBaseUnitOfWork : IDisposable
    {
        #region Save
        /// <summary>
        ///  Called dbContext.SaveChanges. Commit database transaction.
        /// </summary>
        /// <param name="enableAuditing">Default value = false. Enable record update auditing.</param>
        /// <param name="logonUser">Current logon user account. Work for auditing process. Update field "CreatedBy" in table AuditHistory.</param>
        /// <returns>The number of objects written to the underlying database.</returns>
        int Save(bool enableAuditing = false, string logonUser = "");

        /// <summary>
        ///  Save to database asynchronous.
        /// </summary>
        ///<param name="enableAuditing">Default value = false. Enable record update auditing.</param>
        /// <param name="logonUser">Current logon user account. Work for auditing process. Update field "CreatedBy" in table AuditHistory.</param>
        /// <returns>The number of objects written to the underlying database.</returns>
        Task<int> SaveAsync(bool enableAuditing = false, string logonUser = "");
        #endregion

        #region Stored procedures
        /// <summary>
        ///  Calling Stored Procedure with parameters. call exec [stored procedure] internally.
        ///  Sample codes to call this method:
        ///    this.UOW.ExecuteStoredProcedure("spName @parameter1, @parameter2, @parameter3", 
        ///                new SqlParameter("parameter1", SqlDbType.Int) { Value = value1},
        ///                new SqlParameter("parameter2", SqlDbType.Int) { Value = value2 },
        ///                new SqlParameter("parameter3", SqlDbType.VarChar) { Value = value3 }
        /// </summary>
        /// <param name="spName">stored procedure name.</param>
        /// <param name="parameters">stored procedure required parameters.</param>
        /// <returns></returns>
        //int ExecuteStoredProcedure(string spName, params object[] parameters);
        int ExecuteStoredProcedure(string spName, Dictionary<string, object> parameters);

        ///// <summary>
        /////  Return DataSet result through calling a stored procedure.
        /////  It is working for return dynamic collection data without strong typed domain model.
        ///// </summary>
        ///// <param name="storedProcedureName">Stored procedure name.</param>
        ///// <param name="parameters">Parameters for stored procedure. pair of parameter name and parameter value.</param>
        ///// <param name="commandTimeout">
        /////  Optional setting. It is integer value of seconds. If value >0, apply this setting to SqlCommand.CommandTimeout.
        /////  Work for Solve database timeout issues when process long time process. such as Recordset data calculation in batch save.
        /////  If no setting, using default setting of system.
        ///// </param>
        ///// <returns>A data set.</returns>
        //DataSet CallStoredProcedureWithReturn(string storedProcedureName, Dictionary<string, object> parameters, int commandTimeout = 0);

        /// <summary>
        ///  Work for calling sql with parameters.
        ///  Note: This method will not return collection data back. Only work for Update/Delete/Insert etc.
        /// </summary>
        /// <param name="sql">Sql for insert/update/delete.</param>
        /// <param name="parameters">parameters for stored procedure.</param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        ///// <summary>
        /////  Execute custom sql commands and return data set.
        ///// </summary>
        ///// <param name="cmdText"></param>
        ///// <param name="parameters"></param>
        ///// <param name="commandTimeout">
        /////  Optional setting. It is integer value of seconds. If value >0, apply this setting to SqlCommand.CommandTimeout.
        /////  Work for Solve database timeout issues when process long time process. such as Recordset data calculation in batch save.
        /////  If no setting, using default setting of system.
        ///// </param>
        ///// <returns></returns>
        //DataSet ExecuteQuery(string cmdText, Dictionary<string, object> parameters, int commandTimeout = 0);

        /// <summary>
        ///  Call sql stored procedure and return dynamic entity collections which is binding to record set.
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure with prefix.</param>
        /// <param name="parameters"></param>
        /// <param name="commandTimeout">
        ///  Optional setting. It is integer value of seconds. If value >0, apply this setting to SqlCommand.CommandTimeout.
        ///  Work for Solve database timeout issues when process long time process. such as Recordset data calculation in batch save.
        ///  If no setting, using default setting of system.
        /// </param>
        /// <returns></returns>
        List<ExpandoObject> ExecuteStoredProcedureWithDynamicReturn(string storedProcedureName, Dictionary<string, object> parameters, int commandTimeout = 0);

        /// <summary>
        ///  Execute custom sql and return dynamic entity collection which is binding to record set.
        /// </summary>
        /// <param name="sql">sql commands</param>
        /// <param name="parameters"></param>
        /// <param name="commandTimeout">
        ///  Optional setting. It is integer value of seconds. If value >0, apply this setting to SqlCommand.CommandTimeout.
        ///  Work for Solve database timeout issues when process long time process. such as Recordset data calculation in batch save.
        ///  If no setting, using default setting of system.
        /// </param>
        /// <returns></returns>
        List<ExpandoObject> ExecuteSQLWithDynamicReturn(string sql, Dictionary<string, object> parameters, int commandTimeout = 0);

        #endregion


        /// <summary>
        /// Gets the specified repository for the <typeparamref name="TEntity"/>.
        /// Note: It shares the same dbContext with other repository in same UnitOfWork instance, thus, multiple db entities can be processed in same db transaction.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from <see cref="IBaseRepository{TEntity}"/> interface.</returns>
        IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        
    }
}
