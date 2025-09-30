namespace SurveyManagement.Application.DTOS
{
    public class UpdateResponseDto
    {
        public string? FeedbackText { get; set; }
        public int? Rating { get; set; }
        public List<Guid>? OptionIds { get; set; }
    }
}




