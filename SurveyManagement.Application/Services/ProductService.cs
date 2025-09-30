using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SurveyManagement.Application.DTOS;
using SurveyManagement.Domain.Entities;
using SurveyManagement.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyManagement.Application.Services
{
   

    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ProductDto> AddProductAsync(CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            product.ProductId = Guid.NewGuid();
            await _repo.AddAsync(product);
            await _repo.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            var product = await _repo.GetByIdAsync(id);
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductDto dto)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException("Product not found");

            _mapper.Map(dto, product);
            _repo.Update(product);
            await _repo.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException("Product not found");

            _repo.Delete(product);
            await _repo.SaveChangesAsync();
        }
    }
}
