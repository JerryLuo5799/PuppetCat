using System;
using System.Collections.Generic;
using System.Text;

namespace PuppetCat.AspNetCore.Mvc
{
    /// <summary>
    /// Base class about the Response,
    /// all the action has the same format response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseResponse<T>
    {
        /// <summary>
        /// requst result,true or false
        /// </summary>
        public int result { get; set; }
        /// <summary>
        /// error message info,only result=false than is not empty
        /// </summary>
        public string msg { get; set; } = string.Empty;

        /// <summary>
        /// Total item count, only be used when the requset is paging
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// Return data,The format of the different response is different
        /// </summary>
        public T data { get; set; }
    }
}
