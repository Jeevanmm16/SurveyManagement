using SurveyManagement.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyManagement.Application.Services
{
    public interface IOptionService
    {
        Task<OptionDto1> CreateOptionAsync(OptionCreateDto dto);
        Task<OptionDto1> UpdateOptionAsync(OptionUpdateDto dto);
        Task<List<OptionDto1>> GetOptionsByQuestionIdAsync(Guid questionId);
    }
}
