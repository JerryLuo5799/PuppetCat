using System;
using System.Collections.Generic;
using System.Text;

namespace PuppetCat.Sample.Core
{
    public class ConfigCore
    {
        public static AppSettingsModel AppSettings = new AppSettingsModel();

        public static void SetAppSettings(AppSettingsModel appSettings)
        {
            AppSettings = appSettings;
        }
    }

    public class AppSettingsModel
    {
        public string DistributeRoutePath { get; set; }
        public string DistributeRouteIgnorePath { get; set; }
    }
}
