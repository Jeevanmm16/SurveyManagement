using SurveyManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyManagement.Infrastructure.Repository
{
   public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User?> GetByIdAsync(Guid id);

        Task<User> AddAsync(User user);
        Task<User?> UpdateAsync(User user);
        Task<User?> DeleteAsync(Guid id);
    }
}




