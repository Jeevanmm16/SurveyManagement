using System.Text.Json;
namespace SurveyManagement.API.Middleware
{
    public partial class GlobalExceptionMiddleware

    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = ex.Message }));
            }
            catch (ValidationExceptionCustom ex)
            {
                _logger.LogWarning(ex, ex.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = ex.Message }));
            }
            catch (ConflictException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = ex.Message }));
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = ex.Message }));

            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = ex.Message }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = ex.Message }));
            }
        }
    }
}
