using System;
using System.Collections.Generic;
using System.Text;

namespace PuppetCat.AspNetCore.Mvc
{
    public class HttpVersionNotSupportedException : Exception
    {

        public HttpVersionNotSupportedException(string message)
           : base(message)
        {

        }

        public HttpVersionNotSupportedException(string message, Exception e)
          : base(message, e)
        {

        }
    }
}
