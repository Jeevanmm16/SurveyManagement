namespace SurveyManagement.API.Middleware
{
    public class ValidationExceptionCustom : Exception { public ValidationExceptionCustom(string message) : base(message) { } }

}