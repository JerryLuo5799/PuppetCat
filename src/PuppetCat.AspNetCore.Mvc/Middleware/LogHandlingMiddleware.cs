using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PuppetCat.AspNetCore.Mvc.Log;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PuppetCat.AspNetCore.Mvc.Middleware
{
    public class LogHandlingMiddleware
    {
        private readonly ILogger<LogHandlingMiddleware> _logger;


        private readonly RequestDelegate _next;

        public LogHandlingMiddleware(RequestDelegate next, ILogger<LogHandlingMiddleware> logger)
        {
            this._next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method.ToLower() == "post")
            {
                await LogRequest(context);
            }
            else
            {
                await _next(context);
            }


        }

        private async Task LogRequest(HttpContext context)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            RequestLogEntity requestLog = new RequestLogEntity();

            await FormatRequest(context.Request, requestLog);

            var originalBodyStream = context.Response.Body;

            //Create a new memory stream...
            using (var responseBody = new MemoryStream())
            {
                //...and use that for the temporary response body
                context.Response.Body = responseBody;

                //Continue down the Middleware pipeline, eventually returning to this class
                await _next(context);

                //Format the response from the server
                await FormatResponse(context.Response, requestLog, sw);

                await responseBody.CopyToAsync(originalBodyStream);
            }

        }

        private async Task FormatRequest(HttpRequest context, RequestLogEntity requestLog)
        {
            string bodyAsText = string.Empty;
            try
            {
                var body = context.Body;

                //This line allows us to set the reader for the request back at the beginning of its stream.
                context.EnableRewind();

                //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
                var buffer = new byte[Convert.ToInt32(context.ContentLength)];

                //...Then we copy the entire request stream into the new buffer.
                await context.Body.ReadAsync(buffer, 0, buffer.Length);

                //We convert the byte[] into a string using UTF8 encoding...
                bodyAsText = Encoding.UTF8.GetString(buffer);

                //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
                context.Body = body;

                BaseRequest request = JsonConvert.DeserializeObject<BaseRequest>(bodyAsText);
                requestLog.RequestId = request.requestId;
            }
            catch { }
            requestLog.Request = bodyAsText;

        }

        private async Task FormatResponse(HttpResponse response, RequestLogEntity requestLog, Stopwatch sw)
        {
            try
            {
                string text = string.Empty;

                //We need to read the response stream from the beginning...
                response.Body.Seek(0, SeekOrigin.Begin);

                //...and copy it into a string
                text = await new StreamReader(response.Body).ReadToEndAsync();

                //We need to reset the reader for the response so that the client can read it.
                response.Body.Seek(0, SeekOrigin.Begin);

                requestLog.Response = text;

                //BaseResponse<string> res = JsonConvert.DeserializeObject<BaseResponse<string>>(text);
                JObject obj = JObject.Parse(text);
                Object objResult = obj["result"];
                requestLog.ResponseCode = null == objResult ? string.Empty : objResult.ToString();
                sw.Stop();
                requestLog.ResponseTime = sw.ElapsedMilliseconds;
                _logger.LogInformation(JsonConvert.SerializeObject(requestLog));
            }
            catch { }
        }
    }
}