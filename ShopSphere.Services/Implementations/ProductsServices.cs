using ShopSphere.Data.Entities.Data;
using ShopSphere.Data.Interfaces;
using ShopSphere.Data.Specification.ProductSpec;
using ShopSphere.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.Services.Implementations
{
    public class ProductsServices : IProductsServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsServices(IUnitOfWork unitOfWork )
        {
           _unitOfWork = unitOfWork;
        }

        public Task<IReadOnlyList<Product?>> GetProductsAsync(ProductSpecParams productSpec)
        {
          var spec = new ProductSpecification (productSpec);
          var products= _unitOfWork.Repository<Product>().GetAllWihSpecAsync(spec);
            return products;
        } 
        
        public Task<IReadOnlyList<Product?>> GetProductsAsync()
        {
         
          var products= _unitOfWork.Repository<Product>().GetAllAsync();
            return products;
        }

        public Task<Product> GetProductByIdAsync(int id)
        {
            var spec = new ProductSpecification(id);
            var product = _unitOfWork.Repository<Product>().GetByIdAsync(id);
            return product;
        }
    }
}
