﻿using System.Net;

namespace DaDataService.API.Middlewares
{
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка во время обработки запроса");

                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; 

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Внутренняя ошибка сервера. Обратитесь в службу поддержки.",
                Detailed = exception.Message 
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}