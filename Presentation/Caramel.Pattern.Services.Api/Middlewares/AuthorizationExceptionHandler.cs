using Caramel.Pattern.Services.Domain.Entities.DTOs.Responses;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Caramel.Pattern.Services.Api.Middlewares
{
    public class AuthorizationExceptionHandler
    {
        private readonly ILogger<AuthorizationExceptionHandler> _logger;
        private readonly RequestDelegate _next;

        public AuthorizationExceptionHandler(RequestDelegate next,
            ILogger<AuthorizationExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            var statusCode = (HttpStatusCode)context.Response.StatusCode;

            if (statusCode == HttpStatusCode.Forbidden ||
                statusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning(statusCode.GetDescription());

                var response = new ExceptionResponse(
                    StatusProcess.Unauthorized,
                    "Acesso negado."
                );

                context.Response.StatusCode = (int)statusCode;
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
