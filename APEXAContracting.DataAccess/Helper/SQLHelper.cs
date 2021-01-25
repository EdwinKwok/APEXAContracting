using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using APEXAContracting.Common;

namespace APEXAContracting.DataAccess
{
    public static class SQLHelper
    {
        const int TimeoutInSeconds = 5 * 60;//5 minutes

        public static SqlDataReader ExecuteReader(SqlConnection conn, string commandText, ILogger logger, IEnumerable<SqlParameter> parameters = null)
        {
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = commandText;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = TimeoutInSeconds;
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();
                return cmd.ExecuteReader(System.Data.CommandBehavior.Default);
            }
            catch (Exception ex)
            {
                logger.LogError("ExecuteReader. Connection: {0}, CommandText: {1}, Parameters: {2} exception: {3}", conn!=null?conn.ConnectionString: "None", commandText, parameters.GetParametersString(),  ex.ToString());
                throw ex;
            }
        }

        public static int ExecuteNonQuery(SqlConnection conn, string commandText, ILogger logger, IEnumerable<SqlParameter> parameters = null)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = commandText;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = TimeoutInSeconds;
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters.ToArray());
            }

            int val = 0;
            try
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();
                val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch(Exception ex)
            {
                logger.LogError("ExecuteNonQuery. Connection: {0}, CommandText: {1}, Parameters: {2}, Exception: {3}", conn!=null?conn.ConnectionString: "None",  commandText, parameters.GetParametersString(), ex.ToString());
               throw ex;
            }
            finally
            {
                if (cmd != null)
                    cmd.Parameters.Clear();
            }
            return val;
        }

        public static object ExecuteScalar(SqlConnection conn, string commandText, ILogger logger, IEnumerable<SqlParameter> parameters = null)
        {
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = commandText;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = TimeoutInSeconds;
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex) {
                logger.LogError("ExecuteScalar. Connection: {0}, CommandText: {1}, Parameters: {2}, Exception: {3}", conn!=null?conn.ConnectionString:"None", commandText, parameters.GetParametersString(), ex.ToString());
                throw ex;
            }
        }

        public static Guid GetGuid(this SqlDataReader dr, string fieldName)
        {
            return dr.GetGuid(dr.GetOrdinal(fieldName));
        }

        public static string GetString(this SqlDataReader dr, string fieldName)
        {
            if (dr.IsDBNull(dr.GetOrdinal(fieldName)))
                return null;
            else
                return dr.GetString(dr.GetOrdinal(fieldName));
        }

        public static int GetInt32(this SqlDataReader dr, string fieldName)
        {
            return dr.GetInt32(dr.GetOrdinal(fieldName));
        }

        public static long GetInt64(this SqlDataReader dr, string fieldName)
        {
            return dr.GetInt64(dr.GetOrdinal(fieldName));
        }

        public static bool GetBoolean(this SqlDataReader dr, string fieldName)
        {
            return dr.GetBoolean(dr.GetOrdinal(fieldName));
        }

        public static DateTime GetDateTime(this SqlDataReader dr, string fieldName)
        {
            return dr.GetDateTime(dr.GetOrdinal(fieldName));
        }

        public static DateTime? GetDateTime2(this SqlDataReader dr, string fieldName)
        {
            try
            {
                int idx = dr.GetOrdinal(fieldName);
                if (dr[idx] != DBNull.Value)
                    return dr.GetDateTime(idx);
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }


        public static Decimal GetDecimal(this SqlDataReader dr, string fieldName)
        {
            return dr.GetDecimal(dr.GetOrdinal(fieldName));
        }
        public static int ConvertToInt32(this SqlDataReader dr,string fieldName)
        {
            object value = dr.GetValue(dr.GetOrdinal(fieldName));
            if (value != DBNull.Value)
                return Convert.ToInt32(value);
            else
                return 0;
        }

        public static string GetParametersString(this IEnumerable<SqlParameter> parameters)
        {
            string result = string.Empty;
         
            if (parameters != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var parameter in parameters)
                {
                    if (parameter.Value != null)
                    {
                        sb.AppendLine(parameter.ParameterName + ": " + parameter.Value.ToString() + ", ");
                    }
                }
                result = sb.ToString();
            }

            return result;
        }
    }
}
