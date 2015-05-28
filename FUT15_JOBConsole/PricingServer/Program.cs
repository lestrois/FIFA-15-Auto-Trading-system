using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using FUT15_PricingServer.Models;

namespace FUT15_PricingServer
{
    class Program
    {
        static void Main(string[] args)
        {
            FUT15Entities_PC pcDB = new FUT15Entities_PC();
            FUT15Entities_XBoxOne x1DB = new FUT15Entities_XBoxOne();
            Logger logger = new Logger();

            while (true)
            {
                try
                {
                    Console.Title = "FUT15 Pricing Server";
                    Console.WriteLine(DateTime.Now.ToString() + " - Running PC DB SP ...");
                    pcDB.Database.ExecuteSqlCommand("UpdateTargetBidInfor");
                    Thread.Sleep(15000);

                    Console.WriteLine(DateTime.Now.ToString() + " - Running XBoxOne DB SP ...");
                    x1DB.Database.ExecuteSqlCommand("UpdateTargetBidInfor");
                    Thread.Sleep(15000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    logger.WriteLog(LogLevelL4N.ERROR, e.Message);
                }
            }
        }
    }

}
