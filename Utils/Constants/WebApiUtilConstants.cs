using System;
using CustomFramework.BaseWebApi.Utils.Utils;

namespace CustomFramework.BaseWebApi.Utils.Constants
{
    public static class WebApiUtilConstants
    {
        public static readonly string DefaultRoute = ConfigHelper.GetConfigurationValue("DefaultRoute") ?? "api/";
        public static readonly string AdminRoute = ConfigHelper.GetConfigurationValue("AdminRoute") ?? DefaultRoute + "admin/";
        public static readonly int DefaultListCount = Convert.ToInt32(ConfigHelper.GetConfigurationValue("DefaultListCount") ?? "50");
    }
}