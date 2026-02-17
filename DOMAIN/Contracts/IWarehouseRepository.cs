using System.Collections.Generic;
using DOMAIN.Entities;

namespace DOMAIN.Contracts
{
    public interface IWarehouseRepository : IRepository<Warehouse>
    {
        /// <summary>
        /// Obtiene un almacén por su código único
        /// </summary>
        Warehouse GetByCode(string code);
        
        /// <summary>
        /// Verifica si un código de almacén ya existe en el sistema, con opción de excluir un almacén específico
        /// </summary>
        bool CodeExists(string code, int? excludeWarehouseId = null);
    }
}
