using System;

namespace CustomFramework.BaseWebApi.Data.Contracts
{
    public class Paging : IPaging
    {
        public Paging(int pageIndex, int pageSize)
        {
            if (pageIndex == 0) throw new ArgumentException("PageIndexCanNotBeZero");
            if (pageSize == 0) throw new ArgumentException("PageSizeCanNotBeZero");

            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int PageIndex { get; }
        public int PageSize { get; }
    }
}