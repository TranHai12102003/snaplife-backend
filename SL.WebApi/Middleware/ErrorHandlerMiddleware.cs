using SL.Domain.Common.Models;
using System.Net;
using System.Text.Json;

namespace SL.WebApi.Middleware
{
    public class ErrorHandlerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

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
                var errCode = HttpStatusCode.InternalServerError;
                switch (error)
                {
                    case AppException:
                        // custom application error
                        break;
                    case KeyNotFoundException:
                        // not found error
                        errCode = HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        break;
                }

                response.StatusCode = (int)errCode;
                var result = JsonSerializer.Serialize(new ErrorResponseResult(error.Message));
                await response.WriteAsync(result);
            }
        }
    }
}
