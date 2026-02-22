using System;
using System.Configuration;
using System.Data.SqlClient;
using DOMAIN.Contracts;

namespace SERVICES.DAL
{
    /// <summary>
    /// Implementación del patrón Unit of Work.
    /// Gestiona una transacción SQL que envuelve operaciones de múltiples repositorios,
    /// garantizando que todas se confirmen o deshagan de forma atómica.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction;
        private bool _disposed;

        /// <inheritdoc/>
        public SqlConnection Connection => _connection;

        /// <inheritdoc/>
        public SqlTransaction Transaction => _transaction;

        /// <summary>
        /// Crea una nueva instancia del UnitOfWork usando la cadena de conexión configurada
        /// </summary>
        public UnitOfWork()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["StockManagerDB"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string 'StockManagerDB' not found in configuration.");

            _connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Crea una nueva instancia del UnitOfWork con la cadena de conexión indicada
        /// </summary>
        /// <param name="connectionString">Cadena de conexión a la base de datos</param>
        public UnitOfWork(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _connection = new SqlConnection(connectionString);
        }

        /// <inheritdoc/>
        public void Begin()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();

            _transaction = _connection.BeginTransaction();
        }

        /// <inheritdoc/>
        public void Commit()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No hay una transacción activa. Llame a Begin() primero.");

            _transaction.Commit();
        }

        /// <inheritdoc/>
        public void Rollback()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No hay una transacción activa. Llame a Begin() primero.");

            _transaction.Rollback();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!_disposed)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
                _disposed = true;
            }
        }
    }
}
