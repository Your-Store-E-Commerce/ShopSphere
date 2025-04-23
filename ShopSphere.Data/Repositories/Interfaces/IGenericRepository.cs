using ShopSphere.Data;
using ShopSphere.Data.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.Data.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);

        Task<IReadOnlyList<TEntity>> GetAllWihSpecAsync(IBaseSpecification<TEntity> spec);
        Task<TEntity> GetByIdWihSpecAsync(IBaseSpecification<TEntity> spec);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

    }
}
