using System.Collections.Generic;
using DOMAIN.Entities;

namespace DOMAIN.Contracts
{
    public interface IClientRepository : IRepository<Client>
    {
        Client GetByDNI(string dni);
        bool DNIExists(string dni, int? excludeClientId = null);
    }
}
