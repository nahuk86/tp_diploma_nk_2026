using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAO.Helpers
{
    /// <summary>
    /// Clase auxiliar para gestionar la conexión y operaciones con la base de datos
    /// </summary>
    public class DatabaseHelper
    {
        private static string _connectionString;

        /// <summary>
        /// Inicializa la cadena de conexión a la base de datos desde el archivo de configuración
        /// </summary>
        static DatabaseHelper()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["StockManagerDB"]?.ConnectionString;
            
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'StockManagerDB' not found in configuration file.");
            }
        }

        /// <summary>
        /// Obtiene la cadena de conexión configurada para la base de datos
        /// </summary>
        public static string ConnectionString
        {
            get { return _connectionString; }
        }

        /// <summary>
        /// Crea y devuelve una nueva conexión SQL a la base de datos
        /// </summary>
        /// <returns>Objeto SqlConnection configurado</returns>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Prueba la conexión a la base de datos
        /// </summary>
        public static void TestConnection()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                // If we reach here, connection is successful
            }
        }

        /// <summary>
        /// Ejecuta una consulta SQL y devuelve un valor escalar del tipo especificado
        /// </summary>
        /// <typeparam name="T">Tipo de dato del resultado esperado</typeparam>
        /// <param name="query">Consulta SQL a ejecutar</param>
        /// <param name="parameters">Parámetros de la consulta</param>
        /// <returns>Valor escalar del tipo especificado</returns>
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

        /// <summary>
        /// Ejecuta una consulta SQL que no devuelve resultados (INSERT, UPDATE, DELETE)
        /// </summary>
        /// <param name="query">Consulta SQL a ejecutar</param>
        /// <param name="parameters">Parámetros de la consulta</param>
        /// <returns>Número de filas afectadas</returns>
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

        /// <summary>
        /// Crea un parámetro SQL con el nombre y valor especificados
        /// </summary>
        /// <param name="name">Nombre del parámetro</param>
        /// <param name="value">Valor del parámetro</param>
        /// <returns>Objeto SqlParameter configurado</returns>
        public static SqlParameter CreateParameter(string name, object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }

        /// <summary>
        /// Crea un parámetro de salida SQL con el nombre y tipo de dato especificados
        /// </summary>
        /// <param name="name">Nombre del parámetro</param>
        /// <param name="dbType">Tipo de dato SQL del parámetro</param>
        /// <param name="size">Tamaño del parámetro (opcional)</param>
        /// <returns>Objeto SqlParameter configurado como parámetro de salida</returns>
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
