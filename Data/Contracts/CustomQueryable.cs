using System.Linq;

namespace CustomFramework.BaseWebApi.Data.Contracts
{
    public class CustomQueryable<T> : ICustomQueryable<T> where T : class
    {
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public IQueryable<T> Result { get; set; }
    }
}