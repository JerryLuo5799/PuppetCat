using System;
using System.Collections.Generic;
using System.Text;

namespace PuppetCat.AspNetCore.Mvc
{
    public class RequestPage<T> : BaseRequest
        where T : new()
    {
        /// <summary>
        /// pageIndex, be used when paging search
        /// </summary>
        public int pageIndex { get; set; }

        /// <summary>
        /// pageSize, be used when paging search
        /// </summary>
        public int pageSize { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        public T data { get; set; } = new T();
    }
}
