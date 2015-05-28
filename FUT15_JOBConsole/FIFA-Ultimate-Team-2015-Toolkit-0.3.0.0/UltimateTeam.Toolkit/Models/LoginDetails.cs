using UltimateTeam.Toolkit.Extensions;
using UltimateTeam.Toolkit.Addin;

namespace UltimateTeam.Toolkit.Models
{
    public class LoginDetails
    {
        public string Username { get; private set; }
        
        public string Password { get; private set; }

        public string MailPassword { get; private set; }
        
        public string SecretAnswer { get; private set; }

        public Platform Platform { get; set; }

        public IMailReader MReader { get; set; }

        public LoginDetails(string username, string password, string secretAnswer, Platform platform, string mailPassword, IMailReader mr)
        {
            username.ThrowIfInvalidArgument();
            password.ThrowIfInvalidArgument();
            secretAnswer.ThrowIfInvalidArgument();
            Username = username;
            Password = password;
            SecretAnswer = secretAnswer;
            Platform = platform;
            MailPassword = mailPassword;
            MReader = mr;
        }
    }
}