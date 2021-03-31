using API.Core.DbModels;
using API.Core.Interfaces;
using API.Core.Specifications;
using API.Data.DataContext;
using API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //private readonly StoreContext _context;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;

        public ProductsController(IGenericRepository<Product> productRepository, IGenericRepository<ProductType> productTypeRepository, IGenericRepository<ProductBrand> productBrandRepository)
        {
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
            _productBrandRepository = productBrandRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductWithProductTypeAndBrandsSpecification();
            var data = await _productRepository.ListAsync(spec);
            // return Ok(data);
            return data.Select(product => new ProductToReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand != null ? product.ProductBrand.Name : string.Empty,
                ProductType = product.ProductType != null ? product.ProductType.Name : string.Empty
            }).ToList();

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithProductTypeAndBrandsSpecification(id);
            var product = await _productRepository.GetEntityWithSpec(spec);
            return new ProductToReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand!=null? product.ProductBrand.Name : string.Empty,
                ProductType = product.ProductType!=null ? product.ProductType.Name : string.Empty
            };

        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var data = await _productBrandRepository.ListAllAsync();
            return Ok(data);

        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var data = await _productTypeRepository.ListAllAsync();
            return Ok(data);

        }
    }
}