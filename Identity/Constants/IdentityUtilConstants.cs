using System;
using CustomFramework.BaseWebApi.Utils.Utils;

namespace CustomFramework.BaseWebApi.Identity.Constants
{
    public static class IdentityUtilConstants
    {
        public static readonly int IterationCountForHashing =
            Convert.ToInt32(ConfigHelper.GetConfigurationValue("IterationCountForHashing") ?? "5");

    }
}