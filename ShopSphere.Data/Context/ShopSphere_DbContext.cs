using ShopSphere.Data.Entities.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ShopSphere.Data.Context
{
    public class ShopSphere_DbContext :IdentityDbContext
    {
        public ShopSphere_DbContext(DbContextOptions<ShopSphere_DbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; } 

    }
}
