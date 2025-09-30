namespace SurveyManagement.Domain.Entities
{
   
    public class Response
    {
        public Guid ResponseId { get; set; }

        public Guid UserSurveyId { get; set; }
        public UserSurvey UserSurvey { get; set; } = default!;

        public Guid QuestionId { get; set; }
        public Question Question { get; set; } = default!;
        public int? Rating { get; set; }
        public string? FeedbackText { get; set; } // for text answers

        public ICollection<ResponseOption> ResponseOptions { get; set; } = new List<ResponseOption>();

    }


}
