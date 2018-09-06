﻿using System;
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
        /// token, Required if is admin
        /// </summary>
        public string token { get; set; }

    }
}
