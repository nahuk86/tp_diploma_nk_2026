using System;
using System.Collections.Generic;
using DOMAIN.Entities;

namespace DOMAIN.Contracts
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Obtiene una entidad por su identificador
        /// </summary>
        T GetById(int id);
        
        /// <summary>
        /// Obtiene todas las entidades sin filtrar
        /// </summary>
        List<T> GetAll();
        
        /// <summary>
        /// Obtiene todas las entidades activas
        /// </summary>
        List<T> GetAllActive();
        
        /// <summary>
        /// Inserta una nueva entidad y retorna su identificador
        /// </summary>
        int Insert(T entity);
        
        /// <summary>
        /// Actualiza una entidad existente
        /// </summary>
        void Update(T entity);
        
        /// <summary>
        /// Elimina permanentemente una entidad por su identificador
        /// </summary>
        void Delete(int id);
        
        /// <summary>
        /// Realiza un borrado lógico de una entidad
        /// </summary>
        void SoftDelete(int id, int deletedBy);
    }
}
