using System;
using System.Configuration;
using System.Web;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace UltimateTeam.Toolkit.Addin
{
    public interface IMailReader
    {
        string GetTwoFactorCode(string mailaddress, string password, string mailfrom, DateTime mailtime);
    }
}
