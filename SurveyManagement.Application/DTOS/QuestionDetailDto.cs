
using SurveyManagement.Domain.Entities;

namespace SurveyManagement.Application.DTOS
{
    // For GetById
    public class QuestionDetailDto
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; } = default!;
        public QuestionType QuestionType { get; set; } = default!;
        public bool IsMandatory { get; set; }
        public Guid SurveyId { get; set; }
        public List<OptionDto> Options { get; set; } = new List<OptionDto>();
    }
}
