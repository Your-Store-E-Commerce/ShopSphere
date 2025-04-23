using ShopSphere.Data.Entities.Data;
using ShopSphere.Data.Specification.ProductSpec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.Services.Interfaces
{
    public interface IProductsServices
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams productSpec);
        Task<Product> GetProductByIdAsync(int id);
      

    }
}
