using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PuppetCat.AspNetCore.Mvc.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                int resCode = 0;
                var msg = string.Empty;
                if (statusCode == 401)
                {
                    resCode = (int)ResponseStatusCode.Unauthorized;
                    msg = "Unauthorized";
                }
                else if (statusCode == 404)
                {
                    resCode = (int)ResponseStatusCode.NotFound;
                    msg = "The URI is not Found";
                }
                else if (statusCode == 502)
                {
                    resCode = (int)ResponseStatusCode.NotImplemented;
                    msg = "Not Implemented";
                }
                else if (statusCode != 200)
                {
                    resCode = (int)ResponseStatusCode.InternalServerError;
                    msg = "Internal Server Error - " + statusCode;
                }
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    await HandleExceptionResponseAsync(context, resCode, msg);
                }
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            int statusCode = context.Response.StatusCode == 404 ? (int)ResponseStatusCode.NotFound : (int)ResponseStatusCode.InternalServerError;
            string msg = ex.Message;

            if (ex is NotImplementedException)
            {
                statusCode = (int)ResponseStatusCode.NotImplemented;
            }
            else if (ex is BadRequestException)
            {
                statusCode = (int)ResponseStatusCode.BadRequest;
            }
            else if (ex is UnauthorizedException)
            {
                statusCode = (int)ResponseStatusCode.Unauthorized;
            }
            else if (ex is GatewayTimeoutException)
            {
                statusCode = (int)ResponseStatusCode.GatewayTimeout;
            }
            else if (ex is HttpVersionNotSupportedException)
            {
                statusCode = (int)ResponseStatusCode.HttpVersionNotSupported;
            }
            else if (ex is NotFoundException)
            {
                statusCode = (int)ResponseStatusCode.NotFound;
            }

            return HandleExceptionResponseAsync(context, statusCode, msg);
        }

        private static Task HandleExceptionResponseAsync(HttpContext context, int statusCode, string msg)
        {
            ResponseDefault<string> res = new ResponseDefault<string>() { result = statusCode, msg = msg };
            var result = JsonConvert.SerializeObject(res);
            context.Response.ContentType = "application/json;charset=utf-8";
            if (context.Request.Method == "POST")
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }

            return context.Response.WriteAsync(result);
        }
    }

}
