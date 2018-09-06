using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace PuppetCat.AspNetCore.Mvc.Middleware
{
    /// <summary>
    /// distribute api request
    /// </summary>
    public class DistributeRoute : IRouter
    {
        private static List<string> _urls = new List<string>();
        private static List<string> _urlsIgnore = new List<string>();
        private readonly IRouter _mvcRoute;

        /// <summary>
        /// the paths which need to distribute request, comma Separate, if value=string.Empty don't distribute,
        /// </summary>
        public static string DistributeRoutePath { get; set; }

        /// <summary>
        /// the paths which ignore to distribute request, comma Separate
        /// </summary>
        public static string DistributeRouteIgnorePath { get; set; }

        public DistributeRoute(IServiceProvider services)
        {
            if (!string.IsNullOrEmpty(DistributeRoutePath))
            {
                string[] arrPath = DistributeRoutePath.Split(',');
                foreach (string path in arrPath)
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        _urls.Add(path.TrimEnd('/'));
                    }
                }
            }

            if (!string.IsNullOrEmpty(DistributeRouteIgnorePath))
            {
                string[] arrPath = DistributeRouteIgnorePath.Split(',');
                foreach (string path in arrPath)
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        _urlsIgnore.Add(path.TrimEnd('/'));
                    }
                }
            }

            _mvcRoute = services.GetRequiredService<MvcRouteHandler>();
        }

        public async Task RouteAsync(RouteContext context)
        {
            var requestedUrl = context.HttpContext.Request.Path.Value.TrimEnd('/');
            var currentPathHeader = requestedUrl.Trim('/');
            if (!string.IsNullOrEmpty(currentPathHeader))
            {
                string[] arrPath = currentPathHeader.Split('/');
                if (arrPath.Length > 0)
                {
                    currentPathHeader = arrPath[0];
                }
            }
            currentPathHeader = "/" + currentPathHeader;
            //only distribute post request
            if (_urls.Count > 0 && _urls.Contains(requestedUrl) && context.HttpContext.Request.Method.ToLower() == "post")
            {
                if (_urls.Contains(requestedUrl, StringComparer.OrdinalIgnoreCase))
                {
                    try
                    {
                        //Get Post Json
                        string postJson = string.Empty;
                        using (var sr = new StreamReader(context.HttpContext.Request.Body))
                        {
                            postJson = await sr.ReadToEndAsync();
                        }
                        //set Request.Body
                        context.HttpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(postJson));
                        //Find the Action via json
                        BaseRequest request = JsonConvert.DeserializeObject<BaseRequest>(postJson);
                        if (null != request && !string.IsNullOrEmpty(request.uri))
                        {
                            string uri = request.uri;
                            if (uri.IndexOf('.') > 0)
                            {
                                uri = uri.Replace(".", "/");
                            }
                            string[] arrRoute = request.uri.TrimStart('/').Split('/');
                            RouteData r = new RouteData();
                            context.RouteData.Values["controller"] = arrRoute[arrRoute.Length - 2];
                            context.RouteData.Values["action"] = arrRoute[arrRoute.Length - 1];
                            context.RouteData.Values["distributeUrl"] = requestedUrl;
                            await _mvcRoute.RouteAsync(context);
                        }
                        else
                        {
                            throw new BadRequestException("request format error, the field 'uri' is required");
                        }
                    }
                    catch (Exception e)
                    {
                        throw new BadRequestException("request format error", e);
                    }
                }
                else if (_urlsIgnore.Count == 0 || !_urlsIgnore.Contains(currentPathHeader, StringComparer.OrdinalIgnoreCase))
                {
                    throw new NotFoundException("The request URL is not found");
                }
            }
            else if (context.HttpContext.Request.Method.ToLower() == "post")
            {
                string[] arrRoute = requestedUrl.TrimStart('/').Split('/');
                context.RouteData.Values["controller"] = arrRoute[arrRoute.Length - 2];
                context.RouteData.Values["action"] = arrRoute[arrRoute.Length - 1];
                context.RouteData.Values["distributeUrl"] = requestedUrl;
                await _mvcRoute.RouteAsync(context);
            }


        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            if (context.Values.ContainsKey("distributeUrl"))
            {
                var url = context.Values["distributeUrl"] as string;
                if (_urls.Contains(url))
                {
                    return new VirtualPathData(this, url);
                }
            }
            return null;
        }
    }
}
