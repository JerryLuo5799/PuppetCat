using System;
using System.Collections.Generic;
using System.Text;

namespace PuppetCat.AspNetCore.Mvc
{
    //public class RequestDefault : BaseRequest<string>
    //{

    //}

    public class RequestDefault<T> : BaseRequest
        where T : new()
    {
        /// <summary>
        /// request data,The format of the different request is different
        /// </summary>
        public T data { get; set; } = new T();
    }
}
