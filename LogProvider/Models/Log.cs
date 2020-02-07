using System;
using CustomFramework.BaseWebApi.Data.Models;

namespace CustomFramework.BaseWebApi.LogProvider.Models
{
    public class Log : BaseModelNonUser<long>
    {
        public DateTime RequestTime { get; set; }
        public string RequestMethod { get; set; }
        public string RequestUrl { get; set; }
        public string RequestBody { get; set; }
        public DateTime ResponseTime { get; set; }
        public string ResponseBody { get; set; }
        public int? LoggedUserId { get; set; }
    }
}