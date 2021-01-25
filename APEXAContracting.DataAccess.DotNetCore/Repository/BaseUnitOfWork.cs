using AutoMapper;
using APEXAContracting.DataAccess.DotNetCore.Audit;
using APEXAContracting.DataAccess.DotNetCore.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;


namespace APEXAContracting.DataAccess.DotNetCore.Repository
{
    /// <summary>
    ///  Base unit of work.
    /// </summary>
    public partial class BaseUnitOfWork : IBaseUnitOfWork
    {
        #region Properties
        /// <summary>
        ///  Represent one database instance.
        /// </summary>
        protected BaseDbContext DbContext { get; private set; }
        /// <summary>
        ///  Auto mapper instance.
        /// </summary>
        protected IMapper Mapper { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        ///  Corporate with dependency injection with DbContext..
        /// </summary>
        /// <param name="context">Database context.</param>
        /// <param name="mapper">Dependency inject Auto mapper instance.</param>
        public BaseUnitOfWork(DbContext context, IMapper mapper = null)
        {
            this.DbContext = context as BaseDbContext;
            this.Mapper = mapper;
        }
        #endregion

        #region Save

        /// <summary>
        /// Called dbContext.SaveChanges. Commit database transaction.
        /// </summary>
        /// <param name="enableAuditing">Defualt value = false. If value = true, enable auditing for the updated records.</param>
        /// <param name="logonUser">Current logon user account.</param>
        /// <returns>The number of objects written to the underlying database.</returns>
        public int Save(bool enableAuditing = false, string logonUser = "")
        {
            if (enableAuditing)
            {
                this.DbContext.EnsureAudit(logonUser);
            }

            var result = this.DbContext.SaveChanges();

            return result;
        }

        /// <summary>
        ///  Save data into database asynchronization.
        /// </summary>
        /// <param name="enableAuditing">Defualt value = false. If value = true, enable auditing for the updated records.</param>
        /// <param name="logonUser">Current logon user account.</param>
        /// <returns>The number of objects written to the underlying database.</returns>
        public async Task<int> SaveAsync(bool enableAuditing = false, string logonUser = "")
        {
            if (enableAuditing)
            {
                this.DbContext.EnsureAudit(logonUser);
            }

            int result = await this.DbContext.SaveChangesAsync();

            return result;
        }

        #endregion

        #region Retrieve Repository
        /// <summary>
        ///  Repositories container in one unit of work.
        ///  Note: Each repository here shared the same dbcontext.
        /// </summary>
        private Dictionary<Type, object> repositories;

        /// <summary>
        /// Gets the specified repository for the <typeparamref name="TEntity"/>.
        /// Note: It shares the same dbContext with other repository in same UnitOfWork instance, thus, multiple db entities can be processed in same db transaction.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from <see cref="IBaseRepository{TEntity}"/> interface.</returns>
        public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);

            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new BaseRepository<TEntity>(this.DbContext, this.Mapper);
            }

            return (IBaseRepository<TEntity>)repositories[type];
        }
        #endregion

        #region IDisposable
        /// <summary>
        ///  Release database connection. For performance purpose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///  Release database connection.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (DbContext != null)
                    {
                        DbContext.Dispose();
                    }
                }
                catch
                {
                    // Hide exceptions.
                }
            }
        }
        #endregion

        #region Stored Procedure Related
        /// <summary>
        ///  Calling Stored Procedure with parameters. call exec [stored procedure] internally.
        ///  Sample codes to call this method:
        ///    this.UOW.ExecuteStoredProcedure("spName @parameter1, @parameter2, @parameter3", 
        ///                new SqlParameter("parameter1", SqlDbType.Int) { Value = value1},
        ///                new SqlParameter("parameter2", SqlDbType.Int) { Value = value2 },
        ///                new SqlParameter("parameter3", SqlDbType.VarChar) { Value = value3 }
        ///                
        /// 
        ///   Note: the method is not used for returning entities.
        ///   
        ///  https://www.learnentityframeworkcore.com/raw-sql
        /// </summary>
        /// <param name="spName">stored procedure name.</param>
        /// <param name="parameters">stored procedure required parameters.</param>
        /// <returns>
        ///    Returns an integer specifying the number of rows affected by the sql statement passed to it. 
        ///    Valid operations are Insert, Update, Delete.
        ///    Note: the method is not used for returning entities.
        /// </returns>
        public int ExecuteStoredProcedure(string spName, Dictionary<string, object> parameters)
        {
            int result;
            var sqlConn = this.DbContext.Database.GetDbConnection();
            var cmd = sqlConn.CreateCommand();
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameters != null && parameters.Count > 0)
            {
                using (cmd)
                {
                    foreach (var item in parameters)
                    {
                        Microsoft.Data.SqlClient.SqlParameter parameter = new Microsoft.Data.SqlClient.SqlParameter(item.Key, item.Value);
                        cmd.Parameters.Add(parameter);
                    }

                }
            }
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            try
            {
                result = cmd.ExecuteNonQuery();
                sqlConn.Close();
            }
            catch (Exception ex)
            {
                sqlConn.Close();
                throw ex;
            }
            return result;
        }

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
        //public DataSet CallStoredProcedureWithReturn(string storedProcedureName, Dictionary<string, object> parameters, int commandTimeout = 0)
        //{
        //    DataSet result = new DataSet();

        //    IDbConnection sqlConn = this.DbContext.Database.GetDbConnection() as IDbConnection;


        //    IDbCommand cmd = sqlConn.CreateCommand(); // new SqlCommand(storedProcedureName, sqlConn);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = storedProcedureName;

        //    //
        //    // Important setup here. Solve database timeout issues when process long time process. 
        //    // such as Recordset data calculation in batch save.
        //    //
        //    if (commandTimeout > 0) {
        //        cmd.CommandTimeout = commandTimeout;
        //    }

        //    SqlDataAdapter apt = cmd.d new SqlDataAdapter(cmd);

        //    using (cmd)
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        if (parameters != null)
        //        {
        //            foreach (var item in parameters)
        //            {
        //                SqlParameter parameter = new SqlParameter(item.Key, item.Value);
        //                cmd.Parameters.Add(parameter);
        //            }
        //        }

        //        cmd.

        //        apt.Fill(result);

        //        // Always make sure close database connection first.
        //        cmd.Connection.Close();
        //    }

        //    return result;
        //}


        /// <summary>
        ///  Work for calling sql with parameters.
        ///  Note: This method will not return collection data back. Only work for Update/Delete/Insert etc.
        ///  
        /// var user = new SqlParameter("user", "johndoe");
        /// var blogs = context.Blogs
        ///    .FromSqlRaw("EXECUTE dbo.GetMostPopularBlogsForUser @user", user)
        ///    .ToList();
        ///    
        /// </summary>
        /// <param name="sql">Sql for insert/update/delete.</param>
        /// <param name="parameters">parameters for stored procedure. For example, new SqlParameter("user", "johndoe");</param>
        /// <returns></returns>
        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return this.DbContext.Database.ExecuteSqlRaw(sql, parameters);
        }


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
        //public DataSet ExecuteQuery(string cmdText, Dictionary<string, object> parameters, int commandTimeout = 0)
        //{
        //    DataSet result = new DataSet();
        //    SqlConnection sqlConn = this.DbContext.Database.GetDbConnection() as SqlConnection;
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand(cmdText, sqlConn);
        //        // Important setup here. Solve database timeout issues when process long time process. such as Recordset data calculation in batch save.
        //        if (commandTimeout > 0) cmd.CommandTimeout = commandTimeout;
        //        SqlDataAdapter apt = new SqlDataAdapter(cmd);
        //        using (cmd)
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
        //            if (parameters != null)
        //            {
        //                foreach (var item in parameters)
        //                {
        //                    SqlParameter parameter = new SqlParameter(item.Key, item.Value);
        //                    cmd.Parameters.Add(parameter);
        //                }
        //            }
        //            apt.Fill(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return result;
        //}


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
        public List<ExpandoObject> ExecuteStoredProcedureWithDynamicReturn(string storedProcedureName, Dictionary<string, object> parameters, int commandTimeout = 0)
        {
            List<ExpandoObject> result = new List<ExpandoObject>();

            try
            {
                var sqlConn = this.DbContext.Database.GetDbConnection();
                var cmd = sqlConn.CreateCommand();
                cmd.CommandText = storedProcedureName;
                cmd.CommandType = CommandType.StoredProcedure;

                if (commandTimeout > 0)
                {
                    cmd.CommandTimeout = commandTimeout;
                }

                using (cmd)
                {
                    if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
                    //
                    // Note: Call Microsoft.Data.SqlClient.SqlParameter
                    // Reference: https://stackoverflow.com/questions/58153228/system-invalidcastexception-the-sqlparametercollection-only-accepts-non-null-s
                    //
                    if (parameters != null)
                    {
                        foreach (var item in parameters)
                        {
                            Microsoft.Data.SqlClient.SqlParameter parameter = new Microsoft.Data.SqlClient.SqlParameter(item.Key, item.Value);
                            cmd.Parameters.Add(parameter);
                        }
                    }


                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            IDictionary<string, object> item = new ExpandoObject() as IDictionary<string, object>;
                            for (int i = 0; i < rd.FieldCount; i++)
                            {
                                string key = rd.GetName(i);
                                object value = rd.GetValue(i);

                                if (rd.GetValue(i).GetType() != typeof(DBNull))
                                    item[key] = value;
                                else
                                    item[key] = null;
                            }
                            result.Add((ExpandoObject)item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }

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
        public List<ExpandoObject> ExecuteSQLWithDynamicReturn(string sql, Dictionary<string, object> parameters, int commandTimeout = 0)
        {
            List<ExpandoObject> result = new List<ExpandoObject>();
            var sqlConn = this.DbContext.Database.GetDbConnection();

            try
            {
                IDbCommand cmd = sqlConn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                if (commandTimeout > 0) cmd.CommandTimeout = commandTimeout;

                using (cmd)
                {
                    if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }

                    if (parameters != null)
                    {
                        foreach (var item in parameters)
                        {
                            //
                            // Reference: https://stackoverflow.com/questions/58153228/system-invalidcastexception-the-sqlparametercollection-only-accepts-non-null-s
                            //
                            Microsoft.Data.SqlClient.SqlParameter parameter = new Microsoft.Data.SqlClient.SqlParameter(item.Key, item.Value);
                            cmd.Parameters.Add(parameter);
                        }
                    }

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            IDictionary<string, object> item = new ExpandoObject() as IDictionary<string, object>;
                            for (int i = 0; i < rd.FieldCount; i++)
                            {
                                string key = rd.GetName(i);
                                object value = rd.GetValue(i);

                                if (value == null || value == DBNull.Value)
                                    item[key] = null;
                                else
                                    item[key] = value;
                            }
                            result.Add((ExpandoObject)item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        #endregion

    }

}
