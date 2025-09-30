namespace SurveyManagement.Application.DTOS
{
    // For returning in GetById (with questions)
    public class SurveyDetailDto
    {
        public Guid SurveyId { get; set; }
        public string Title { get; set; } = default!;
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }

        public ICollection<QuestionDto1> Questions { get; set; } = new List<QuestionDto1>();
    }
}
