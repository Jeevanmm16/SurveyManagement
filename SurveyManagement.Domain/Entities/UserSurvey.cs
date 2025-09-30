namespace SurveyManagement.Domain.Entities
{
    public class UserSurvey
    {
        public Guid UserSurveyId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        public Guid SurveyId { get; set; }
        public Survey Survey { get; set; } = default!;

        public ICollection<Response> Responses { get; set; } = new List<Response>();
    }

}