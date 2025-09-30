using SurveyManagement.Domain.Entities;

public class ResponseOption
{
    public Guid ResponseId { get; set; }
    public Response Response { get; set; } = default!;

    public Guid OptionId { get; set; }

    public Option Option { get; set; } = default!;
}