using SurveyManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    void Update(Product product);
    void Delete(Product product);
    Task SaveChangesAsync();
}