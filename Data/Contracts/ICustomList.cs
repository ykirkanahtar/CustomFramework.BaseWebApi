using System.Collections.Generic;

namespace CustomFramework.BaseWebApi.Data.Contracts
{
    public interface ICustomList<T> where T : class
    {
        int TotalCount { get; set; }
        int PageIndex { get; set; }
        int PageSize { get; set; }
        int PageCount { get; set; }
        IList<T> Result { get; set; }
    }
}