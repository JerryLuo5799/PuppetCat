using System;
using System.Collections.Generic;
using System.Text;

namespace PuppetCat.AspNetCore.Mvc
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message)
           : base(message)
        {

        }

        public UnauthorizedException(string message, Exception e)
          : base(message, e)
        {

        }
    }
}
