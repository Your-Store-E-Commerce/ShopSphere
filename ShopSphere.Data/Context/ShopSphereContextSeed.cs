using Microsoft.EntityFrameworkCore;
using ShopSphere.Data.Entities.Data;
using ShopSphere.Data.Entities.Order;
using ShopSphere.Data.Migrations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopSphere.Data.Context
{
    public class ShopSphereContextSeed
    {

		public static async Task ApplySeedingAsync(ShopSphere_DbContext context)
		{
			var basePath = Path.Combine(AppContext.BaseDirectory, "SeedData");

			if (!context.ProductBrands.Any())
			{
				var brandPath = Path.Combine(basePath, "brand.json");
				var brandData = await File.ReadAllTextAsync(brandPath);
				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
				if (brands?.Count > 0)
				{
					await context.Set<ProductBrand>().AddRangeAsync(brands);
					await context.SaveChangesAsync();
				}
			}

			if (!context.ProductTypes.Any())
			{
				var typePath = Path.Combine(basePath, "type.json");
				var typeData = await File.ReadAllTextAsync(typePath);
				var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);
				if (types?.Count > 0)
				{
					await context.Set<ProductType>().AddRangeAsync(types);
					await context.SaveChangesAsync();
				}
			}

			if (!context.Products.Any())
			{
				var productPath = Path.Combine(basePath, "product.json");
				var productData = await File.ReadAllTextAsync(productPath);
				var products = JsonSerializer.Deserialize<List<Product>>(productData);
				if (products?.Count > 0)
				{
					await context.Set<Product>().AddRangeAsync(products);
					await context.SaveChangesAsync();
				}
			}
			
			if (!context.DeliveryMethods.Any())
			{
              
                var deliverPath = Path.Combine(basePath, "delivery.json");
				var deliverData = await File.ReadAllTextAsync(deliverPath);
				var deliver = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliverData);
				if (deliver?.Count > 0)
				{
					await context.Set<DeliveryMethod>().AddRangeAsync(deliver);
					await context.SaveChangesAsync();
				}
			}
		}


	}
}

