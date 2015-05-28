using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;

namespace FUT15_Center.Models
{
    public class TradeUserAccount
    {
        public int ID { get; set; }
        public int AccountID { get; set; }
        public int PlatformID { get; set; }
        public int PlatformUserID { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string SecurityAnswer { get; set; }
        public string LoginStatus { get; set; }
        public int JobGroupNumber { get; set; }
        public int Credit { get; set; }
        public string MailPassword { get; set; }

        public TradeUserAccount()
        {
        }
        public TradeUserAccount(string username, string password, string securityAnswer, string mailPassword)
        {
            UserID = username;
            Password = password;
            SecurityAnswer = securityAnswer;
            MailPassword = mailPassword;
        }
    }
}
