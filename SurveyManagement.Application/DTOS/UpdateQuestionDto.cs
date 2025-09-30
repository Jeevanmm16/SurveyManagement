
namespace SurveyManagement.Application.DTOS
{
    public class UpdateQuestionDto
    {
        public string QuestionText { get; set; } = default!;
     
        public bool IsMandatory { get; set; }
    }
}



