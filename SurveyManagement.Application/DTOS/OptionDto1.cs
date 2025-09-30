namespace SurveyManagement.Application.DTOS
{
    public class OptionDto1
    {
        public Guid OptionId { get; set; }
        public string OptionValue { get; set; } = default!;
        public int Order { get; set; }
        public Guid QuestionId { get; set; }
    }
}
