using ShopSphere.Data;
using ShopSphere.Data.Context;
using ShopSphere.Data.Interfaces;
using ShopSphere.Data.Repositories.Implementations;
using ShopSphere.Data.Repositories.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopSphere_DbContext _dbContext;
        private Hashtable _repository;

        public UnitOfWork(ShopSphere_DbContext DbContext)
        {
            _dbContext = DbContext;
            _repository = new Hashtable();
        }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;
            if (!_repository.ContainsKey(key))
            {
                var Repository = new GenericRepository<TEntity>(_dbContext);
                _repository.Add(key, Repository);
            }
            return _repository[key] as IGenericRepository<TEntity>;
        }
        public Task<int> CompleteAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public ValueTask DisposeAsync()
        {
            return _dbContext.DisposeAsync();
        }
    }
}
