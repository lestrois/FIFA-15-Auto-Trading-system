using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;
using FUT15_JOB.Models;
using FUT15_Center;
using FUT15_Center.Models;
using UltimateTeam.Toolkit.Constants;
using UltimateTeam.Toolkit.Exceptions;
using UltimateTeam.Toolkit.Extensions;
using UltimateTeam.Toolkit.Models;
using UltimateTeam.Toolkit.Services;
using UltimateTeam.Toolkit;
using UltimateTeam.Toolkit.Parameters;

namespace FUT15_JOB
{
    public class Trade_JOB
    {
        DateTime JOB_StartTime = DateTime.Now;
        DateTime JOB_InstantTime = DateTime.Now;

        private List<TargetBidInfo> TB_DataCollect = new List<TargetBidInfo>();
        List<TargetBidInfo_Enhanced> TBE = new List<TargetBidInfo_Enhanced>();
        FUT15Configs fut15_config = new FUT15Configs();
        int iReloginTimes = 0;
        bool NeedReLogin = false;
        bool BidFromWatchList = false;
        Level PlayerLevel;
        public Trade_JOB(TradeUserAccount tu, JOBStatus currentjob, string level)
        {
            fut15_config.Center = new FUT15_Center.FUT15_Center(currentjob.JOBID, currentjob.JOBGroupNumber);
            //get platform's connection based on the userid
            fut15_config.Platform_DB = (new FUT15EntityFactory()).GetFUT15Entity(fut15_config.Center.Database.TradeUserAccounts.FirstOrDefault(a => a.UserID == tu.UserID).PlatformID);
            //Read Account's configuration from Center DB
            getLatestConfig(tu.AccountID);
            fut15_config.TradePileSize = 0;
            fut15_config.JobUserAccountStatus = new JobUserAccount(tu.UserID, currentjob.JOBID, fut15_config);
            fut15_config.SalePrices = new List<SalePrice>();
            fut15_config.logger = new Logger(tu.UserID);
            if(level.ToLower() == "gold")
                PlayerLevel = Level.Gold;
            if (level.ToLower() == "silver")
                PlayerLevel = Level.Silver;
            if (level.ToLower() == "brone")
                PlayerLevel = Level.Bronze;
        }
        public void getLatestConfig(int accountid)
        {
            if (fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "query_interval") == null)
            {
                fut15_config.Center.Database.InitiateTradeConfigData(accountid);
            }
            fut15_config.Center.Database.Dispose();
            fut15_config.Center.Database = new FUT15Entities_CenterDB();
            fut15_config.Query_Interval = Convert.ToInt32(fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "query_interval").Value);
            fut15_config.Bid_Interval = Convert.ToInt32(fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "bid_interval").Value);
            fut15_config.Relist_Interval_Mins = Convert.ToInt32(fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "relist_interval_mins").Value);
            fut15_config.Max_Singleplayers_Tobid = Convert.ToInt32(fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "max_singleplayers_tobid").Value);
            fut15_config.CoinsLimit = Convert.ToInt32(fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "coins_limit").Value);
            fut15_config.Expired_Hour = Convert.ToInt32(fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "expired_hour").Value);
            fut15_config.Expired_Seconds = fut15_config.Expired_Hour * 3600;
            fut15_config.Bid_In_Seconds = Convert.ToInt32(fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "bid_in_seconds").Value);
            fut15_config.Urgent_Bid_In_Seconds = Convert.ToInt32(fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "urgent_bid_in_seconds").Value);
            fut15_config.Low_Price_To_Avoid_Collect = Convert.ToInt32(fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "low_price_to_avoid_collect").Value);
            fut15_config.Log_Level = Convert.ToInt32(fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "Log_Level").Value);
            fut15_config.Enable_BIN = fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "enable_bin").Value.ToString();
            fut15_config.Enable_Login_App_Start = fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "enable_login_app_start").Value.ToString();
            fut15_config.PriceLimit = Convert.ToInt32(fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "PriceLimit").Value.ToString());
            fut15_config.StopAutoRelist = fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "StopAutoRelist").Value.ToString();
            fut15_config.AutoListLatestTargetPrice = fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "AutoListLatestTargetPrice").Value.ToString();
            fut15_config.DataCollectionWhileBidding = fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "DataCollectionWhileBidding").Value.ToString();
            fut15_config.ControlCommand = fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "ControlCommand").Value;
            fut15_config.RunningHours = Convert.ToInt32(fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "RunningHours").Value);
            fut15_config.SuspendMinutes = Convert.ToInt32(fut15_config.Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == "SuspendMinutes").Value);
        }
        public async Task<int> Start()
        {
            //Login
            try
            {
                fut15_config.FUT_Client_Facade = null;
                fut15_config.FUT_Client_Facade = new FUTClientFacade(fut15_config);

                //Get latest JOBID and try session log in
                var latestJob = fut15_config.Center.Database.AccountStatuses.OrderByDescending(a => a.LastModified).FirstOrDefault(a => a.AccountID == fut15_config.JobUserAccountStatus.TradeUser.AccountID);
                if (latestJob != null)
                {
                    fut15_config.MessageDisplay("JobID: " + latestJob.JobID + " AccountID:" + latestJob.AccountID + " SessionID:" + latestJob.SessionID + " PhishingToken:" + latestJob.PhishingToken);
                    if (fut15_config.JobUserAccountStatus.TradeUser.PlatformID == 4 || fut15_config.JobUserAccountStatus.TradeUser.PlatformID == 5)
                        fut15_config.JobUserAccountStatus.SessionLogin(latestJob.SessionID, latestJob.PhishingToken, Platform.Xbox360);
                    else
                        fut15_config.JobUserAccountStatus.SessionLogin(latestJob.SessionID, latestJob.PhishingToken, Platform.Pc);

                    fut15_config.MessageDisplay("Session Logged in");
                }
                else
                {
                    //if job id is not created ever, log in it.
                    Thread.Sleep(fut15_config.Bid_Interval);
                    ////await Task.Delay(fut15_config.Bid_Interval);
                    var loginResponse = await fut15_config.JobUserAccountStatus.Login();
                    fut15_config.MessageDisplay("Login Successfully!");
                }
            }
            catch (FutException e1)
            {
                fut15_config.MessageDisplay(e1.Message);
                fut15_config.JobUserAccountStatus.UpdateAccountStatus("Login failed.");
                return -1;
            }

            //Run Job BidLoop Or DataCollection
            while (fut15_config.ControlCommand != "exit") // user didn't press "exit" command
            {
                JOB_InstantTime = DateTime.Now;
                fut15_config.MessageDisplay("  JOB having been running for " + Convert.ToInt32((JOB_InstantTime - JOB_StartTime).TotalSeconds).ToString() + " seconds. (" + (fut15_config.RunningHours * 60 * 60).ToString() + " secs/cycle)", 4);

                if ((JOB_InstantTime - JOB_StartTime).TotalSeconds > (fut15_config.RunningHours * 60 * 60))
                {
                    fut15_config.MessageDisplay("  Working too hard. Sleep " + fut15_config.SuspendMinutes.ToString() + " minutes and be right back... (" + JOB_InstantTime.ToString() + ")");
                    fut15_config.JobUserAccountStatus.UpdateAccountStatus("Sleeping(" + fut15_config.SuspendMinutes.ToString() + "mins)");
                    Thread.Sleep(fut15_config.SuspendMinutes * 60 * 1000);
                    JOB_StartTime = DateTime.Now;
                }

                //if Coins is less than coin limit, break, to do datacollection
                if (NeedReLogin)
                {
                    while (NeedReLogin && iReloginTimes < 3)
                    {
                        //Got Expired Session Exception. Need Relogin.
                        //Try 3 times, if still failed, exit
                        //If Relogin Successfully, quit iReloginTimes = 0
                        try
                        {
                            var cookie = fut15_config.FUT_Client_Facade.fc.RequestFactories._cookieContainer;
                            fut15_config.FUT_Client_Facade = null;
                            fut15_config.FUT_Client_Facade = new FUTClientFacade(fut15_config);
                            fut15_config.FUT_Client_Facade.fc.RequestFactories._cookieContainer = cookie;
                            fut15_config.MessageDisplay("  Wait 10 seconds. Reloging in (" + (iReloginTimes + 1).ToString() + ")...");
                            Thread.Sleep(10000);
                            ////await Task.Delay(10000);
                            var loginResponse = await fut15_config.JobUserAccountStatus.Login();
                            fut15_config.MessageDisplay("  Relogin Successfully!");

                            NeedReLogin = false;
                            iReloginTimes = 0;
                            if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Relogged in successfully", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Relogged in successfully") == -1)
                                fut15_config.MessageDisplay("sending mail failed");
                            break;
                        }
                        catch (FutException e1)
                        {
                            fut15_config.MessageDisplay(e1.Message);
                            iReloginTimes++;
                            fut15_config.MessageDisplay("  Reloging in (" + (iReloginTimes + 1).ToString() + ") faled. Trying again...");
                        }
                    }
                    if (iReloginTimes == 3)
                    {
                        fut15_config.MessageDisplay("  Tryied Relogin 3 times and failed. Now Exit.");
                        return 1;
                    }
                }
                await CalculateAndDisplayFUTCoins();
                if (fut15_config.JobUserAccountStatus.TradeUser.Credit < fut15_config.CoinsLimit)
                {
                    //if (button1.Text == "Stop")
                    await DataCollectionLoop();
                }
                else
                {
                    //if (button1.Text == "Stop")
                    await BIDLoop();
                }

            }
            fut15_config.JobUserAccountStatus.UpdateAccountStatus("exit");
            fut15_config.MessageDisplay("Bid Job Ended.", 4);
            fut15_config.MessageDisplay(" ", 4);
            fut15_config.MessageDisplay(" ", 4);
            return 0;
        }

        private async Task CalculateAndDisplayFUTCoins()
        {
            await fut15_config.JobUserAccountStatus.GetCoins();
            fut15_config.MessageDisplay("Common.Acct.Credit:" + fut15_config.JobUserAccountStatus.TradeUser.Credit.ToString() + " Common.CoinsLimit:" + fut15_config.CoinsLimit.ToString(), 4);
        }

        private int Login(string userid, string password, string securityanswer)
        {
            return 0;
        }
        public int End()
        {
            return 0;
        }

        //1.Save and clear expired ones
        //2.Move old data to history table
        //3.Send successful bought auctionInfor to TransferList
        //4.Calculate new targetBid by running SP
        //5.Fill out new targetBid list to TBE
        //6.List new at targetSaleMin/Max
        private async Task LogisticsProcess()
        {
            fut15_config.MessageDisplay("  LogisticsProcess started", 4);

            //1.Save and clear expired ones
            //2.Send successful bought auctionInfor to TransferList
            //await SaveAndRemoveExpiredOnesInWatchList();
            //GetWatchList();

            //3.list auction
            //3.Move old data to history table -- TBD
            //4.Calculate new targetBid by running SP
            //5.Fill out new targetBid list to TBE
            //6.List new at targetSaleMin/Max

            await ListAuctions();

            //7.Relist All
            //Relist
            if (fut15_config.StopAutoRelist.ToLower() == "n")
            {
                fut15_config.MessageDisplay("Relisting Cards in Transferlist", 3);

                try
                {
                    Thread.Sleep(fut15_config.Bid_Interval);
                    //await Task.Delay(fut15_config.Bid_Interval);
                    await fut15_config.FUT_Client_Facade.ReListAsync();
                    fut15_config.MessageDisplay("Relisting Cards in Transferlist Done", 3);
                    //If there's more than 30 items in transfer list, list them one by one
                    await RelistOneByOne();
                }
                catch (Exception e)
                {
                    fut15_config.MessageDisplay(e.Message, 3);
                }
            }

            fut15_config.MessageDisplay("  LogisticsProcess Done", 4);


        }

        public async Task RelistOneByOne()
        {
            fut15_config.MessageDisplay("Relisting one by one started.", 3);
            var tlResponse = await fut15_config.FUT_Client_Facade.GetTradePileAsync();
            var tl = tlResponse.AuctionInfo;
            if (tl.Count > 30)
            {
                foreach (var t in tl)
                {
                    if (t.Expires == -1)
                    {
                        try
                        {
                            Thread.Sleep(fut15_config.Bid_Interval);
                            //await Task.Delay(fut15_config.Bid_Interval);
                            var auctionDetails = new AuctionDetails(t.ItemData.Id, AuctionDuration.OneHour, Convert.ToUInt32(t.StartingBid), Convert.ToUInt32(t.BuyNowPrice));
                            int i_result = fut15_config.Platform_DB.UpdateSaleTrackerStartingBid(t.ItemData.ResourceId, Convert.ToInt32(auctionDetails.StartingBid), Convert.ToInt32(auctionDetails.BuyNowPrice), fut15_config);
                            fut15_config.MessageDisplay(" Update sale tracker resource(" + t.ItemData.ResourceId.ToString() + ") starting(" + auctionDetails.StartingBid.ToString() + ")/buynow(" + auctionDetails.BuyNowPrice.ToString() + ") price. result(" + i_result.ToString() + ")", 3);
                            fut15_config.MessageDisplay(" Listing Auction ResourceID" + t.ItemData.ResourceId.ToString() + "  TargetSaleMin:" + t.StartingBid.ToString(), 3);
                            var listAuctionResponse = await fut15_config.FUT_Client_Facade.ListAuctionAsync(auctionDetails);
                        }
                        catch (Exception ee)
                        {
                            fut15_config.MessageDisplay(ee.Message);
                        }
                    }
                }
            }
            fut15_config.MessageDisplay("Relisting one by one done.", 3);
        }

        private async Task BIDLoop()
        {
            bool HadBid = false;
            fut15_config.MessageDisplay("BID Started.", 4);

            DateTime StartTime = DateTime.Now;
            DateTime InstantTime = DateTime.Now;
            uint nextLeague;
            uint currentLeague;
            currentLeague = League.BarclaysPremierLeague;
            nextLeague = League.LigaBbva;
            await LogisticsProcess();

            while (fut15_config.ControlCommand != "exit")
            {
                try
                {
                    JOB_InstantTime = DateTime.Now;
                    fut15_config.MessageDisplay("  JOB having been running for " + Convert.ToInt32((JOB_InstantTime - JOB_StartTime).TotalSeconds).ToString() + " seconds. (" + (fut15_config.RunningHours * 60 * 60).ToString() + " secs/cycle)", 4);

                    if ((JOB_InstantTime - JOB_StartTime).TotalSeconds > (fut15_config.RunningHours * 60 * 60))
                    {
                        fut15_config.MessageDisplay("  Working too hard. Sleep " + fut15_config.SuspendMinutes.ToString() + " minutes and be right back... (" + JOB_InstantTime.ToString() + ")");
                        fut15_config.JobUserAccountStatus.UpdateAccountStatus("Sleeping(" + fut15_config.SuspendMinutes.ToString() + "mins)");
                        Thread.Sleep(fut15_config.SuspendMinutes * 60 * 1000);
                        JOB_StartTime = DateTime.Now;
                    }

                    fut15_config.MessageDisplay("", 4);
                    fut15_config.MessageDisplay("--------------------------------------New cycle of BID Loop Starts--------------------------------------");
                    fut15_config.MessageDisplay("Refresh TargetBid List", 4);
                    fut15_config.Platform_DB.FillTargetBid(TBE, fut15_config);
                    fut15_config.MessageDisplay("Refresh TargetBid List Done", 4);
                    getLatestConfig(fut15_config.JobUserAccountStatus.TradeUser.AccountID);
                    fut15_config.MessageDisplay("Common.ControlCommand:" + fut15_config.ControlCommand, 4);

                    //In case coin is not enough do below in data collection loop
                    //1.Save and clear expired ones --  Done
                    //2.Move old data to history table
                    //3.Send successful bought auctionInfor to TransferList -- Done
                    //4.Calculate new targetBid by running SP   --  Done
                    //5.Fill out new targetBid list to TBE  --  Done
                    //6.List new at targetSaleMin/Max   --  Done
                    //if InstantTime - StartTime > 1 hour then LogisticsProcess() and reset StartTime
                    InstantTime = DateTime.Now;
                    fut15_config.MessageDisplay("  BID having been running for " + Convert.ToInt32((InstantTime - StartTime).TotalSeconds).ToString() + " seconds. (" + (fut15_config.Relist_Interval_Mins * 60).ToString() + " secs/cycle)", 4);

                    if ((InstantTime - StartTime).TotalSeconds > (fut15_config.Relist_Interval_Mins * 60))
                    {
                        await LogisticsProcess();
                        StartTime = DateTime.Now;
                    }

                    //New Strategy:
                    //Grab 30 records
                    if (currentLeague == League.BarclaysPremierLeague)
                    {
                        nextLeague = League.LigaBbva;
                    }
                    else if (currentLeague == League.LigaBbva)
                    {
                        nextLeague = League.Bundesliga;
                    }
                    else if (currentLeague == League.Bundesliga)
                    {
                        nextLeague = League.SerieA;
                    }
                    else if (currentLeague == League.SerieA)
                    {
                        nextLeague = League.Ligue1;
                    }
                    else if (currentLeague == League.Ligue1)
                    {
                        nextLeague = League.BarclaysPremierLeague;
                    }
                    currentLeague = nextLeague;


                    //Search n pages of players
                    HadBid = false;
                    fut15_config.MessageDisplay("Search next round of players of league " + currentLeague.ToString(), 3);
                    for (uint i_Page = 1; i_Page < 4; i_Page++)
                    {
                        var searchParameters = new PlayerSearchParameters
                        {
                            Page = i_Page,
                            PageSize = 48,
                            Level = PlayerLevel,
                            League = currentLeague
                        };

                        fut15_config.MessageDisplay("Search Page(" + i_Page.ToString() + ")", 3);
                        try
                        {
                            Thread.Sleep(fut15_config.Bid_Interval);
                            //await Task.Delay(fut15_config.Query_Interval);
                            var searchResponse = await fut15_config.FUT_Client_Facade.SearchAsync(searchParameters);
                            fut15_config.MessageDisplay("  " + searchResponse.AuctionInfo.Count.ToString() + " Players found", 3);
                            foreach (var auctionInfo in searchResponse.AuctionInfo)
                            {
                                //if auctionInfo is in the target bid player list
                                foreach (var TB in TBE)
                                {
                                    if (TB.ResourceId == auctionInfo.ItemData.ResourceId && TB.Position == auctionInfo.ItemData.PreferredPosition)
                                    {
                                        // Compare target players with proper price expired in proper time (1 min) and you are rich enough
                                        // if there is any good deal, bid it
                                        if (TB.ResourceId == auctionInfo.ItemData.ResourceId
                                            && TB.Biddable == true
                                            && auctionInfo.BidState != "highest"
                                            && auctionInfo.Expires <= fut15_config.Urgent_Bid_In_Seconds
                                            && auctionInfo.Expires > -1
                                            && auctionInfo.CurrentBid <= TB.targetBuyAt
                                            && auctionInfo.StartingBid <= TB.targetBuyAt
                                            && auctionInfo.CurrentBid <= fut15_config.PriceLimit
                                            && auctionInfo.StartingBid <= fut15_config.PriceLimit
                                            && fut15_config.JobUserAccountStatus.TradeUser.Credit > fut15_config.CoinsLimit
                                            && (TB.targetSaleMax > fut15_config.Low_Price_To_Avoid_Collect || TB.targetSaleMax == 0))
                                        {
                                            fut15_config.MessageDisplay("  Get into single Player Card Bid Mode ... ResourceID:" + TB.ResourceId.ToString() + " TargetPrice:" + TB.targetBuyAt.ToString(), 4);
                                            BidFromWatchList = false;
                                            var bResult = await QuickBid(TB.targetBuyAt, auctionInfo);
                                            HadBid = true;
                                            if (bResult == true)
                                            {
                                                fut15_config.MessageDisplay("  Bid successfully!", 4);
                                                //Save and clear expired ones
                                                //1. Get Player list from Watchlist
                                                //2. if it's untradable, save to DB, then remove it from watchlist.
                                                await SaveAndRemoveExpiredOnesInWatchList();
                                                
                                                //List Target Bid auctions
                                                await ListAuctions();


                                                //Relist
                                                if (fut15_config.StopAutoRelist.ToLower() == "n")
                                                {
                                                    fut15_config.MessageDisplay("Relisting Cards in Transferlist", 3);
                                                    Thread.Sleep(fut15_config.Bid_Interval);
                                                    //await Task.Delay(fut15_config.Bid_Interval);
                                                    await fut15_config.FUT_Client_Facade.ReListAsync();
                                                    fut15_config.MessageDisplay(" ", 3);
                                                    //If there's more than 30 items in transfer list, list them one by one
                                                    await RelistOneByOne();
                                                }
                                            }
                                            break;
                                        }

                                        // If it's bought, show a message
                                        if (TB.ResourceId == auctionInfo.ItemData.ResourceId
                                            && auctionInfo.Expires == -1
                                            && auctionInfo.BidState == "highest")
                                        {
                                            fut15_config.MessageDisplay("  Bought TradeId " + auctionInfo.TradeId + " at price of " + auctionInfo.CurrentBid.ToString(), 4);

                                        }

                                        // If it's outbid, show a message
                                        if (TB.ResourceId == auctionInfo.ItemData.ResourceId
                                            && auctionInfo.Expires == -1
                                            && auctionInfo.BidState != "highest")
                                        {
                                            fut15_config.MessageDisplay("  TradeId " + auctionInfo.TradeId + " is outbid at price of " + auctionInfo.CurrentBid.ToString(), 4);

                                        }

                                        //if checkBox1.value == true then collect data
                                        if (fut15_config.DataCollectionWhileBidding.ToLower() == "y" && (auctionInfo.CurrentBid > 0 || auctionInfo.CurrentBid == 0 && auctionInfo.StartingBid < TB.targetBuyAt))
                                        {
                                            //Add this aucinfor to watch list
                                            if (fut15_config.Platform_DB.AddWatchListQueue(auctionInfo, fut15_config) == 0 || fut15_config.Platform_DB.AddWatchListQueue(auctionInfo, fut15_config) == -1)
                                            {
                                                Thread.Sleep(fut15_config.Bid_Interval);
                                                //await Task.Delay(fut15_config.Bid_Interval);
                                                await fut15_config.FUT_Client_Facade.AddToWatchlistRequestAsync(auctionInfo);
                                                fut15_config.MessageDisplay(("  TradeId " + auctionInfo.TradeId + " ResourceID(" + auctionInfo.ItemData.ResourceId.ToString() + ") BidState:"
                                                                    + auctionInfo.BidState + " CurrentBid:" + auctionInfo.CurrentBid.ToString() + " StartBid:" + auctionInfo.StartingBid.ToString()
                                                                    + " Expires:" + auctionInfo.Expires.ToString() + " -- Expected Price:" + TB.targetBuyAt.ToString()), 3);
                                                fut15_config.MessageDisplay("  Player: " + auctionInfo.ItemData.ResourceId + " CurrentBid:" + auctionInfo.CurrentBid.ToString() + " added to watch list", 3);
                                            }
                                        }

                                    }
                                }
                                if (HadBid) break;
                            }
                        }
                        catch (Exception ee)
                        {
                            fut15_config.MessageDisplay(ee.Message);
                            if (ee is ExpiredSessionException)
                            {
                                if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                    fut15_config.MessageDisplay("sending mail failed");
                                NeedReLogin = true;
                                fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                                return;
                            }

                        }
                        if (HadBid) break;
                    }

                    if(fut15_config.DataCollectionWhileBidding.ToLower() == "y")
                        await SaveAndRemoveExpiredOnesInWatchList();


                    //Search if there is any biddable plaer in Watch list.
                    fut15_config.MessageDisplay("Search if there is any biddable player in Watch list.", 3);
                    try
                    {
                        Thread.Sleep(fut15_config.Bid_Interval);
                        //await Task.Delay(fut15_config.Bid_Interval);
                        var watchlistResponse = await fut15_config.FUT_Client_Facade.GetWatchlistAsync();
                        List<AuctionInfo> SortedList = watchlistResponse.AuctionInfo.OrderBy(o => o.Expires).ToList();
                       
                        foreach (var auctionInfo in SortedList)
                        {
                            try
                            {
                               
                                //check if biddable
                                foreach (var TB in TBE)
                                {
                                    if (TB.ResourceId == auctionInfo.ItemData.ResourceId && TB.Position == auctionInfo.ItemData.PreferredPosition)
                                    {
                                        fut15_config.MessageDisplay(("  TradeId " + auctionInfo.TradeId + " ResourceID(" + auctionInfo.ItemData.ResourceId.ToString() + ") BidState:"
                                                            + auctionInfo.BidState + " CurrentBid:" + auctionInfo.CurrentBid.ToString() + " StartBid:" + auctionInfo.StartingBid.ToString()
                                                            + " Expires:" + auctionInfo.Expires.ToString() + " -- Expected Price:" + TB.targetBuyAt.ToString()), 3);
                                        // Compare target players with proper price expired in proper time (1 min) and you are rich enough
                                        // if there is any good deal, bid it
                                        if (TB.ResourceId == auctionInfo.ItemData.ResourceId
                                            && TB.Biddable == true
                                            && auctionInfo.BidState != "highest"
                                            && auctionInfo.Expires <= fut15_config.Urgent_Bid_In_Seconds
                                            && auctionInfo.Expires > -1
                                            && auctionInfo.CurrentBid <= TB.targetBuyAt
                                            && fut15_config.JobUserAccountStatus.TradeUser.Credit > fut15_config.CoinsLimit
                                            && auctionInfo.StartingBid <= TB.targetBuyAt
                                            && (TB.targetSaleMax > fut15_config.Low_Price_To_Avoid_Collect || TB.targetSaleMax == 0))
                                        {
                                            fut15_config.MessageDisplay("  Get into single Player Card Bid Mode ... ResourceID:" + TB.ResourceId.ToString() + " TargetPrice:" + TB.targetBuyAt.ToString(), 4);
                                            BidFromWatchList = true;
                                            var bResult = await QuickBid(TB.targetBuyAt, auctionInfo);
                                            if (bResult == true)
                                            {
                                                fut15_config.MessageDisplay("  Bid successfully!", 4);


                                                //Save and clear expired ones
                                                //1. Get Player list from Watchlist
                                                //2. if it's untradable, save to DB, then remove it from watchlist.
                                                await SaveAndRemoveExpiredOnesInWatchList();
                                                
                                                //List Target Bid auctions
                                                await ListAuctions();

                                                //Relist
                                                if (fut15_config.StopAutoRelist.ToLower() == "n")
                                                {
                                                    fut15_config.MessageDisplay("Relisting Cards in Transferlist", 3);
                                                    try
                                                    {
                                                        Thread.Sleep(fut15_config.Bid_Interval);
                                                        //await Task.Delay(fut15_config.Bid_Interval);
                                                        await fut15_config.FUT_Client_Facade.ReListAsync();
                                                        fut15_config.MessageDisplay(" ", 3);
                                                        //If there's more than 30 items in transfer list, list them one by one
                                                        await RelistOneByOne();
                                                    }
                                                    catch (Exception ee)
                                                    {
                                                        fut15_config.MessageDisplay(ee.Message);
                                                        if (ee is ExpiredSessionException)
                                                        {
                                                            if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                                                fut15_config.MessageDisplay("sending mail failed");
                                                            NeedReLogin = true;
                                                            fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                                                            return;
                                                        }

                                                    }
                                                }

                                            }
                                            break;
                                        }


                                    }
                                }

                            }
                            catch (Exception EE)
                            {
                                fut15_config.MessageDisplay(EE.Message);
                                if (EE is ExpiredSessionException)
                                {
                                    if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                        fut15_config.MessageDisplay("sending mail failed");
                                    NeedReLogin = true;
                                    fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                                    return;
                                }
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        fut15_config.MessageDisplay(e1.Message);
                        if (e1 is ExpiredSessionException)
                        {
                            if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                fut15_config.MessageDisplay("sending mail failed");
                            fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                            NeedReLogin = true;
                            return;
                        }
                    }

                    //if Coins is less than coin limit, break, to do datacollection
                    await CalculateAndDisplayFUTCoins();
                    if (fut15_config.JobUserAccountStatus.TradeUser.Credit < fut15_config.CoinsLimit)
                    {
                        fut15_config.MessageDisplay("  Coins is less than limit. Quit Bid mode.", 4);
                        break;
                    }
                    fut15_config.MessageDisplay("--------------------------------------New cycle of BID Loop Ends--------------------------------------", 4);
                }
                catch (Exception exc)
                {
                    fut15_config.MessageDisplay("  Got Error:" + exc.Message);
                    if (exc is ExpiredSessionException)
                    {
                        if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                            fut15_config.MessageDisplay("sending mail failed");
                        NeedReLogin = true;
                        fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                        return;
                    }
                }
            }
        }

        private async Task ListAuctions()
        {
            List<AuctionInfo> SortedList = new List<AuctionInfo>();
            fut15_config.MessageDisplay("Refresh TargetBid List", 4);
            fut15_config.Platform_DB.FillTargetBid(TBE, fut15_config);
            fut15_config.MessageDisplay("Refresh TargetBid List Done", 4);

            fut15_config.MessageDisplay("  List Auctions at targetBid starts", 4);

            Thread.Sleep(fut15_config.Bid_Interval);
            //await Task.Delay(fut15_config.Bid_Interval);
            try
            {
                var watchlistResponse1 = await fut15_config.FUT_Client_Facade.GetTradePileAsync();
                SortedList = watchlistResponse1.AuctionInfo.OrderBy(o => o.Expires).ToList();
            }
            catch (Exception e)
            {
                fut15_config.MessageDisplay(e.Message);
                if (e is ExpiredSessionException)
                {
                    if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                        fut15_config.MessageDisplay("sending mail failed");
                    NeedReLogin = true;
                    fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                    return;
                }
            }

            fut15_config.AuctionValue = 0;

            foreach (var auctionInfo in SortedList)
            {
                try
                {
                    if (auctionInfo.Expires == -1 && auctionInfo.CurrentBid != 0)
                    {
                        try
                        {
                            fut15_config.SaveAuctionInfo(auctionInfo);
                            fut15_config.MessageDisplay("Trade " + auctionInfo.TradeId.ToString() + " Price (ResrouceID: " + auctionInfo.ItemData.ResourceId.ToString() + ") saved", 3);
                            int i_result = fut15_config.Platform_DB.UpdateSaleTracker(auctionInfo, fut15_config);
                            fut15_config.MessageDisplay("  ResourceId: " + auctionInfo.ItemData.ResourceId + " Price: " + auctionInfo.CurrentBid.ToString() + " status is updated in table SaleTracker(" + i_result.ToString() + ").", 3);
                            Thread.Sleep(fut15_config.Bid_Interval);
                            //await Task.Delay(fut15_config.Bid_Interval);
                            await fut15_config.FUT_Client_Facade.RemoveFromTradePileAsync(auctionInfo);
                            fut15_config.MessageDisplay("ResourceID(" + auctionInfo.ItemData.ResourceId.ToString() + ") sold at price " + auctionInfo.CurrentBid.ToString() + ". Now it's removed from Transfer List.", 3);
                        }
                        catch (Exception ee)
                        {
                            fut15_config.MessageDisplay(ee.Message);
                            if (ee is ExpiredSessionException)
                            {
                                if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                    fut15_config.MessageDisplay("sending mail failed");
                                NeedReLogin = true;
                                fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                                return;
                            }

                        }
                    }
                    if (auctionInfo.Expires == -1 && auctionInfo.CurrentBid == 0 && fut15_config.AutoListLatestTargetPrice.ToLower() == "y")
                    {
                        SalePrice _saleprice = new SalePrice(0, 0, 0);
                        try
                        {
                            _saleprice = fut15_config.SalePrices.FirstOrDefault(a => a.ResourceID == auctionInfo.ItemData.ResourceId);
                            if (_saleprice == null)
                            {
                                var _tbe2 = TBE.FirstOrDefault(a => a.ResourceId == auctionInfo.ItemData.ResourceId && a.Position == auctionInfo.ItemData.PreferredPosition);
                                _saleprice = new SalePrice(auctionInfo.ItemData.ResourceId, _tbe2.targetSaleMin, _tbe2.targetSaleMax);
                                fut15_config.SalePrices.Add(_saleprice);
                            }
                            if (_saleprice != null)
                            {
                                Thread.Sleep(fut15_config.Bid_Interval);
                                //await Task.Delay(fut15_config.Bid_Interval);
                                var auctionDetails = new AuctionDetails(auctionInfo.ItemData.Id, AuctionDuration.OneHour, Convert.ToUInt32(_saleprice.TargetSaleMin), Convert.ToUInt32(_saleprice.TargetSaleMax));
                                int i_result = fut15_config.Platform_DB.UpdateSaleTrackerStartingBid(auctionInfo.ItemData.ResourceId, Convert.ToInt32(auctionDetails.StartingBid), Convert.ToInt32(auctionDetails.BuyNowPrice), fut15_config);
                                fut15_config.MessageDisplay(" Update sale tracker resource(" + auctionInfo.ItemData.ResourceId.ToString() + ") starting(" + auctionDetails.StartingBid.ToString() + ")/buynow(" + auctionDetails.BuyNowPrice.ToString() + ") price. result(" + i_result.ToString() + ")", 3);
                                fut15_config.MessageDisplay(" Listing Auction ResourceID" + auctionInfo.ItemData.ResourceId.ToString() + "  TargetSaleMin:" + _saleprice.TargetSaleMin.ToString(), 3);
                                fut15_config.SalePrices.Remove(_saleprice);
                                var listAuctionResponse = await fut15_config.FUT_Client_Facade.ListAuctionAsync(auctionDetails);
                            }
                            else if (auctionInfo.ItemData.Rating < 80)
                            {
                                Thread.Sleep(fut15_config.Bid_Interval);
                                //await Task.Delay(fut15_config.Bid_Interval);
                                var auctionDetails = new AuctionDetails(auctionInfo.ItemData.Id);
                                int i_result = fut15_config.Platform_DB.UpdateSaleTrackerStartingBid(auctionInfo.ItemData.ResourceId, Convert.ToInt32(auctionDetails.StartingBid), Convert.ToInt32(auctionDetails.BuyNowPrice), fut15_config);
                                fut15_config.MessageDisplay(" Update sale tracker resource(" + auctionInfo.ItemData.ResourceId.ToString() + ") starting(" + auctionDetails.StartingBid.ToString() + ")/buynow(" + auctionDetails.BuyNowPrice.ToString() + ") price. result(" + i_result.ToString() + ")", 3);
                                fut15_config.MessageDisplay(" Listing Auction ResourceID" + auctionInfo.ItemData.ResourceId.ToString() + "  at 150.", 3);
                                var listAuctionResponse = await fut15_config.FUT_Client_Facade.ListAuctionAsync(auctionDetails);
                            }
                        }
                        catch (Exception EE)
                        {
                            if (EE is NotFoundException)
                            {
                                fut15_config.MessageDisplay(" List Auction ResourceID" + auctionInfo.ItemData.ResourceId.ToString() + "  TargetSaleMin:" + _saleprice.TargetSaleMin.ToString(), 3);
                                fut15_config.SalePrices.Remove(_saleprice);
                            }
                            if (EE is ExpiredSessionException)
                            {
                                if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                    fut15_config.MessageDisplay("sending mail failed");
                                NeedReLogin = true;
                                fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                                return;
                            }

                        }
                    }
                    if (auctionInfo.Expires == 0)
                    {
                        SalePrice _saleprice = new SalePrice(0, 0, 0);
                        try
                        {
                            _saleprice = fut15_config.SalePrices.FirstOrDefault(a => a.ResourceID == auctionInfo.ItemData.ResourceId);
                            if (_saleprice == null)
                            {
                                var _tbe2 = TBE.FirstOrDefault(a => a.ResourceId == auctionInfo.ItemData.ResourceId && a.Position == auctionInfo.ItemData.PreferredPosition);
                                if (_tbe2 != null)
                                {
                                    _saleprice = new SalePrice(auctionInfo.ItemData.ResourceId, _tbe2.targetSaleMin, _tbe2.targetSaleMax);
                                }
                                else
                                {
                                    _saleprice = new SalePrice(auctionInfo.ItemData.ResourceId, 150, 5000000);
                                }
                                fut15_config.SalePrices.Add(_saleprice);
                            }
                            fut15_config.AuctionValue = fut15_config.AuctionValue + Convert.ToInt32(_saleprice.TargetSaleMin);
                            Thread.Sleep(fut15_config.Bid_Interval);
                            //await Task.Delay(fut15_config.Bid_Interval);
                            var auctionDetails = new AuctionDetails(auctionInfo.ItemData.Id, AuctionDuration.OneHour, Convert.ToUInt32(_saleprice.TargetSaleMin), Convert.ToUInt32(_saleprice.TargetSaleMax));
                            int i_result = fut15_config.Platform_DB.UpdateSaleTrackerStartingBid(auctionInfo.ItemData.ResourceId, Convert.ToInt32(auctionDetails.StartingBid), Convert.ToInt32(auctionDetails.BuyNowPrice), fut15_config);
                            fut15_config.MessageDisplay(" Update sale tracker resource(" + auctionInfo.ItemData.ResourceId.ToString() + ") starting(" + auctionDetails.StartingBid.ToString() + ")/buynow(" + auctionDetails.BuyNowPrice.ToString() + ") price. result(" + i_result.ToString() + ")", 3);
                            fut15_config.MessageDisplay(" Listing Auction ResourceID" + auctionInfo.ItemData.ResourceId.ToString() + "  TargetSaleMin:" + _saleprice.TargetSaleMin.ToString(), 3);
                            fut15_config.SalePrices.Remove(_saleprice);
                            var listAuctionResponse = await fut15_config.FUT_Client_Facade.ListAuctionAsync(auctionDetails);
                        }
                        catch (Exception DE)
                        {
                            fut15_config.MessageDisplay(DE.Message);
                            if (DE is NotFoundException)
                            {
                                fut15_config.MessageDisplay(" List Auction ResourceID" + auctionInfo.ItemData.ResourceId.ToString() + "  TargetSaleMin:" + _saleprice.TargetSaleMin.ToString(), 3);
                                fut15_config.SalePrices.Remove(_saleprice);
                            }
                            if (DE is ExpiredSessionException)
                            {
                                if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                    fut15_config.MessageDisplay("sending mail failed");
                                NeedReLogin = true;
                                fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                                return;
                            }

                        }
                    }
                    if (auctionInfo.Expires != -1 || (auctionInfo.Expires == -1 && auctionInfo.CurrentBid == 0))
                        fut15_config.AuctionValue = fut15_config.AuctionValue + Convert.ToInt32((auctionInfo.CurrentBid == 0) ? auctionInfo.StartingBid : auctionInfo.CurrentBid);

                }
                catch (Exception e)
                {
                    fut15_config.MessageDisplay(e.Message);
                    if (e is ExpiredSessionException)
                    {
                        if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                            fut15_config.MessageDisplay("sending mail failed");
                        NeedReLogin = true;
                        fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                        return;
                    }

                }
            }
            fut15_config.AuctionValue = Convert.ToInt32(Convert.ToDouble(fut15_config.AuctionValue) * 0.95);
            fut15_config.JobUserAccountStatus.UpdateAccountCredits(fut15_config.AuctionValue, fut15_config.JobUserAccountStatus.TradeUser.Credit);
            fut15_config.MessageDisplay("  List Auctions at targetBid done", 4);
        }
        private async Task SaveAndRemoveExpiredOnesInWatchList()
        {
            try
            {
                fut15_config.MessageDisplay("Saving&Removing Expired ones Started", 4);
                //1. Get Player list from Watchlist
                //2. if it's untradable, save to DB, then remove it from watchlist.
                Thread.Sleep(fut15_config.Bid_Interval);
                //await Task.Delay(fut15_config.Bid_Interval);
                var watchlistResponse = await fut15_config.FUT_Client_Facade.GetWatchlistAsync();
                if (watchlistResponse != null)
                {
                    List<AuctionInfo> LAI = new List<AuctionInfo>();
                    foreach (var auctionInfo in watchlistResponse.AuctionInfo)
                    {
                        if (auctionInfo.Expires == -1)
                        {
                            if (auctionInfo.CurrentBid > 0)
                            {
                                fut15_config.SaveAuctionInfo(auctionInfo);
                                fut15_config.MessageDisplay("  Trade " + auctionInfo.TradeId.ToString() + " Price (ResrouceID: " + auctionInfo.ItemData.ResourceId.ToString() + ") saved", 3);
                            }

                            if (auctionInfo.BidState != "highest")
                            {
                                //Remove it from watch list;
                                LAI.Add(auctionInfo);
                                fut15_config.MessageDisplay("  Trade " + auctionInfo.TradeId.ToString() + " (ResrouceID: " + auctionInfo.ItemData.ResourceId.ToString() + ") removed from watchlist", 3);

                            }
                            else if (auctionInfo.BidState == "highest" && auctionInfo.ItemData.ItemState != "invalid")
                            {
                                try
                                {
                                    //Get TradePileSize
                                    Thread.Sleep(fut15_config.Bid_Interval);
                                    if (fut15_config.TradePileSize == 0)
                                    {
                                        var pilesize = await fut15_config.FUT_Client_Facade.GetPileSizeAsync();
                                        fut15_config.TradePileSize = pilesize.Entries.Count;
                                    }
                                    fut15_config.MessageDisplay("  TradePileMaxSize: " + fut15_config.TradePileSize.ToString(), 3);

                                    Thread.Sleep(fut15_config.Bid_Interval);
                                    var PlayerInTrading = await fut15_config.FUT_Client_Facade.GetTradePileAsync();
                                    var tlResponse = await fut15_config.FUT_Client_Facade.GetTradePileAsync();
                                    var tl = PlayerInTrading.AuctionInfo;
                                    int TradingPlayerCount = tl.Count;
                                    fut15_config.MessageDisplay("  Players in trade pile: " + TradingPlayerCount.ToString(), 3);
                                    if ((TradingPlayerCount < 30 && !fut15_config.JobUserAccountStatus.TradeUser.UserID.Equals("lestrois@gmail.com")) || (TradingPlayerCount < 70 && fut15_config.JobUserAccountStatus.TradeUser.UserID.Equals("lestrois@gmail.com")))
                                    {
                                        //Send to Transferlist
                                        var player = fut15_config.Platform_DB.Players.FirstOrDefault(p => p.resourceID == auctionInfo.ItemData.ResourceId);
                                        var tmp = fut15_config.SalePrices.FirstOrDefault(a => a.ResourceID == auctionInfo.ItemData.ResourceId);
                                        if (tmp == null)
                                        {
                                            fut15_config.SalePrices.Add(new SalePrice(auctionInfo.ItemData.ResourceId,
                                                                                TBE.FirstOrDefault(a => a.ResourceId == auctionInfo.ItemData.ResourceId && a.Position == auctionInfo.ItemData.PreferredPosition).targetSaleMin,
                                                                                TBE.FirstOrDefault(a => a.ResourceId == auctionInfo.ItemData.ResourceId && a.Position == auctionInfo.ItemData.PreferredPosition).targetSaleMax)
                                                                                );
                                            fut15_config.MessageDisplay("  Add SalePrice Resource:"
                                                                                + auctionInfo.ItemData.ResourceId.ToString() + " TargetSaleMin:"
                                                                                + TBE.FirstOrDefault(a => a.ResourceId == auctionInfo.ItemData.ResourceId && a.Position == auctionInfo.ItemData.PreferredPosition).targetSaleMin.ToString() + " TargetSaleMax:"
                                                                                + TBE.FirstOrDefault(a => a.ResourceId == auctionInfo.ItemData.ResourceId && a.Position == auctionInfo.ItemData.PreferredPosition).targetSaleMax, 3);
                                        }
                                        int i_result = fut15_config.Platform_DB.UpdateWatchListQueue(auctionInfo, "closed");
                                        int i_result2 = fut15_config.Platform_DB.AddSaleTracker(auctionInfo, fut15_config);
                                        fut15_config.MessageDisplay("  ResourceId: " + auctionInfo.ItemData.ResourceId + " Name: " + player.Name + " Price: " + auctionInfo.CurrentBid.ToString() + " status is updated in table WatchListQueue(" + i_result.ToString() + ") & SaleTracker(" + i_result2.ToString() + ").", 3);
                                        Thread.Sleep(fut15_config.Bid_Interval);
                                        //await Task.Delay(fut15_config.Bid_Interval);
                                        var sendToTradePileResponse = await fut15_config.FUT_Client_Facade.SendItemToTradePileAsync(auctionInfo.ItemData);
                                        fut15_config.MessageDisplay("  ResourceId: " + auctionInfo.ItemData.ResourceId + " Name: " + player.Name + " Price: " + auctionInfo.CurrentBid.ToString() + " is sent to transfer list", 3);
                                    }
                                }
                                catch (Exception fee)
                                {
                                    fut15_config.MessageDisplay("  " + fee.Message);
                                    if (fee is ExpiredSessionException)
                                    {
                                        if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                            fut15_config.MessageDisplay("sending mail failed");
                                        NeedReLogin = true;
                                        fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    if (LAI.Count != 0)
                    {
                        try
                        {
                            if (fut15_config.Platform_DB.UpdateWatchListQueue(LAI, "closed") != 0)
                                fut15_config.MessageDisplay("failed to update status 'closed' in table [WatchListQueues]");
                            Thread.Sleep(fut15_config.Bid_Interval);
                            //await Task.Delay(fut15_config.Bid_Interval);
                            await fut15_config.FUT_Client_Facade.RemoveFromWatchlistAsync(LAI);
                        }
                        catch (Exception ee)
                        {
                            fut15_config.MessageDisplay(ee.Message);
                            if (ee is ExpiredSessionException)
                            {
                                if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                    fut15_config.MessageDisplay("sending mail failed");
                                NeedReLogin = true;
                                fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                                return;
                            }
                        }
                    }

                }
                fut15_config.MessageDisplay("Saving&Removing Expired ones Done", 4);
            }
            catch (Exception fee)
            {
                fut15_config.MessageDisplay(fee.Message);
                if (fee is ExpiredSessionException)
                {
                    if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                        fut15_config.MessageDisplay("sending mail failed");
                    NeedReLogin = true;
                    fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                    return;
                }
            }

        }

        private async Task<bool> QuickBid(int targetBuyAt, AuctionInfo TargetAucInf)
        {
            uint nextLeague;
            uint currentLeague;
            currentLeague = League.BarclaysPremierLeague;
            nextLeague = League.LigaBbva;
            int _i = 0;
            bool foundIt = false;
            long LastTradeID = 0;
            //send to tradepile
            var player = fut15_config.Platform_DB.Players.FirstOrDefault(p => p.resourceID == TargetAucInf.ItemData.ResourceId);
            while (true)
            {
                try
                {
                    //if it's the first time coming at this point of this function, add player into watch list.
                    if (_i == 0 && !BidFromWatchList)
                    {
                        //add it to watch list
                        if (fut15_config.Platform_DB.AddWatchListQueue(TargetAucInf, fut15_config) == 0 || fut15_config.Platform_DB.AddWatchListQueue(TargetAucInf, fut15_config) == -1)
                        {
                            fut15_config.MessageDisplay(("  -TradeId " + TargetAucInf.TradeId + " ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") BidState:"
                                                + TargetAucInf.BidState + " CurrentBid:" + TargetAucInf.CurrentBid.ToString() + " StartBid:" + TargetAucInf.StartingBid.ToString()
                                                + " Expires:" + TargetAucInf.Expires.ToString()), 4);

                            Thread.Sleep(fut15_config.Bid_Interval);
                            //await Task.Delay(2500);
                            await fut15_config.FUT_Client_Facade.AddToWatchlistRequestAsync(TargetAucInf);
                            fut15_config.MessageDisplay("  -Player: " + TargetAucInf.ItemData.ResourceId + " Name:(" + player.Name + ") CurrentBid:" + TargetAucInf.CurrentBid.ToString() + " added to watch list", 4);
                        }
                    }
                    _i++;

                    //Query this Trade ID again to return to TargetAucInf
                    //1. Query Watchlist
                    Thread.Sleep(fut15_config.Bid_Interval);
                    //await Task.Delay(2500);
                    var watchlistResponse = await fut15_config.FUT_Client_Facade.GetWatchlistAsync();
                    var aucL = watchlistResponse.AuctionInfo;
                    if (aucL == null)
                    {
                        fut15_config.MessageDisplay("  -Bid has been expired or outbid(1). Quit Bid.", 4);
                        return false;
                    }
                    else
                    {
                        foundIt = false;
                        //Loop to find out the right Trade ID
                        List<AuctionInfo> SortedList = aucL.OrderBy(o => o.Expires).ToList();

                        //1. find out the most urgent biddable auction Information
                        foreach (var aucc in SortedList)
                        {
                            //if (aucc.TradeId == TargetAucInf.TradeId)
                            //{
                            //    TargetAucInf = aucc;
                            //    foundIt = true;
                            //    Common.AddLogToListItem("  New status: ResourceID:" + TargetAucInf.ItemData.ResourceId + " Name:(" + player.Name + ") BidState:" + TargetAucInf.BidState + " Expires:" + TargetAucInf.Expires + " CurrentBid:" + TargetAucInf.CurrentBid.ToString());

                            //    break;
                            //}
                            //Change to any the most urgent biddable auction Information
                            foreach (var _tbe in TBE)
                            {
                                if (_tbe.ResourceId == aucc.ItemData.ResourceId &&
                                    _tbe.Position == aucc.ItemData.PreferredPosition &&
                                   _tbe.targetBuyAt >= aucc.CurrentBid &&
                                   _tbe.targetBuyAt >= aucc.StartingBid &&
                                   _tbe.Biddable == true &&
                                   aucc.Expires != -1 &&
                                   aucc.Expires <= fut15_config.Urgent_Bid_In_Seconds &&
                                   aucc.BidState != "highest" &&
                                   TargetAucInf.CurrentBid <= fut15_config.PriceLimit)
                                {
                                    TargetAucInf = aucc;
                                    targetBuyAt = _tbe.targetBuyAt;
                                    foundIt = true;
                                    fut15_config.MessageDisplay("  -New status(biddable): ResourceID:" + TargetAucInf.ItemData.ResourceId + " BidState:" + TargetAucInf.BidState + " Expires:" + TargetAucInf.Expires + " CurrentBid:" + TargetAucInf.CurrentBid.ToString() + " TargetBuyAt:" + targetBuyAt.ToString(), 4);
                                    break;
                                }
                            }
                            if (foundIt) break;
                        }

                        //2. Find out highest kept auctionInformation
                        if (!foundIt)
                        {
                            foreach (var aucc in SortedList)
                            {
                                foreach (var _tbe in TBE)
                                {
                                    if (aucc.BidState == "highest" && aucc.Expires != -1 && _tbe.ResourceId == aucc.ItemData.ResourceId && _tbe.Position == aucc.ItemData.PreferredPosition)
                                    {
                                        TargetAucInf = aucc;
                                        targetBuyAt = _tbe.targetBuyAt;
                                        foundIt = true;
                                        fut15_config.MessageDisplay("  -New status(highest): ResourceID:" + TargetAucInf.ItemData.ResourceId + " BidState:" + TargetAucInf.BidState + " Expires:" + TargetAucInf.Expires + " CurrentBid:" + TargetAucInf.CurrentBid.ToString() + " TargetBuyAt:" + targetBuyAt.ToString(), 4);
                                        break;
                                    }
                                }
                                if (foundIt) break;
                            }
                        }

                        //3. Find out highest expired auctionInformation
                        if (!foundIt)
                        {
                            foreach (var aucc in SortedList)
                            {
                                if (aucc.BidState == "highest" && aucc.Expires == -1)
                                {
                                    TargetAucInf = aucc;
                                    foundIt = true;
                                    fut15_config.MessageDisplay("  -TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Bid successfully at price " + TargetAucInf.CurrentBid.ToString(), 4);
                                    return true;
                                }
                            }
                        }

                        if (!foundIt)
                        {
                            fut15_config.MessageDisplay("  -Bid has been expired or exceeded expected price. Quit Bid.", 4);
                            return false;
                        }
                    }

                    //If coins not enough quit bid
                    if (fut15_config.JobUserAccountStatus.TradeUser.Credit < TargetAucInf.CurrentBid || fut15_config.JobUserAccountStatus.TradeUser.Credit < TargetAucInf.StartingBid)
                    {
                        fut15_config.MessageDisplay("  -Coins not enough. Quit Bid.", 4);
                        //return false;
                    }

                    //5. If it's outbidstate != highest and expires != -1 and current bid < targetprice then bid it
                    if (TargetAucInf.BidState != "highest" && TargetAucInf.Expires != -1 && TargetAucInf.CurrentBid <= targetBuyAt && TargetAucInf.StartingBid <= targetBuyAt && TargetAucInf.CurrentBid <= fut15_config.PriceLimit)
                    {
                        fut15_config.MessageDisplay("  -(Watchlist)TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Bid an urgent deal at next price of " + TargetAucInf.CurrentBid.ToString() + " Expected Price:" + targetBuyAt.ToString(), 4);
                        Thread.Sleep(fut15_config.Bid_Interval);
                        //await Task.Delay(2500);
                        try
                        {
                            var auctionResponse = await fut15_config.FUT_Client_Facade.PlaceBidAsync(TargetAucInf);
                        }
                        catch (Exception EE)
                        {
                            if (EE is ExpiredSessionException)
                            {
                                if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                    fut15_config.MessageDisplay("sending mail failed");
                                fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                                throw EE;
                            }
                            if (EE is PermissionDeniedException)
                            {
                                fut15_config.MessageDisplay("  -(Watchlist)TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Outbid at this moment. try again next time.", 4);
                            }
                        }
                    }
                    if (TargetAucInf.CurrentBid > fut15_config.PriceLimit)
                    {
                        fut15_config.MessageDisplay("  -TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Price has been exceeded Price Limit.", 4);
                    }
                    if (TargetAucInf.CurrentBid > targetBuyAt)
                    {
                        fut15_config.MessageDisplay("  -TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Price has been exceeded expected.", 4);
                    }
                    //4. If it's outbidstate != highest and expires == -1 return false
                    if (TargetAucInf.BidState != "highest" && TargetAucInf.Expires == -1)
                    {
                        fut15_config.MessageDisplay("  -TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Bid has been expired(0).", 4);
                    }
                    //4. If it's outbidstate == highest and expires == -1, return true
                    if (TargetAucInf.BidState == "highest" && TargetAucInf.Expires == -1)
                    {
                        fut15_config.MessageDisplay("  -TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Bid successfully at price " + TargetAucInf.CurrentBid.ToString(), 4);
                    }
                    //6. If it's outbidstate == highest and expires != -1 Loop
                    if (TargetAucInf.BidState == "highest" && TargetAucInf.Expires != -1)
                    {
                        fut15_config.MessageDisplay("  -TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Keep at the highest price " + TargetAucInf.CurrentBid.ToString(), 4);
                    }

                    //If it's the same trade ID, then try once search+bid player, to avoid wasting time in bidding
                    if (TargetAucInf.TradeId == LastTradeID)
                    {
                        //Search next player list
                        if (currentLeague == League.BarclaysPremierLeague)
                        {
                            nextLeague = League.LigaBbva;
                        }
                        else if (currentLeague == League.LigaBbva)
                        {
                            nextLeague = League.Bundesliga;
                        }
                        else if (currentLeague == League.Bundesliga)
                        {
                            nextLeague = League.SerieA;
                        }
                        else if (currentLeague == League.SerieA)
                        {
                            nextLeague = League.Ligue1;
                        }
                        else if (currentLeague == League.Ligue1)
                        {
                            nextLeague = League.BarclaysPremierLeague;
                        }
                        currentLeague = nextLeague;
                        for (uint _i_page = 1; _i_page < 2; _i_page++)
                        {
                            var searchParameters = new PlayerSearchParameters
                            {
                                Page = _i_page,
                                PageSize = 48,
                                Level = PlayerLevel,
                                League = currentLeague
                            };

                            fut15_config.MessageDisplay("  -Page(" + _i_page.ToString() + ")Quick Search League " + currentLeague.ToString() + " when QuickBidding! Saving Time.", 4);
                            Thread.Sleep(fut15_config.Bid_Interval);
                            //await Task.Delay(fut15_config.Query_Interval);
                            var searchResponse = await fut15_config.FUT_Client_Facade.SearchAsync(searchParameters);
                            foreach (var auctionInfo in searchResponse.AuctionInfo)
                            {
                                foreach (var TB in TBE)
                                {
                                    if (TB.ResourceId == auctionInfo.ItemData.ResourceId
                                        && TB.Position == auctionInfo.ItemData.PreferredPosition
                                           && TB.Biddable == true
                                           && auctionInfo.BidState != "highest"
                                           && auctionInfo.Watched != true
                                           && auctionInfo.Expires <= fut15_config.Urgent_Bid_In_Seconds
                                           && auctionInfo.Expires > -1
                                           && auctionInfo.CurrentBid <= TB.targetBuyAt
                                           && fut15_config.JobUserAccountStatus.TradeUser.Credit > fut15_config.CoinsLimit
                                           && auctionInfo.StartingBid <= TB.targetBuyAt
                                           && (TB.targetSaleMax > fut15_config.Low_Price_To_Avoid_Collect || TB.targetSaleMax == 0))
                                    {
                                        TargetAucInf = auctionInfo;
                                        try
                                        {
                                            if (fut15_config.Platform_DB.AddWatchListQueue(TargetAucInf, fut15_config) == 0 || fut15_config.Platform_DB.AddWatchListQueue(TargetAucInf, fut15_config) == -1)
                                            {
                                                fut15_config.MessageDisplay(("  -(Search)TradeId " + TargetAucInf.TradeId + " ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") BidState:"
                                                                    + TargetAucInf.BidState + " CurrentBid:" + TargetAucInf.CurrentBid.ToString() + " StartBid:" + TargetAucInf.StartingBid.ToString()
                                                                    + " Expires:" + TargetAucInf.Expires.ToString()), 4);

                                                Thread.Sleep(fut15_config.Bid_Interval);
                                                //await Task.Delay(2500);
                                                await fut15_config.FUT_Client_Facade.AddToWatchlistRequestAsync(TargetAucInf);
                                                fut15_config.MessageDisplay("  -Player: " + TargetAucInf.ItemData.ResourceId + " Name:(" + player.Name + ") CurrentBid:" + TargetAucInf.CurrentBid.ToString() + " added to watch list", 4);

                                                fut15_config.MessageDisplay("  -(Search)TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Bid an urgent deal at next price of " + TargetAucInf.CurrentBid.ToString() + " Expected Price:" + TB.targetBuyAt.ToString(), 4);
                                                Thread.Sleep(fut15_config.Bid_Interval);
                                                //await Task.Delay(2500);
                                                var auctionResponse = await fut15_config.FUT_Client_Facade.PlaceBidAsync(TargetAucInf);
                                            }
                                        }
                                        catch (Exception EE)
                                        {
                                            if (EE is ExpiredSessionException)
                                            {
                                                if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                                    fut15_config.MessageDisplay("sending mail failed");
                                                fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                                                throw EE;
                                            }
                                            if (EE is PermissionDeniedException)
                                            {
                                                fut15_config.MessageDisplay("  -(Search)TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Outbid at this moment. Try again next time.", 4);
                                            }
                                        }

                                        break;
                                    }
                                }
                            }
                        }
                    }

                    LastTradeID = TargetAucInf.TradeId;
                }
                catch (Exception fee)
                {
                    fut15_config.MessageDisplay("  " + fee.Message);
                    if (fee is ExpiredSessionException)
                    {
                        if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                            fut15_config.MessageDisplay("sending mail failed");
                        fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                        throw fee;
                    }
                }
            }
        }

        private async Task SearchPlayer(uint TotalpageNumber, byte pageSize, Level level, uint league)
        {
            AuctionInfo ai = new AuctionInfo();
            bool NeedCollect;
            List<AuctionInfo> LAI = new List<AuctionInfo>();
            LAI.Clear();
            for (uint _i_Page = 1; _i_Page <= TotalpageNumber; _i_Page++)
            {
                try
                {
                    var searchParameters = new PlayerSearchParameters
                    {
                        Page = _i_Page,
                        PageSize = pageSize,
                        Level = level,
                        League = league
                    };
                    //1. Get Player list
                    //1.1 rating larger than 79
                    //1.2 current bid <> 0
                    //2. Add to Watch list
                    Thread.Sleep(fut15_config.Bid_Interval);
                    //await Task.Delay(fut15_config.Bid_Interval);
                    fut15_config.MessageDisplay("  Search League " + league.ToString() + "(page " + _i_Page.ToString() + " of " + TotalpageNumber.ToString() + ")", 3);
                    var searchResponse = await fut15_config.FUT_Client_Facade.SearchAsync(searchParameters);
                    fut15_config.MessageDisplay(searchResponse.AuctionInfo.Count + " Players found", 3);
                    foreach (var auctionInfo in searchResponse.AuctionInfo)
                    {
                        NeedCollect = true;
                        //If it's in Table TB and max price is too low (< "low_price_to_avoid_collect"), ignore it
                        foreach (var _tb in TB_DataCollect)
                        {
                            if (_tb.ResourceId == auctionInfo.ItemData.ResourceId && _tb.Position == auctionInfo.ItemData.PreferredPosition && _tb.targetSaleMax <= fut15_config.Low_Price_To_Avoid_Collect)
                            {
                                NeedCollect = false;
                            }
                        }

                        if (auctionInfo.ItemData.Rating > 79 && auctionInfo.CurrentBid != 0 && auctionInfo.Expires < 240 && auctionInfo.Watched != true && NeedCollect && auctionInfo.ItemData.RareFlag != 0)
                        {
                            LAI.Add(auctionInfo.Clone());
                        }
                    }
                }
                catch (Exception EE)
                {
                    fut15_config.MessageDisplay(EE.Message);
                    if (EE is ExpiredSessionException)
                    {
                        NeedReLogin = true;
                        fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                        throw EE;
                    }
                }
            }

            foreach (var _ai in LAI)
            {
                try
                {
                    Thread.Sleep(fut15_config.Bid_Interval);
                    //await Task.Delay(fut15_config.Bid_Interval);
                    await fut15_config.FUT_Client_Facade.AddToWatchlistRequestAsync(_ai);
                    fut15_config.MessageDisplay(("  TradeId " + _ai.TradeId + " ResourceID(" + _ai.ItemData.ResourceId.ToString() + ") BidState:"
                                        + _ai.BidState + " CurrentBid:" + _ai.CurrentBid.ToString() + " StartBid:" + _ai.StartingBid.ToString()
                                        + " Expires:" + _ai.Expires.ToString() + " added to watchlist"), 3);
                }
                catch (Exception EE)
                {
                    fut15_config.MessageDisplay(EE.Message);
                    if (EE is ExpiredSessionException)
                    {
                        NeedReLogin = true;
                        fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                        throw EE;
                    }
                }
            }
        }


        private async Task SearchSaveListDisplayCoins(int CycleNumber)
        {
            fut15_config.MessageDisplay(CycleNumber.ToString() + ".Query Barclays PL Players Started", 3);
            //1. Search England Leagues
            await SearchPlayer(5, 48, PlayerLevel, League.BarclaysPremierLeague);
            fut15_config.MessageDisplay(CycleNumber.ToString() + ".Query Barclays PL Players Done", 3);

            //2. Search Spain leagues
            fut15_config.MessageDisplay(CycleNumber.ToString() + ".Query Liga BBVA Players Started", 3);
            await SearchPlayer(5, 48, PlayerLevel, League.LigaBbva);
            fut15_config.MessageDisplay(CycleNumber.ToString() + ".Query Liga BBVA Players Done", 3);

            //3. Search Germany Leagues
            fut15_config.MessageDisplay(CycleNumber.ToString() + ".Query Bundesliga Players Started", 3);
            await SearchPlayer(5, 48, PlayerLevel, League.Bundesliga);
            fut15_config.MessageDisplay(CycleNumber.ToString() + ".Query Bundesliga Players Done", 3);

            //4. Search Italian Leagues
            fut15_config.MessageDisplay(CycleNumber.ToString() + ".Query Seria A Players Started", 3);
            await SearchPlayer(5, 48, PlayerLevel, League.SerieA);
            fut15_config.MessageDisplay(CycleNumber.ToString() + ".Query Seria A Players Done", 3);

            //5. Search France Leagues
            fut15_config.MessageDisplay(CycleNumber.ToString() + ".Query Ligue 1 Players Started", 3);
            await SearchPlayer(5, 48, PlayerLevel, League.Ligue1);
            fut15_config.MessageDisplay(CycleNumber.ToString() + ".Query Ligue 1 Players Done", 3);

            await SaveAndRemoveExpiredOnesInWatchList();
            await ListAuctions();

            //Relist
            if (fut15_config.StopAutoRelist.ToLower() == "n")
            {
                fut15_config.MessageDisplay("Relisting Cards in Transferlist", 3);
                Thread.Sleep(fut15_config.Bid_Interval);
                //await Task.Delay(fut15_config.Bid_Interval);
                await fut15_config.FUT_Client_Facade.ReListAsync();
                fut15_config.MessageDisplay(" ", 3);
                //If there's more than 30 items in transfer list, list them one by one
                await RelistOneByOne();
            }

            //Get Coins, if coins is less than coin limit break to jump to bid process.
            await CalculateAndDisplayFUTCoins();
        }


        //1.Save and clear expired ones -- Done
        //2.Move old data to history table
        //3.Send successful bought auctionInfor to TransferList --  Done
        //4.Calculate new targetBid by running SP "UpdateTargetBidInfor"    --  Done
        //5.Fill out new targetBid list to TBE  --  Done
        //6.List new at targetSaleMin/Max   --  Done
        //7.List All--Done
        private async Task DataCollectionLoop()
        {
            fut15_config.MessageDisplay("Data collection Started.", 4);

            int _cycle = 0;
            int milliseconds = Convert.ToInt32(fut15_config.Query_Interval);
            iReloginTimes = 0;
            while (fut15_config.ControlCommand != "exit")
            {
                try
                {
                    JOB_InstantTime = DateTime.Now;
                    fut15_config.MessageDisplay("  JOB having been running for " + Convert.ToInt32((JOB_InstantTime - JOB_StartTime).TotalSeconds).ToString() + " seconds. (" + (fut15_config.RunningHours * 60 * 60).ToString() + " secs/cycle)", 4);

                    if ((JOB_InstantTime - JOB_StartTime).TotalSeconds > (fut15_config.RunningHours * 60 * 60))
                    {
                        fut15_config.MessageDisplay("  Working too hard. Sleep " + fut15_config.SuspendMinutes.ToString() + " minutes and be right back... (" + JOB_InstantTime.ToString() + ")");
                        fut15_config.JobUserAccountStatus.UpdateAccountStatus("Sleeping(" + fut15_config.SuspendMinutes.ToString() + "mins)");
                        Thread.Sleep(fut15_config.SuspendMinutes * 60 * 1000);
                        JOB_StartTime = DateTime.Now;
                    }

                    getLatestConfig(fut15_config.JobUserAccountStatus.TradeUser.AccountID);
                    if (NeedReLogin)
                    {
                        while (NeedReLogin && iReloginTimes < 3)
                        {
                            //Got Expired Session Exception. Need Relogin.
                            //Try 3 times, if still failed, exit
                            //If Relogin Successfully, quit iReloginTimes = 0
                            try
                            {
                                var cookie = fut15_config.FUT_Client_Facade.fc.RequestFactories._cookieContainer;
                                fut15_config.FUT_Client_Facade = null;
                                fut15_config.FUT_Client_Facade = new FUTClientFacade(fut15_config);
                                fut15_config.FUT_Client_Facade.fc.RequestFactories._cookieContainer = cookie;
                                fut15_config.MessageDisplay("  Wait 10 seconds. Reloging in (" + (iReloginTimes + 1).ToString() + ")...");
                                Thread.Sleep(fut15_config.Bid_Interval);
                                //await Task.Delay(10000);
                                var loginResponse = await fut15_config.JobUserAccountStatus.Login();
                                fut15_config.MessageDisplay("  Relogin Successfully!");

                                NeedReLogin = false;
                                iReloginTimes = 0;
                                if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Relogged in successfully", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Relogged in successfully") == -1)
                                    fut15_config.MessageDisplay("sending mail failed");
                                break;
                            }
                            catch (FutException e1)
                            {
                                fut15_config.MessageDisplay(e1.Message);
                                iReloginTimes++;
                                fut15_config.MessageDisplay("  Reloging in (" + (iReloginTimes + 1).ToString() + ") faled. Trying again...");
                            }
                        }
                        if (iReloginTimes == 3)
                        {
                            fut15_config.MessageDisplay("  Tryied Relogin 3 times and failed. Now Exit.");
                            return;
                        }
                    }
                    //Fill TB_DataCollect
                    fut15_config.MessageDisplay("  Refresh TargetBid Collection List", 3);
                    fut15_config.Platform_DB.FillTargetBid_Collection(TB_DataCollect, fut15_config);
                    fut15_config.MessageDisplay("  Refresh TargetBid Collection List Done", 3);
                    _cycle++;
                    await SearchSaveListDisplayCoins(_cycle);

                    if (fut15_config.JobUserAccountStatus.TradeUser.Credit > fut15_config.CoinsLimit)
                    {
                        fut15_config.MessageDisplay("  Coins is more than limit. Got to bid.", 4);

                        break;
                    }
                }
                catch (Exception exc)
                {
                    fut15_config.MessageDisplay("  Got Error:" + exc.Message);
                    if (exc is ExpiredSessionException)
                    {
                        if (fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                            fut15_config.MessageDisplay("sending mail failed");
                        NeedReLogin = true;
                        fut15_config.JobUserAccountStatus.UpdateAccountStatus("expired");
                        return;
                    }

                }

            }

            fut15_config.MessageDisplay("Datacollection Done", 4);

        }
    }
}
