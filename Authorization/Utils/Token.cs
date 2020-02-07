namespace CustomFramework.BaseWebApi.Authorization.Utils
{
    public class Token : IToken
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int ExpireInMinutes { get; set; }
    }
}