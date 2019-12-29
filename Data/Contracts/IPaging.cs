namespace CustomFramework.BaseWebApi.Data.Contracts
{
    public interface IPaging
    {
        int PageIndex { get; }
        int PageSize { get; }
    }
}