namespace SurveyManagement.Application.DTOS
{
    // For updating survey (title & productId changeable)
    public class UpdateSurveyDto
    {
        public string Title { get; set; } = default!;
        public Guid ProductId { get; set; }
    }
}
