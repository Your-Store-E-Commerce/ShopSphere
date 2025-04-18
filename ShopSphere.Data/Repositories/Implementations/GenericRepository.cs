﻿using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.Data.Context;
using ShopSphere.Data.Repositories.Interfaces;
using ShopSphere.Data.Specification;


namespace ShopSphere.Data.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ShopSphere_DbContext _dbContext;

        public GenericRepository(ShopSphere_DbContext DbContext)
        {
            _dbContext = DbContext;

        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWihSpecAsync(IBaseSpecification<T> spec)
        {
            return await ApplyQuery(spec).ToListAsync();
        }


        public async Task<T> GetByIdWihSpecAsync(IBaseSpecification<T> spec)
        {
            return await ApplyQuery(spec).FirstOrDefaultAsync();
        }
        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);

        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);

        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        private IQueryable<T> ApplyQuery(IBaseSpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }


    }
}
