namespace CustomFramework.BaseWebApi.Utils.Utils
{
    public interface IApiRequestAccessor
    {
        T GetApiRequest<T>();
    }
}