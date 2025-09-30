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
    public class QuestionRepository : IQuestionRepository
    {
        private readonly SurveyDbContext _context;

        public QuestionRepository(SurveyDbContext context)
        {
            _context = context;
        }

        public async Task<Question> AddAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var question = await _context.Questions.Include(q => q.Options)
                                                   .FirstOrDefaultAsync(q => q.QuestionId == id);
            if (question == null) return false;

            _context.Options.RemoveRange(question.Options); // Cascade delete
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Question>> GetAllAsync()
        {
            return await _context.Questions.ToListAsync();
        }

        public async Task<Question?> GetByIdAsync(Guid id)
        {
            return await _context.Questions.Include(q => q.Options)
                                           .FirstOrDefaultAsync(q => q.QuestionId == id);
        }

        public async Task<Question?> UpdateAsync(Question question)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<bool> SurveyExistsAsync(Guid surveyId)
        {
            return await _context.Surveys.AnyAsync(s => s.SurveyId == surveyId);
        }
    }
}
