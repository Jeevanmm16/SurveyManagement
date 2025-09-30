namespace SurveyManagement.Application.DTOS
{
    // For creating a new survey
    public class CreateSurveyDto
    {
        public string Title { get; set; } = default!;
        public Guid ProductId { get; set; }   // product is chosen while creating
    }
}
