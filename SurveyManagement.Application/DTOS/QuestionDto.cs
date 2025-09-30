
using SurveyManagement.Domain.Entities;

namespace SurveyManagement.Application.DTOS
{
    // For GetAll
    public class QuestionDto
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; } = default!;
        public QuestionType QuestionType { get; set; } = default!;
        public bool IsMandatory { get; set; }
        public Guid SurveyId { get; set; }
    }
}
