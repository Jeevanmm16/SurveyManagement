using Microsoft.EntityFrameworkCore;
using SurveyManagement.Domain.Entities;
using SurveyManagement.Infrastructure.Data;

namespace SurveyManagement.Infrastructure.Repository
{
    public class UserSurveyRepository : IUserSurveyRepository
    {
        private readonly SurveyDbContext _context;

        public UserSurveyRepository(SurveyDbContext context)
        {
            _context = context;
        }

        public async Task<UserSurvey> AddAsync(UserSurvey userSurvey)
        {
            await _context.UserSurveys.AddAsync(userSurvey);
            await _context.SaveChangesAsync();
            return userSurvey;
        }

        public async Task<UserSurvey?> GetByIdAsync(Guid id)
        {
            return await _context.UserSurveys
                 .Include(us => us.User)
                     .ThenInclude(u => u.Role)   // ensure Role is loaded
                 .Include(us => us.Survey)
                 .Include(us => us.Responses)
                 .FirstOrDefaultAsync(us => us.UserSurveyId == id);
        }

        public async Task<IEnumerable<UserSurvey>> GetAllAsync()
        {
            return await _context.UserSurveys
                .Include(us => us.User)
                .Include(us => us.Survey)
                .Include(us => us.Responses)
                .ToListAsync();
        }

        public async Task DeleteAsync(UserSurvey userSurvey)
        {
            _context.UserSurveys.Remove(userSurvey);
            await _context.SaveChangesAsync();
        }
    }
}
