using Microsoft.EntityFrameworkCore;
using ShopSphere.Data.Context;
using ShopSphere.Data.Repositories.Interfaces;
using ShopSphere.Data.Specification;
using System.Formats.Asn1;


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

		public async Task<IReadOnlyList<T>> GetAllAsync()
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
		public async Task CreateAsync(T entity)
		{
			await _dbContext.Set<T>().AddAsync(entity);

		}

		public async Task Update(int id, T entity)
		{
			_dbContext.Set<T>().Update(entity);

		}

		public async Task Delete(int id)
		{
			 _dbContext.Remove(id);
		}

		private IQueryable<T> ApplyQuery(IBaseSpecification<T> spec)
		{
			return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
		}


	}
}
