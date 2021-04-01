using API.Core.DbModels;
using API.Core.Interfaces;
using API.Core.Specifications;
using API.Data.DataContext;
using API.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        //private readonly StoreContext _context;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly IMapper _mapper;
        public ProductsController(IGenericRepository<Product> productRepository, IGenericRepository<ProductType> productTypeRepository, IGenericRepository<ProductBrand> productBrandRepository,IMapper mapper)
        {
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
            _productBrandRepository = productBrandRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductWithProductTypeAndBrandsSpecification();
            var data = await _productRepository.ListAsync(spec);
            //return Ok(data);
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(data));

           

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithProductTypeAndBrandsSpecification(id);
            var product = await _productRepository.GetEntityWithSpec(spec);

            return _mapper.Map<Product, ProductToReturnDto>(product);

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