using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using MailBee.ImapMail;
using AE;
using AE.Net;
using AE.Net.Mail;
using AE.Net.Mail.Imap;
using System.Text.RegularExpressions;
using UltimateTeam.Toolkit.Addin;

namespace MailReader
{
    public class MailReader : IMailReader
    {
        public string GetTwoFactorCode(string mailaddress, string password, string mailfrom, DateTime mailtime)
        {
            string code = string.Empty;
            if (mailaddress.ToLower().Contains("hotmail.com"))
                code = GetLatestLoginVerificationCode_Hotmail(mailaddress, password, mailfrom, mailtime);
            if (mailaddress.ToLower().Contains("gmail.com"))
                code = GetLatestLoginVerificationCode_Gmail(mailaddress, password, mailfrom, mailtime);
            return code;
        }
        private string GetLatestLoginVerificationCode_Gmail(string mailaddress, string password, string mailfrom, DateTime mailtime)
        {
            try
            {
                // Create a folder named "updates" under current directory

                // Connect to the IMAP server. The 'true' parameter specifies to use SSL
                // which is important (for Gmail at least)
                ImapClient ic = new ImapClient("imap.gmail.com", mailaddress, password, ImapClient.AuthMethods.Login, 993, true);
                // Select a mailbox. Case-insensitive
                ic.SelectMailbox("INBOX");
                int mailcount = ic.GetMessageCount();
                // Get the first *11* messages. 0 is the first message;
                // and it also includes the 10th message, which is really the eleventh ;)
                // MailMessage represents, well, a message in your mailbox
                MailMessage[] mm = ic.GetMessages(mailcount - 1, mailcount - 1, false);
                if (!mm[0].From.Address.Contains(mailfrom))
                {
                    ic.Dispose();
                    return "NotFromEA";
                }

                //if mail arrived time >= date param then go ahead -- "X-OriginalArrivalTime:"
                if (mm[0].Date < mailtime)
                {
                    ic.Dispose();
                    return "TooOldMail";
                }

                string code = Regex.Match(mm[0].Body, "<strong>\\d{6}</strong>").Value.Replace("<strong>", string.Empty).Replace("</strong>", string.Empty);
                //Console.WriteLine(code);

                // Probably wiser to use a using statement
                ic.Dispose();

                //string code = "111<strong>598574</strong>222";
                //string code2 = Regex.Match(code, "<strong>\\d{6}</strong>").ToString();
                //Console.WriteLine(code2);

                return code;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private string GetLatestLoginVerificationCode_Hotmail(string mailaddress, string password, string mailfrom, DateTime mailtime)
        {
            try
            {
                string str_result = string.Empty;
                string strTemp = string.Empty;
                int mailAmount = 1;
                bool bReadyToReadCode;
                // create an instance of TcpClient

                TcpClient tcpclient = new TcpClient();
                // HOST NAME POP SERVER and gmail uses port number 995 for POP
                tcpclient.Connect("pop3.live.com", 995);

                // This is Secure Stream // opened the connection between client and POP Server
                System.Net.Security.SslStream sslstream = new SslStream(tcpclient.GetStream());

                // authenticate as client
                sslstream.AuthenticateAsClient("pop3.live.com");

                //bool flag = sslstream.IsAuthenticated; // check flag
                // Asssigned the writer to stream
                System.IO.StreamWriter sw = new StreamWriter(sslstream);
                // Assigned reader to stream
                System.IO.StreamReader reader = new StreamReader(sslstream);
                // refer POP rfc command, there very few around 6-9 command
                sw.WriteLine("USER " + mailaddress);
                // sent to server

                sw.Flush();
                sw.WriteLine("PASS " + password);
                sw.Flush();

                //1. Get Max mail number
                sw.WriteLine("LIST");
                sw.Flush();
                bReadyToReadCode = false;
                while ((strTemp = reader.ReadLine()) != null)
                {
                    if (strTemp.IndexOf("+OK mailbox has ") != -1)
                    {
                        string mailamount = strTemp.Replace("+OK mailbox has ", "").Replace(" messages", "");
                        mailAmount = Convert.ToInt32(mailamount);
                        //break;
                    }
                    // find the . character in line
                    if (strTemp == ".")
                    {
                        break;
                    }

                    if (strTemp.IndexOf("-ERR") != -1)
                    {
                        str_result = strTemp;
                        break;
                    }
                }

                //reader = new StreamReader(sslstream);
                //2. Retrieve the last mail
                // It will read content of your first email.
                sw.WriteLine("RETR " + mailAmount.ToString());
                sw.Flush();
                while ((strTemp = reader.ReadLine()) != null)
                {
                    //if mail sent from EA/Origin(noreply@em.ea.com) then go ahead -- "X-SID-PRA:"
                    if (strTemp.IndexOf("X-SID-PRA:") != -1)
                    {
                        string from = strTemp.Replace("X-SID-PRA: ", "");
                        if (!from.Equals(mailfrom))
                        {
                            //str_result = -2;
                            str_result = "NotFromEA";
                            break;
                        }
                    }

                    //if mail arrived time >= date param then go ahead -- "X-OriginalArrivalTime:"
                    if (strTemp.IndexOf("X-OriginalArrivalTime:") != -1)
                    {
                        string mtime = strTemp.Replace("X-OriginalArrivalTime: ", "");
                        int i = mtime.IndexOf("(");
                        mtime = mtime.Substring(0, i);
                        DateTime dt = Convert.ToDateTime(mtime).AddHours(-6);

                        if (dt < mailtime)
                        {
                            str_result = "TooOldMail";
                            //str_result = -3;
                            break;
                        }
                    }

                    //Getting code
                    if (bReadyToReadCode && strTemp.IndexOf("<strong>") != -1)
                    {
                        string code = strTemp.Replace("<strong>", "").Replace("</strong>", "").Replace(" ", "").Replace("\t", "");
                        str_result = code;
                    }

                    //Getting code ready
                    if (strTemp.IndexOf("Your Origin Security Code") != -1)
                    {
                        bReadyToReadCode = true;
                    }

                    // find the . character in line
                    if (strTemp == ".")
                    {
                        break;
                    }

                    if (strTemp.IndexOf("-ERR") != -1)
                    {
                        str_result = strTemp;
                        break;
                    }
                }

                // close the connection
                sw.WriteLine("Quit ");
                sw.Flush();

                return str_result;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }


        }
    }
}
