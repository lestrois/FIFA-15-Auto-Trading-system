using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MailReader;
using System.Configuration;

namespace Test_Mailreader
{
    class Program
    {
        static void Main(string[] args)
        {
            MailReader.MailReader mr = new MailReader.MailReader();
            string code = string.Empty;
            int i_code;
            DateTime dt = DateTime.Now.AddMinutes(-5);

            while (!Int32.TryParse(code, out i_code))
            {
                code = mr.GetTwoFactorCode("lestrois@gmail.com", "XXXXXX", "noreply@em.ea.com", dt);
                Console.WriteLine(code);
                if (Int32.TryParse(code, out i_code)) break;
                Thread.Sleep(60000);
            }
            Console.ReadKey();
        }
    }
}
