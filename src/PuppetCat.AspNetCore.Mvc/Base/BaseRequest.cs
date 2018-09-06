using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PuppetCat.AspNetCore.Mvc
{
    /// <summary>
    /// Base Request
    /// </summary>
    public class BaseRequest
    {
        /// <summary>
        /// uri
        /// </summary>
        [Required]
        public string uri { get; set; }

        /// <summary>
        /// languages,cn=Chinese, en=English
        /// </summary>
        [Required]
        public string lang { get; set; }

        /// <summary>
        /// version
        /// </summary>
        [Required]
        public string version { get; set; }

        /// <summary>
        /// token, Required if is admin
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// pageIndex, be used when paging search
        /// </summary>
        public int pageIndex { get; set; }

        /// <summary>
        /// pageSize, be used when paging search
        /// </summary>
        public int pageSize { get; set; }
    }
}
