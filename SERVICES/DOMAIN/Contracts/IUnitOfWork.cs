using System;
using System.Data.SqlClient;

namespace DOMAIN.Contracts
{
    /// <summary>
    /// Interfaz del patrón Unit of Work.
    /// Coordina operaciones de escritura en múltiples repositorios dentro de una sola transacción de base de datos,
    /// garantizando la atomicidad de operaciones que afectan a varias tablas.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Conexión SQL activa compartida entre repositorios participantes
        /// </summary>
        SqlConnection Connection { get; }

        /// <summary>
        /// Transacción activa compartida entre repositorios participantes
        /// </summary>
        SqlTransaction Transaction { get; }

        /// <summary>
        /// Inicia la transacción de base de datos
        /// </summary>
        void Begin();

        /// <summary>
        /// Confirma todos los cambios realizados durante la transacción
        /// </summary>
        void Commit();

        /// <summary>
        /// Revierte todos los cambios de la transacción en caso de error
        /// </summary>
        void Rollback();
    }
}
