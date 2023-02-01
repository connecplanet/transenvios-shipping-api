using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace Transenvios.Shipping.Api.Infraestructure
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case AppException e: // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        error = e.GetBaseException();
                        break;
                    case KeyNotFoundException e: // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        error = e.GetBaseException();
                        break;
                    case DbUpdateException e: // Db Error
                        response.StatusCode = (int)HttpStatusCode.FailedDependency;
                        error = e.GetBaseException();
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        error = error.GetBaseException();
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
