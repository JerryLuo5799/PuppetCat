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
        public string requestId { get; set; } = Guid.NewGuid().ToString();


        /// <summary>
        /// uri
        /// </summary>
        [Required]
        public string uri { get; set; }


        /// <summary>
        /// token, Required if is admin
        /// </summary>
        public string token { get; set; }

    }
}
