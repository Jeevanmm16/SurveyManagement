
namespace SurveyManagement.Application.DTOS
{
    public class OptionDto
    {
        public Guid OptionId { get; set; }
        public string OptionValue { get; set; } = default!;
    }
}
