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
    public class OptionRepository : IOptionRepository
    {
        private readonly SurveyDbContext _context;

        public OptionRepository(SurveyDbContext context)
        {
            _context = context;
        }

        public async Task<Option?> GetByIdAsync(Guid id)
        {
            return await _context.Options.FindAsync(id);
        }

        public async Task<List<Option>> GetByQuestionIdAsync(Guid questionId)
        {
            return await _context.Options
                                 .Where(o => o.QuestionId == questionId)
                                 .OrderBy(o => o.Order)
                                 .ToListAsync();
        }

        public async Task AddAsync(Option option)
        {
            await _context.Options.AddAsync(option);
        }

        public async Task UpdateAsync(Option option)
        {
            _context.Options.Update(option);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
