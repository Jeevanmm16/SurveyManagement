namespace SurveyManagement.API.Middleware
{
    public class ConflictException : Exception { public ConflictException(string message) : base(message) { } }
}
