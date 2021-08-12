using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Mall_Related_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Mall_Related_API.Repository
{
    public class Repository<T> : IRepository<T> where T:class
    {
        private readonly NORTHWNDContext _context;
        private readonly DbSet<T> _dbset;

        public Repository(NORTHWNDContext context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbset.Add(entity);
            _context.SaveChanges();
        }

        public IEnumerable<T> GetAllEntities()
        {
            return _dbset.ToList();
        }



        public IQueryable<T> GetAllEntitiesIQueryable()
        {
            return _dbset;
        }

        public IEnumerable<T> GetEntitiesToShow(int PageNo, int pageSize, int currentPage, Expression<Func<T, bool>> wherePredict, Expression<Func<T, int>> orderByPredict)
        {
            if (wherePredict != null)
            {
                return _dbset.OrderBy(orderByPredict).Where(wherePredict).ToList();
            }
            else
            {
                return _dbset.OrderBy(orderByPredict).ToList();
            }
        }

        public T GetEntity(int entityID)
        {
            return _dbset.Find(entityID);
        }
        //over loading of GetEntity with string parameter

        public T GetEntity(string entityID)
        {
            return _dbset.Find(entityID);
        }

        public T GetEntityByParamter(Expression<Func<T, bool>> wherePredict)
        {
            return _dbset.Where(wherePredict).FirstOrDefault();
        }

        public IEnumerable<T> GetListByParamter(Expression<Func<T, bool>> wherePredict)
        {
            return _dbset.Where(wherePredict).ToList();
        }

        public int GetNumberOfEntities()
        {
            return _dbset.Count();
        }

        public IQueryable<T> GetResultBySqlProcedure(string query, params object[] paramters)
        {
            if(paramters != null)
            {
                return _dbset.FromSqlRaw<T>(query, paramters);
            }
            else
            {
                return _dbset.FromSqlRaw<T>(query);
            }
            
        }

        public void Remove(int entityID)
        {
            _dbset.Remove(GetEntity(entityID));
            _context.SaveChanges();
        }
        //over loading of remove with string parameter

        public void Remove(string entityID)
        {
            _dbset.Remove(GetEntity(entityID));
            _context.SaveChanges();
        }

        public void RemoveByWhereClause(Expression<Func<T, bool>> wherePredict)
        {
            var entity = _dbset.Where(wherePredict).SingleOrDefault();
            _dbset.Remove(entity);
            _context.SaveChanges();
        }

        public void RemoveRangeByWhereClause(Expression<Func<T, bool>> wherePredict)
        {
            var entities = _dbset.Where(wherePredict).ToList();
            _dbset.RemoveRange(entities);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbset.Update(entity);
            _context.SaveChanges();
        }

        public void UpdateByWhereClause(Expression<Func<T, bool>> wherePredict, Action<T> forEachPredict)
        {
            _dbset.Where(wherePredict).ToList().ForEach(forEachPredict);
            _context.SaveChanges();
        }


    }
}
