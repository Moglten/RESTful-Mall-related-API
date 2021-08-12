using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mall_Related_API.Models;

namespace Mall_Related_API.Repository
{
    public class UnitOfWork 
    {
        private readonly NORTHWNDContext DbEntity = new();
        public IRepository<T> GetRepositoryInstance<T>() where T : class
        {
            return new Repository<T>(DbEntity);
        }


        public NORTHWNDContext GetDBInstance() 
        {
            return DbEntity;
        }

        public void SaveChages()
        {
            DbEntity.SaveChangesAsync();
        }





    }
}
