namespace SurveyManagement.Domain.Entities
{
    public class Question
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; } = default!;
        public QuestionType QuestionType { get; set; }// MultipleChoice, Checkbox, Text, Rating
        public bool IsMandatory { get; set; }

        public Guid SurveyId { get; set; }
        public Survey Survey { get; set; } = default!;

        public ICollection<Option> Options { get; set; } = new List<Option>();
    }

}