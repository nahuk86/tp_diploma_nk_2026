using System;
using System.Collections.Generic;
using DOMAIN.Entities;

namespace DOMAIN.Contracts
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        List<T> GetAll();
        List<T> GetAllActive();
        int Insert(T entity);
        void Update(T entity);
        void Delete(int id);
        void SoftDelete(int id, int deletedBy);
    }
}
