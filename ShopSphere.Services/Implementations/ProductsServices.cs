using ShopSphere.Data.Entities.Data;
using ShopSphere.Data.Interfaces;
using ShopSphere.Data.Specification.ProductSpec;
using ShopSphere.Services.Interfaces;

namespace ShopSphere.Services.Implementations
{
	public class ProductsServices : IProductsServices
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductsServices(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams productSpec)
		{
			var spec = new ProductSpecification(productSpec);
			var products =await _unitOfWork.Repository<Product>().GetAllWihSpecAsync(spec);
			return products;
		}


		public async Task<Product> GetProductByIdAsync(int id)
		{
			var spec = new ProductSpecification(id);
			var product = await _unitOfWork.Repository<Product>().GetByIdWihSpecAsync(spec);
			return product;
		}

		

		 // Admin Services
		public async Task CreateAsync(Product product)
		{
			await _unitOfWork.Repository<Product>().CreateAsync(product);
			await _unitOfWork.CompleteAsync();	
		}

		public async Task UpdateAsync(int id, Product product)
		{
			await _unitOfWork.Repository<Product>().Update(id, product);
			await  _unitOfWork.CompleteAsync();
		}

		public async Task DeleteAsync(int id)
		{
			await  _unitOfWork.Repository<Product>().Delete(id);
			await  _unitOfWork.CompleteAsync();
		}

		public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
		{
			return await _unitOfWork.Repository<ProductBrand>().GetAllAsync();	
		}

		public async Task<IReadOnlyList<ProductType>> GetTypesAsync()
		{
			return await _unitOfWork.Repository<ProductType>().GetAllAsync();
		}

	
	}
}
