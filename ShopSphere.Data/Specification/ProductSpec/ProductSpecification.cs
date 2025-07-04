﻿using ShopSphere.Data.Entities.Data;

namespace ShopSphere.Data.Specification.ProductSpec
{
    public class ProductSpecification : BaseSpecification<Product>
    {

        public ProductSpecification(ProductSpecParams specParams) : base(P =>
        (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search)) &&
        (!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId.Value) &&
        (!specParams.TypeId.HasValue || P.TypeId == specParams.TypeId.Value))
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Type);

            if (!string.IsNullOrEmpty(specParams.sort))
            {
                switch (specParams.sort)
                {
                    case "PriceAsc":
                        OrderBy = o => o.Price;
                        break;
                    case "PriceDesc":
                        OrderByDesc = o => o.Price;
                        break;
                    default:
                        OrderBy = o => o.Name;
                        break;
                }
            }
            else
                OrderBy = o => o.Name;

            // إضافة الـ Pagination:
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }

        public ProductSpecification(int? id) : base(p => p.Id == id)
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Type);
        }

        //       public ProductSpecification(ProductSpecParams specParams) :
        //base(P =>
        //   (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search)) &&
        //   (!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId.Value) &&
        //   (!specParams.TypeId.HasValue || P.TypeId == specParams.TypeId.Value))
        //       {
        //           Includes.Add(P => P.Brand);
        //           Includes.Add(P => P.Type);
        //           if (!string.IsNullOrEmpty(specParams.sort))
        //           {
        //               switch (specParams.sort)
        //               {
        //                   case "PriceAsc":
        //                       OrderBy = o => o.Price;
        //                       break;
        //                   case "PriceDesc":
        //                       OrderByDesc = o => o.Price;
        //                       break;
        //                   default:
        //                       OrderBy = o => o.Name;
        //                       break;
        //               }
        //           }
        //           else
        //               OrderBy = o => o.Name;


        //           //ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        //       }

        //       public ProductSpecification(int? id) : base(p => p.Id == id)
        //       {

        //           Includes.Add(p => p.Brand);
        //           Includes.Add(p => p.Type);
        //       }

    }
}
