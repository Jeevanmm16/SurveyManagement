using SurveyManagement.API.CustomLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class CustomLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new CustomLogger(categoryName);
    }

    public void Dispose()
    {
        // clean up resources if needed
    }
}

