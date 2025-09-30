namespace SurveyManagement.Application.DTOS
{
    public class CreateResponseDto
    {
        public Guid UserSurveyId { get; set; }
        public Guid QuestionId { get; set; }
        public string? FeedbackText { get; set; }
        public int? Rating { get; set; }
        public List<Guid>? OptionIds { get; set; }
    }
}




