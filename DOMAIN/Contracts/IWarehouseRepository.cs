using System.Collections.Generic;
using DOMAIN.Entities;

namespace DOMAIN.Contracts
{
    public interface IWarehouseRepository : IRepository<Warehouse>
    {
        Warehouse GetByCode(string code);
        bool CodeExists(string code, int? excludeWarehouseId = null);
    }
}
