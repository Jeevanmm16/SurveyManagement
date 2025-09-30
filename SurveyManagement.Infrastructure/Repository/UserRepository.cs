using Microsoft.EntityFrameworkCore;
using SurveyManagement.Domain.Entities;
using SurveyManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyManagement.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SurveyDbContext _context;
        public UserRepository(SurveyDbContext context)
        {
            _context = context;
        }
        public async Task<User> AddAsync(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> DeleteAsync(Guid id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return null;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
           return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
               .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> UpdateAsync(User user)
        {
            if (!_context.Users.Any(u => u.Id == user.Id))
            {
                return null;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
