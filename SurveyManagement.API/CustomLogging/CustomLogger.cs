using Microsoft.Extensions.Logging;
using System;
namespace SurveyManagement.API.CustomLogging
{
    

    public class CustomLogger : ILogger
    {
        private readonly string _categoryName;

        public CustomLogger(string categoryName)
        {
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true; // log all levels

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var message = formatter(state, exception);
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {_categoryName}: {message}";

            if (exception != null)
            {
                logEntry += Environment.NewLine + $"Exception: {exception}";
            }

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                case LogLevel.Information:
                    Console.ForegroundColor = ConsoleColor.Green; break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow; break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    Console.ForegroundColor = ConsoleColor.Red; break;
            }

            Console.WriteLine(logEntry);
            Console.ResetColor();
        }
    }

}
