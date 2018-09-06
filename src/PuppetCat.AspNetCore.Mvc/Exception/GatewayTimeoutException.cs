using System;
using System.Collections.Generic;
using System.Text;

namespace PuppetCat.AspNetCore.Mvc
{
    public class GatewayTimeoutException : Exception
    {

        public GatewayTimeoutException(string message)
           : base(message)
        {

        }

        public GatewayTimeoutException(string message, Exception e)
          : base(message, e)
        {

        }
    }
}
