using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mall_Related_API.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAllEntities();
        IQueryable<T> GetAllEntitiesIQueryable();
        int GetNumberOfEntities();
        void Add(T entity);
        void Update(T entity);
        T GetEntity(int entityID);
        T GetEntity(string entityID);
        void Remove(int entityID);
        void  Remove(string entityID);


    }
}
