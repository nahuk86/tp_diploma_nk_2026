using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAO.Helpers
{
    public class DatabaseHelper
    {
        private static string _connectionString;

        static DatabaseHelper()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["StockManagerDB"]?.ConnectionString;
            
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'StockManagerDB' not found in configuration file.");
            }
        }

        public static string ConnectionString
        {
            get { return _connectionString; }
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public static void TestConnection()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                // If we reach here, connection is successful
            }
        }

        public static T ExecuteScalar<T>(string query, params SqlParameter[] parameters)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    var result = command.ExecuteScalar();
                    
                    if (result == null || result == DBNull.Value)
                        return default(T);

                    return (T)Convert.ChangeType(result, typeof(T));
                }
            }
        }

        public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public static SqlParameter CreateParameter(string name, object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }

        public static SqlParameter CreateOutputParameter(string name, SqlDbType dbType, int size = 0)
        {
            var parameter = new SqlParameter(name, dbType);
            parameter.Direction = ParameterDirection.Output;
            
            if (size > 0)
            {
                parameter.Size = size;
            }

            return parameter;
        }
    }
}
