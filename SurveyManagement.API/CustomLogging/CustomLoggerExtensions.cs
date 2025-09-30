using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
public static class CustomLoggerExtensions

{
    public static ILoggingBuilder AddCustomLogger(this ILoggingBuilder builder)
    {
        builder.Services.AddSingleton<ILoggerProvider, CustomLoggerProvider>();
        return builder;
    }
}