namespace SurveyManagement.Application.DTOS
{
    public class ResponseDto
    {
        public Guid ResponseId { get; set; }
        public Guid UserSurveyId { get; set; }
        public Guid QuestionId { get; set; }
        public string? FeedbackText { get; set; }
        public int? Rating { get; set; }
        public List<Guid>? OptionIds { get; set; } // only if options are selected
    }
}




