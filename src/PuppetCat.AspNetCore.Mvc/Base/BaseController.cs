using Microsoft.AspNetCore.Mvc;
using PuppetCat.AspNetCore.Mvc.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace PuppetCat.AspNetCore.Mvc
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        protected Dictionary<string,string> _requestLog
        {
            get
            {
                RequestLogEntity log = HttpContext.Items["RequestLogEntity"] as RequestLogEntity;

                return (log ?? new RequestLogEntity()).ApiTrace;


           }
        }


        /// <summary>
        /// Create return json
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <param name="statusCode"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <param name="totalCount">记录条数,分页时使用</param>
        /// <returns></returns>
        protected JsonResult CreateResult<TData>(ResponseStatusCode statusCode, string msg="", TData data = null, int totalCount=0)
             where TData : class
        {
            ResponseDefault<TData> res = new ResponseDefault<TData>();
            res.result = (int)statusCode;
            res.msg = msg;
            res.count = totalCount;
            if (null != data)
            {
                res.data = data;
            }

            return Json(res);
        }
    }
}
