using API.Core.DbModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace API.Core.Specifications
{
    public class ProductWithProductTypeAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductWithProductTypeAndBrandsSpecification(string sort,int? brandId,int? typeId)
                            :base(x=>
                            (!brandId.HasValue || x.ProductBrandId == brandId) && (!typeId.HasValue || x.ProductTypeId == typeId)
                            )
        {
            AddInclude(x => x.ProductBrand);
            AddInclude(x => x.ProductType);
            AddOrderBy(x => x.Name);

            if (!string.IsNullOrWhiteSpace(sort))
            {
                switch (sort)
                {
                    case "priceAsc":
                            AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;

                }
            }       
        }
        public ProductWithProductTypeAndBrandsSpecification(int id) 
            : base(x=>x.Id == id)
        {
            AddInclude(x => x.ProductBrand);
            AddInclude(x => x.ProductType);
        }
       
    }
}
