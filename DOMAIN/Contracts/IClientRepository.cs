using System.Collections.Generic;
using DOMAIN.Entities;

namespace DOMAIN.Contracts
{
    public interface IClientRepository : IRepository<Client>
    {
        /// <summary>
        /// Obtiene un cliente por su número de DNI
        /// </summary>
        Client GetByDNI(string dni);
        
        /// <summary>
        /// Verifica si un DNI ya existe en el sistema, con opción de excluir un cliente específico
        /// </summary>
        bool DNIExists(string dni, int? excludeClientId = null);
    }
}
