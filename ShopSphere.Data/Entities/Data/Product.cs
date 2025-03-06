﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.Data.Entities.Data
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public int TypeId { get; set; }
        public ProductType Type { get; set; } = null!;
        public int BrandId { get; set; }
        public ProductBrand Brand { get; set; } = null!;



    }
}
