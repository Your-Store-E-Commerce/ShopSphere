using ShopSphere.Data.Entities.Data;
using ShopSphere.Data.Interfaces;
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

        public Task<IEnumerable<Product>> GetProductsAsync()
        {
          var products= _unitOfWork.Repository<Product>().GetAllAsync();
            return products;
        }

        public Task<Product> GetProductByIdAsync(int id)
        {
            var product = _unitOfWork.Repository<Product>().GetByIdAsync(id);
            return product;
        }
    }
}
