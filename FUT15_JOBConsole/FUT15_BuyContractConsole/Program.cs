using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUT15_JOB;
using FUT15_JOB.Models;
using UltimateTeam.Toolkit.Models;
using UltimateTeam.Toolkit.Parameters;

namespace FUT15_BuyContractConsole
{
    class Program
    {
        //args: txtContractPrice.Text + "," + txtContractAmount.Text + "," + sessionid + "," + phishingtoken + "," +
        //      platform + "," + Common.JobUserAccountStatus.TradeUser.UserID
        static string sessionid;
        static string phishingtoken;
        static string platform;
        static string userid;
        static uint maxbuy;
        static int buyamount;
        static FUT15Configs fut15_config;
        static void Main(string[] args)
        {
            fut15_config = new FUT15Configs();
            fut15_config.FUT_Client_Facade = null;
            fut15_config.FUT_Client_Facade = new FUTClientFacade(fut15_config);
            maxbuy = Convert.ToUInt32(args[0]);
            buyamount = Convert.ToInt32(args[1]);
            sessionid = args[2];
            phishingtoken = args[3];
            platform = args[4];
            userid = args[5];
            Console.Title = "(Buying " + buyamount.ToString() + " Contracts at price " + maxbuy.ToString() + ")" + userid;
            Console.WriteLine("Buying started.");
            BuyContract();
            //Console.WriteLine("Buying done.");
            Console.ReadKey();
        }

        private static async void BuyContract()
        {
            await Login();

            var searchParameters = new PlayerSearchParameters
            {
                Page = 1,
                //PageSize = 36,
                ResourceId = ContractCard.ShinyGoldPlayerContract,
                //DevelopmentType = DevelopmentType.Contract
                //MaxBuy = maxbuy
            };

            //await Task.Delay(2000);
            var searchResponse = await fut15_config.FUT_Client_Facade.SearchAsync(searchParameters);
            foreach (var auctionInfo in searchResponse.AuctionInfo)
            {
                
            }
        }

        private static async Task Login()
        {
            if (platform == "xboxone")
            {
                fut15_config.FUT_Client_Facade.UpdateToken(sessionid, phishingtoken, Platform.Xbox360);
            }
            else
                fut15_config.FUT_Client_Facade.UpdateToken(sessionid, phishingtoken, Platform.Pc);

            Console.WriteLine("Login Successfully!");
        }
    }
}
