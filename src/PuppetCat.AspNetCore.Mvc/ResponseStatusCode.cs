using System;
using System.Collections.Generic;
using System.Text;

namespace PuppetCat.AspNetCore.Mvc
{
    public enum ResponseStatusCode
    {

        /// <summary>
        /// request succeeded and that the requested information is in the response
        /// </summary>
        OK = 8200,

        /// <summary>
        ///  message format error
        /// </summary>
        BadRequest = 8400,

        /// <summary>
        /// Unauthorized
        /// </summary>
        Unauthorized = 8401,
      
        /// <summary>
        /// can't found uri 
        /// </summary>
        NotFound = 8404,

        /// <summary>
        /// indicates that a generic error has occurred on the server.
        /// </summary>
        InternalServerError = 8500,

        /// <summary>
        /// fuction error
        /// </summary>
        NotImplemented = 8501,
       
        /// <summary>
        /// timeout
        /// </summary>
        GatewayTimeout = 8504,

        /// <summary>
        /// HttpVersionNotSupported
        /// </summary>
        HttpVersionNotSupported = 8505
    }
}
