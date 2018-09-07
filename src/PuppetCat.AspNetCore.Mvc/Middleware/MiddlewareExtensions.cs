using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace PuppetCat.AspNetCore.Mvc.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }

        public static IApplicationBuilder UseLogHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogHandlingMiddleware>();
        }

    }
}
