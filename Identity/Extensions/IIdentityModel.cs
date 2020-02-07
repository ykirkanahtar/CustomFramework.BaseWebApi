using CustomFramework.BaseWebApi.Authorization.Utils;
using CustomFramework.EmailProvider;

namespace CustomFramework.BaseWebApi.Identity.Extensions
{
    public interface IIdentityModel
    {
        bool EmailConfirmationViaUrl { get; set; }
        string SenderEmailAddress { get; set; }
        string AppName { get; set; }
        Token Token { get; set; }
        EmailConfig EmailConfig { get; set; }
        int GeneratedPasswordLength { get; set; }
    }
}