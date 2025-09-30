namespace SurveyManagement.Application.DTOS
{
    public class OptionUpdateDto
    {
        public Guid OptionId { get; set; }
        public string OptionValue { get; set; } = default!;
    }
}
