using CustomFramework.BaseWebApi.Authorization.Utils;
using CustomFramework.EmailProvider;

namespace CustomFramework.BaseWebApi.Identity.Extensions
{
    public class IdentityModel : IIdentityModel
    {
        public IdentityModel()
        {
            Token = new Token();
            EmailConfig = new EmailConfig();
        }

        public bool EmailConfirmationViaUrl { get; set; }
        public string SenderEmailAddress { get; set; }
        public string AppName { get; set; }
        public Token Token { get; set; }
        public EmailConfig EmailConfig { get; set; }
        public int GeneratedPasswordLength { get; set; }
    }
}