using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PuppetCat.AspNetCore.Core
{
    public class VersionUtils
    {
        public static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
