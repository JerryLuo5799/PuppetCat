using System;
using System.Collections.Generic;
using System.Text;

namespace PuppetCat.AspNetCore.Mvc
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message)
            : base(message)
        {

        }

        public BadRequestException(string message, Exception e)
          : base(message, e)
        {

        }
    }
}
