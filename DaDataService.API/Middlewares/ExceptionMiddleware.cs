using System.Net;

namespace DaDataService.API.Middlewares
{
    /// <summary>
    /// Middleware for handling exceptions in the application
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next; 
        private readonly ILogger<ExceptionMiddleware> _logger; 

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while processing the request");

                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; 

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error",
                Information = exception.Message 
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}