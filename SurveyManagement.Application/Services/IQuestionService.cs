using SurveyManagement.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyManagement.Application.Services
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionDto>> GetAllAsync();
        Task<QuestionDetailDto?> GetByIdAsync(Guid id);
        Task<QuestionDto?> CreateAsync(CreateQuestionDto dto);
        Task<QuestionDto?> UpdateAsync(Guid id, UpdateQuestionDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
