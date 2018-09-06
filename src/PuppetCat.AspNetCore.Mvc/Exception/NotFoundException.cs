using System;
using System.Collections.Generic;
using System.Text;

namespace PuppetCat.AspNetCore.Mvc
{
    public class NotFoundException : Exception
    {

        public NotFoundException(string message)
           : base(message)
        {

        }

        public NotFoundException(string message, Exception e)
          : base(message, e)
        {

        }
    }
}
