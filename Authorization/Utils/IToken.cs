namespace CustomFramework.BaseWebApi.Authorization.Utils
{
    public interface IToken
    {
        string Issuer { get; set; }
        string Audience { get; set; }
        string Key { get; set; }
        int ExpireInMinutes { get; set; }
    }
}