using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using UltimateTeam.Toolkit.Constants;
using UltimateTeam.Toolkit.Exceptions;
using UltimateTeam.Toolkit.Extensions;
using UltimateTeam.Toolkit.Models;
using UltimateTeam.Toolkit.Services;
using UltimateTeam.Toolkit;
using UltimateTeam.Toolkit.Parameters;
using FUT15_Center;
using FUT15_Center.Models;
using FUT15_JOB;
using FUT15_JOB.Models;
using FUT_MonitorCenter.Modle;

using System.Configuration;
using System.Data.Entity;

namespace FUT_MonitorCenter
{
    public partial class frmJOB_AutoBid : Form
    {
        bool NeedReLogin = false;
        private List<TargetBidInfo> TB_DataCollect;
        //List<TargetBidInfo> _TargetBids;
        List<TargetBidInfo_Enhanced> TBE = new List<TargetBidInfo_Enhanced>();
        public static Player _Player1 = new Player();
        public static Player _Player2 = new Player();
        public static TargetBidInfo _TargetBid1 = new TargetBidInfo();
        public static TargetBidInfo _TargetBid2 = new TargetBidInfo();
        bool BidFromWatchList = false;

        public frmJOB_AutoBid()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            int i_test;
            if (button15.Text == "Stop" || button15.Enabled == false) return;
            if (txtCoinsLimit.Text != "" || !int.TryParse(txtCoinsLimit.Text, out i_test))
                Common.fut15_config.CoinsLimit = Convert.ToInt32(txtCoinsLimit.Text);

            if (button1.Text.Trim() == "BID IT!")
            {
                button1.Text = "Stop";
                Common.fut15_config.AddLogToListItem(lstLog1, "Bid Player Started.", 4);
                //Relist
                //Common.AddLogToListItem(lstLog1, "Relisting Cards in Transferlist");
                //await Task.Delay(Common.Bid_Interval);
                //await Common.FUT_Client_Facade.ReListAsync();
                //Common.AddLogToListItem(lstLog1, " ");

                while (button1.Text == "Stop")
                {
                    //if Coins is less than coin limit, break, to do datacollection
                    await CalculateAndDisplayFUTCoins();
                    if (Common.fut15_config.JobUserAccountStatus.TradeUser.Credit < Common.fut15_config.CoinsLimit)
                    {
                        if (button1.Text == "Stop")
                            await DataCollectionLoop();
                    }
                    else
                    {
                        if (button1.Text == "Stop")
                            await BIDLoop();
                    }

                }

                Common.fut15_config.AddLogToListItem(lstLog1, "Bid Job Ended.", 4);
                Common.fut15_config.AddLogToListItem(lstLog1, " ", 4);
                Common.fut15_config.AddLogToListItem(lstLog1, " ", 4);

                button1.Enabled = true;
            }
            else
            {
                button1.Text = "BID IT!";
                button1.Enabled = false;
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
                    await Task.Delay(Common.fut15_config.Bid_Interval);
                    Common.fut15_config.AddLogToListItem(lstLog1, "  Search League " + league.ToString() + "(page " + _i_Page.ToString() + " of " + TotalpageNumber.ToString() + ")", 3);
                    var searchResponse = await Common.fut15_config.FUT_Client_Facade.SearchAsync(searchParameters);
                    Common.fut15_config.AddLogToListItem(lstLog1, searchResponse.AuctionInfo.Count + " Players found", 3);
                    foreach (var auctionInfo in searchResponse.AuctionInfo)
                    {
                        NeedCollect = true;
                        //If it's in Table TB and max price is too low (< "low_price_to_avoid_collect"), ignore it
                        foreach (var _tb in TB_DataCollect)
                        {
                            if (_tb.ResourceId == auctionInfo.ItemData.ResourceId && _tb.targetSaleMax <= Common.fut15_config.Low_Price_To_Avoid_Collect)
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
                    Common.fut15_config.AddLogToListItem(lstLog1, EE.Message);
                }
            }

            foreach (var _ai in LAI)
            {
                try
                {
                    if (Common.fut15_config.Platform_DB.AddWatchListQueue(_ai, Common.fut15_config) == 0 || Common.fut15_config.Platform_DB.AddWatchListQueue(_ai, Common.fut15_config) == -1)
                    {
                        await Task.Delay(Common.fut15_config.Bid_Interval);
                        await Common.fut15_config.FUT_Client_Facade.AddToWatchlistRequestAsync(_ai);
                        if(cbAutoRefresh.Checked)
                            DisplayAuctionItem(dgPlayers, _ai);
                        Common.fut15_config.AddLogToListItem(lstLog1, ("  TradeId " + _ai.TradeId + " ResourceID(" + _ai.ItemData.ResourceId.ToString() + ") BidState:"
                                            + _ai.BidState + " CurrentBid:" + _ai.CurrentBid.ToString() + " StartBid:" + _ai.StartingBid.ToString()
                                            + " Expires:" + _ai.Expires.ToString() + " added to watchlist"), 3);
                    }
                }
                catch (Exception EE)
                {
                    Common.fut15_config.AddLogToListItem(lstLog1, EE.Message);
                }
            }
        }

        private async void frmJOB_AutoBid_Load(object sender, EventArgs e)
        {
            TBE = new List<TargetBidInfo_Enhanced>();
            TB_DataCollect = new List<TargetBidInfo>();

            await CalculateAndDisplayFUTCoins();

            txtCoinsLimit.Text = Common.fut15_config.CoinsLimit.ToString();
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if (Convert.ToInt32(comboBox1.GetItemText(comboBox1.Items[i])) == Common.fut15_config.Log_Level)
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < cbPriceLimit.Items.Count; i++)
            {
                if (Convert.ToInt32(cbPriceLimit.GetItemText(cbPriceLimit.Items[i])) == Common.fut15_config.PriceLimit)
                {
                    cbPriceLimit.SelectedIndex = i;
                    break;
                }
            }

            //Load Player list
            //var a = await LoadComboBox();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            GetWatchList();
        }

        //Send to club
        private void button3_Click(object sender, EventArgs e)
        {
            GetWatchList();
        }

        //Send to Transferlist
        private async void button4_Click(object sender, EventArgs e)
        {
            //get watchlist
            Common.fut15_config.AddLogToListItem(lstLog1, "Sending all successful bids to transfer list started..", 4);

            await Task.Delay(Common.fut15_config.Bid_Interval);
            var watchlistResponse = await Common.fut15_config.FUT_Client_Facade.GetWatchlistAsync();
            List<AuctionInfo> SortedList = watchlistResponse.AuctionInfo.OrderBy(o => o.Expires).ToList();
            foreach (var auctionInfo in SortedList)
            {
                if (auctionInfo.Expires == -1 && auctionInfo.BidState == "highest")
                {
                    try
                    {
                        //send to tradepile
                        var player = Common.fut15_config.Platform_DB.Players.FirstOrDefault(p => p.resourceID == auctionInfo.ItemData.ResourceId);
                        var tmp = Common.fut15_config.SalePrices.FirstOrDefault(a => a.ResourceID == auctionInfo.ItemData.ResourceId);
                        if (tmp == null)
                        {
                            Common.fut15_config.SalePrices.Add(new SalePrice(auctionInfo.ItemData.ResourceId,
                                                                TBE.FirstOrDefault(a => a.ResourceId == auctionInfo.ItemData.ResourceId).targetSaleMin,
                                                                TBE.FirstOrDefault(a => a.ResourceId == auctionInfo.ItemData.ResourceId).targetSaleMax)
                                                                );
                            Common.fut15_config.AddLogToListItem(lstLog1, "  Add SalePrice Resource:"
                                                                + auctionInfo.ItemData.ResourceId.ToString() + " TargetSaleMin:"
                                                                + TBE.FirstOrDefault(a => a.ResourceId == auctionInfo.ItemData.ResourceId).targetSaleMin.ToString() + " TargetSaleMax:"
                                                                + TBE.FirstOrDefault(a => a.ResourceId == auctionInfo.ItemData.ResourceId).targetSaleMax, 3);
                        }
                        int i_result = Common.fut15_config.Platform_DB.UpdateWatchListQueue(auctionInfo, "closed");
                        int i_result2 = Common.fut15_config.Platform_DB.AddSaleTracker(auctionInfo, Common.fut15_config);
                        Common.fut15_config.AddLogToListItem(lstLog1, "  ResourceId: " + auctionInfo.ItemData.ResourceId + " Name: " + player.Name + " Price: " + auctionInfo.CurrentBid.ToString() + " status is updated in table WatchListQueue(" + i_result.ToString() + ") & SaleTracker(" + i_result2.ToString() + ").", 3);
                        await Task.Delay(Common.fut15_config.Bid_Interval);
                        var sendToTradePileResponse = await Common.fut15_config.FUT_Client_Facade.SendItemToTradePileAsync(auctionInfo.ItemData);
                        Common.fut15_config.AddLogToListItem(lstLog1, "  ResourceId: " + auctionInfo.ItemData.ResourceId + " Name: " + player.Name + " Price: " + auctionInfo.CurrentBid.ToString() + " is sent to transfer list", 3);

                    }
                    catch (Exception fee)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  " + fee.Message);

                    }
                }
            }
            Common.fut15_config.AddLogToListItem(lstLog1, "Sending all successful bids to transfer list done", 4);
            Common.fut15_config.AddLogToListItem(lstLog1, " ", 4);


            GetWatchList();
        }

        private async void GetWatchList()
        {
            Common.fut15_config.AddLogToListItem(lstLog1, "Start retrieving watchlist", 4);

            await Common.fut15_config.RetrieveWatchlistGridview(dgPlayers);
            lblBidCoins.Text = Common.fut15_config.BID_Coins.ToString();
            Common.fut15_config.AddLogToListItem(lstLog1, "End retrieving watchlist", 4);

        }

        private async Task CalculateAndDisplayFUTCoins()
        {
            lblCoins.Text = "...";
            await Common.fut15_config.JobUserAccountStatus.GetCoins();
            Common.fut15_config.AddLogToListItem(lstLog1, "Common.JobUserAccountStatus.TradeUser.Credit:" + Common.fut15_config.JobUserAccountStatus.TradeUser.Credit.ToString() + " Common.CoinsLimit:" + Common.fut15_config.CoinsLimit.ToString(), 4);

            lblCoins.Text = Common.fut15_config.JobUserAccountStatus.TradeUser.Credit.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            lstLog1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {

        }

        private async void button7_Click(object sender, EventArgs e)
        {
            await SaveAndRemoveExpiredOnesInWatchList();
            GetWatchList();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Load target bid information
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Load target bid information
        }

        private void button8_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(Dialog_SelectPlayer))
                {
                    form.Activate();
                    return;
                }
            }
            Dialog_SelectPlayer childForm = new Dialog_SelectPlayer();
            //childForm.MdiParent = this;
            childForm.Text = "Select Player";
            childForm.ShowDialog();

            textBox1.Text = _Player1.Name;
            textBox2.Text = _Player1.resourceID.ToString();
            textBox3.Text = _TargetBid1.targetBuyAt.ToString();
            textBox16.Text = _TargetBid1.targetSaleMax.ToString();
            textBox13.Text = _TargetBid1.targetBuyAmount.ToString();

        }

        private void button9_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(Dialog_SelectPlayer))
                {
                    form.Activate();
                    return;
                }
            }
            Dialog_SelectPlayer childForm = new Dialog_SelectPlayer();
            //childForm.MdiParent = this;
            childForm.Text = "Select Player";
            childForm.ShowDialog();

            textBox6.Text = _Player1.Name;
            textBox5.Text = _Player1.resourceID.ToString();
            textBox4.Text = _TargetBid1.targetBuyAt.ToString();
            textBox15.Text = _TargetBid1.targetSaleMax.ToString();
            textBox14.Text = _TargetBid1.targetBuyAmount.ToString();
        }

        //1.Save and clear expired ones
        //2.Move old data to history table
        //3.Send successful bought auctionInfor to TransferList
        //4.Calculate new targetBid by running SP
        //5.Fill out new targetBid list to TBE
        //6.List new at targetSaleMin/Max
        private async Task LogisticsProcess()
        {
            Common.fut15_config.AddLogToListItem(lstLog1, "  LogisticsProcess started", 4);

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
            if (!chkStopAutoRelist.Checked)
            {
                Common.fut15_config.AddLogToListItem(lstLog1, "Relisting Cards in Transferlist", 3);

                await Task.Delay(Common.fut15_config.Bid_Interval);
                await Common.fut15_config.FUT_Client_Facade.ReListAsync();
                Common.fut15_config.AddLogToListItem(lstLog1, "Relisting Cards in Transferlist Done", 3);
            }


            Common.fut15_config.AddLogToListItem(lstLog1, "  LogisticsProcess Done", 4);


        }

        private async Task BIDLoop()
        {
            bool HadBid = false;
            int iReloginTimes = 0;
            Common.fut15_config.AddLogToListItem(lstLog1, "BID Started.", 4);

            DateTime StartTime = DateTime.Now;
            DateTime InstantTime = DateTime.Now;
            uint nextLeague;
            uint currentLeague;
            currentLeague = League.BarclaysPremierLeague;
            nextLeague = League.LigaBbva;

            while (button1.Text == "Stop")
            {
                try
                {
                    Common.fut15_config.AddLogToListItem(lstLog1, "", 4);
                    Common.fut15_config.AddLogToListItem(lstLog1, "--------------------------------------New cycle of BID Loop Starts--------------------------------------");
                    Common.fut15_config.AddLogToListItem(lstLog1, "Refresh TargetBid List", 4);
                    Common.fut15_config.Platform_DB.FillTargetBid(TBE, Common.fut15_config);
                    Common.fut15_config.AddLogToListItem(lstLog1, "Refresh TargetBid List Done", 4);
                    if (txtCoinsLimit.Text != "")
                        Common.fut15_config.CoinsLimit = Convert.ToInt32(txtCoinsLimit.Text);
                    if (NeedReLogin)
                    {
                        while (NeedReLogin && iReloginTimes < 3)
                        {
                            //Got Expired Session Exception. Need Relogin.
                            //Try 3 times, if still failed, exit
                            //If Relogin Successfully, quit iReloginTimes = 0
                            try
                            {
                                //Common.fut15_config.FUT_Client_Facade = null;
                                //Common.fut15_config.FUT_Client_Facade = new FUTClientFacade(Common.fut15_config);
                                Common.fut15_config.AddLogToListItem(lstLog1, "  Wait 10 seconds. Reloging in (" + (iReloginTimes + 1).ToString() + ")...");
                                await Task.Delay(10000);
                                var loginResponse = await Common.fut15_config.JobUserAccountStatus.Login();
                                Common.fut15_config.AddLogToListItem(lstLog1, "  Relogin Successfully!");

                                NeedReLogin = false;
                                iReloginTimes = 0;
                                if (Common.fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Relogged in successfully", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Relogged in successfully") == -1)
                                    Common.fut15_config.AddLogToListItem(lstLog1, "sending mail failed");
                                break;
                            }
                            catch (FutException e1)
                            {
                                Common.fut15_config.AddLogToListItem(lstLog1, e1.Message);
                                iReloginTimes++;
                                Common.fut15_config.AddLogToListItem(lstLog1, "  Reloging in (" + (iReloginTimes + 1).ToString() + ") faled. Trying again...");
                            }
                        }
                        if (iReloginTimes == 3)
                        {
                            Common.fut15_config.AddLogToListItem(lstLog1, "  Tryied Relogin 3 times and failed. Now Exit.");
                            return;
                        }
                    }

                    //In case coin is not enough do below in data collection loop
                    //1.Save and clear expired ones --  Done
                    //2.Move old data to history table
                    //3.Send successful bought auctionInfor to TransferList -- Done
                    //4.Calculate new targetBid by running SP   --  Done
                    //5.Fill out new targetBid list to TBE  --  Done
                    //6.List new at targetSaleMin/Max   --  Done
                    //if InstantTime - StartTime > 1 hour then LogisticsProcess() and reset StartTime
                    InstantTime = DateTime.Now;
                    Common.fut15_config.AddLogToListItem(lstLog1, "  BID having been running for " + Convert.ToInt32((InstantTime - StartTime).TotalSeconds).ToString() + " seconds. (" + (Common.fut15_config.Relist_Interval_Mins * 15).ToString()+ " secs/cycle)", 4);

                    if ((InstantTime - StartTime).TotalSeconds > (Common.fut15_config.Relist_Interval_Mins * 15))
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
                    Common.fut15_config.AddLogToListItem(lstLog1, "Search next round of players of league " + currentLeague.ToString(), 3);
                    for (uint i_Page = 1; i_Page < 4; i_Page++)
                    {
                        var searchParameters = new PlayerSearchParameters
                        {
                            Page = i_Page,
                            PageSize = 36,
                            Level = Level.Gold,
                            League = currentLeague
                        };

                        Common.fut15_config.AddLogToListItem(lstLog1, "Search Page(" + i_Page.ToString() + ")", 3);
                        try
                        {
                            await Task.Delay(Common.fut15_config.Query_Interval);
                            var searchResponse = await Common.fut15_config.FUT_Client_Facade.SearchAsync(searchParameters);
                            Common.fut15_config.AddLogToListItem(lstLog1, "  " + searchResponse.AuctionInfo.Count.ToString() + " Players found", 3);
                            foreach (var auctionInfo in searchResponse.AuctionInfo)
                            {
                                //if auctionInfo is in the target bid player list
                                foreach (var TB in TBE)
                                {
                                    if (TB.ResourceId == auctionInfo.ItemData.ResourceId)
                                    {
                                        // Compare target players with proper price expired in proper time (1 min) and you are rich enough
                                        // if there is any good deal, bid it
                                        if (TB.ResourceId == auctionInfo.ItemData.ResourceId
                                            && TB.Biddable == true
                                            && auctionInfo.BidState != "highest"
                                            && auctionInfo.Expires <= Common.fut15_config.Urgent_Bid_In_Seconds
                                            && auctionInfo.Expires > -1
                                            && auctionInfo.CurrentBid <= TB.targetBuyAt
                                            && auctionInfo.StartingBid <= TB.targetBuyAt
                                            && auctionInfo.CurrentBid <= Common.fut15_config.PriceLimit
                                            && auctionInfo.StartingBid <= Common.fut15_config.PriceLimit
                                            && Common.fut15_config.JobUserAccountStatus.TradeUser.Credit > Common.fut15_config.CoinsLimit
                                            && (TB.targetSaleMax > Common.fut15_config.Low_Price_To_Avoid_Collect || TB.targetSaleMax == 0))
                                        {
                                            Common.fut15_config.AddLogToListItem(lstLog1, "  Get into single Player Card Bid Mode ... ResourceID:" + TB.ResourceId.ToString() + " TargetPrice:" + TB.targetBuyAt.ToString(), 4);
                                            BidFromWatchList = false;
                                            var bResult = await QuickBid(TB.targetBuyAt, auctionInfo);
                                            HadBid = true;
                                            if (bResult == true)
                                            {
                                                Common.fut15_config.AddLogToListItem(lstLog1, "  Bid successfully!", 4);
                                                //Save and clear expired ones
                                                //1. Get Player list from Watchlist
                                                //2. if it's untradable, save to DB, then remove it from watchlist.
                                                await SaveAndRemoveExpiredOnesInWatchList();
                                                GetWatchList();
                                                //List Target Bid auctions
                                                await ListAuctions();


                                                //Relist
                                                if (!chkStopAutoRelist.Checked)
                                                {
                                                    Common.fut15_config.AddLogToListItem(lstLog1, "Relisting Cards in Transferlist", 3);
                                                    await Task.Delay(Common.fut15_config.Bid_Interval);
                                                    await Common.fut15_config.FUT_Client_Facade.ReListAsync();
                                                    Common.fut15_config.AddLogToListItem(lstLog1, " ", 3);
                                                }

                                            }
                                            break;
                                        }

                                        // If it's bought, show a message
                                        if (TB.ResourceId == auctionInfo.ItemData.ResourceId
                                            && auctionInfo.Expires == -1
                                            && auctionInfo.BidState == "highest")
                                        {
                                            Common.fut15_config.AddLogToListItem(lstLog1, "  Bought TradeId " + auctionInfo.TradeId + " at price of " + auctionInfo.CurrentBid.ToString(), 4);

                                        }

                                        // If it's outbid, show a message
                                        if (TB.ResourceId == auctionInfo.ItemData.ResourceId
                                            && auctionInfo.Expires == -1
                                            && auctionInfo.BidState != "highest")
                                        {
                                            Common.fut15_config.AddLogToListItem(lstLog1, "  TradeId " + auctionInfo.TradeId + " is outbid at price of " + auctionInfo.CurrentBid.ToString(), 4);

                                        }

                                        //if checkBox1.value == true then collect data
                                        if (checkBox1.Checked == true && (auctionInfo.CurrentBid > 0 || auctionInfo.CurrentBid == 0 && auctionInfo.StartingBid < TB.targetBuyAt))
                                        {
                                            //Add this aucinfor to watch list
                                            if (Common.fut15_config.Platform_DB.AddWatchListQueue(auctionInfo, Common.fut15_config) == 0 || Common.fut15_config.Platform_DB.AddWatchListQueue(auctionInfo, Common.fut15_config) == -1)
                                            {
                                                await Task.Delay(Common.fut15_config.Bid_Interval);
                                                await Common.fut15_config.FUT_Client_Facade.AddToWatchlistRequestAsync(auctionInfo);
                                                if (cbAutoRefresh.Checked)
                                                    DisplayAuctionItem(dgPlayers, auctionInfo);
                                                Common.fut15_config.AddLogToListItem(lstLog1, ("  TradeId " + auctionInfo.TradeId + " ResourceID(" + auctionInfo.ItemData.ResourceId.ToString() + ") BidState:"
                                                                    + auctionInfo.BidState + " CurrentBid:" + auctionInfo.CurrentBid.ToString() + " StartBid:" + auctionInfo.StartingBid.ToString()
                                                                    + " Expires:" + auctionInfo.Expires.ToString() + " -- Expected Price:" + TB.targetBuyAt.ToString()), 3);
                                                Common.fut15_config.AddLogToListItem(lstLog1, "  Player: " + auctionInfo.ItemData.ResourceId + " CurrentBid:" + auctionInfo.CurrentBid.ToString() + " added to watch list", 3);
                                            }
                                        }

                                    }
                                }
                                if (HadBid) break;
                            }
                        }
                        catch (Exception ee)
                        {
                            Common.fut15_config.AddLogToListItem(lstLog1, ee.Message);
                            if (ee is ExpiredSessionException)
                            {
                                if (Common.fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                    Common.fut15_config.AddLogToListItem(lstLog1, "sending mail failed");
                                NeedReLogin = true;
                                break;
                            }

                        }
                        if (HadBid) break;
                    }


                    if (checkBox1.Checked == true)
                    {
                        //Save and clear expired ones
                        //1. Get Player list from Watchlist
                        //2. if it's untradable, save to DB, then remove it from watchlist.
                        await SaveAndRemoveExpiredOnesInWatchList();
                    }


                    //Search if there is any biddable plaer in Watch list.
                    Common.fut15_config.AddLogToListItem(lstLog1, "Search if there is any biddable player in Watch list.", 3);
                    try
                    {
                        await Task.Delay(Common.fut15_config.Bid_Interval);
                        var watchlistResponse = await Common.fut15_config.FUT_Client_Facade.GetWatchlistAsync();
                        List<AuctionInfo> SortedList = watchlistResponse.AuctionInfo.OrderBy(o => o.Expires).ToList();
                        if (cbAutoRefresh.Checked)
                        {
                            dgPlayers.Rows.Clear();
                        }
                        foreach (var auctionInfo in SortedList)
                        {
                            try
                            {
                                if (cbAutoRefresh.Checked)
                                {
                                    DataGridViewRow row = (DataGridViewRow)dgPlayers.Rows[0].Clone();

                                    row.Cells[0].Value = auctionInfo.TradeId;
                                    row.Cells[1].Value = auctionInfo.ItemData.ResourceId;
                                    var player = Common.fut15_config.Platform_DB.Players.FirstOrDefault(a => a.resourceID == auctionInfo.ItemData.ResourceId);
                                    if (player != null)
                                    {
                                        row.Cells[2].Value = player.Name;
                                    }
                                    else
                                    {
                                        row.Cells[2].Value = "";
                                    }
                                    //await Task.Delay(Common.Bid_Interval);
                                    var imageBytes = await Common.fut15_config.FUT_Client_Facade.GetPlayerImageAsync(auctionInfo);
                                    row.Cells[3].Value = imageBytes;
                                    row.Cells[4].Value = auctionInfo.BidState;
                                    row.Cells[5].Value = auctionInfo.Expires;

                                    row.Cells[6].Value = auctionInfo.CurrentBid;
                                    row.Cells[7].Value = auctionInfo.StartingBid;
                                    row.Cells[8].Value = auctionInfo.BuyNowPrice;
                                    row.Cells[9].Value = auctionInfo.ItemData.PreferredPosition;
                                    row.Cells[10].Value = auctionInfo.ItemData.Rating;
                                    row.Cells[11].Value = auctionInfo.ItemData.RareFlag;
                                    row.Cells[12].Value = auctionInfo.ItemData.Contract;
                                    row.Cells[13].Value = auctionInfo.ItemData.Fitness;
                                    row.Height = 90;
                                    dgPlayers.Rows.Add(row);
                                }

                                //check if biddable
                                foreach (var TB in TBE)
                                {
                                    if (TB.ResourceId == auctionInfo.ItemData.ResourceId)
                                    {
                                        Common.fut15_config.AddLogToListItem(lstLog1, ("  TradeId " + auctionInfo.TradeId + " ResourceID(" + auctionInfo.ItemData.ResourceId.ToString() + ") BidState:"
                                                            + auctionInfo.BidState + " CurrentBid:" + auctionInfo.CurrentBid.ToString() + " StartBid:" + auctionInfo.StartingBid.ToString()
                                                            + " Expires:" + auctionInfo.Expires.ToString() + " -- Expected Price:" + TB.targetBuyAt.ToString()), 3);
                                        // Compare target players with proper price expired in proper time (1 min) and you are rich enough
                                        // if there is any good deal, bid it
                                        if (TB.ResourceId == auctionInfo.ItemData.ResourceId
                                            && TB.Biddable == true
                                            && auctionInfo.BidState != "highest"
                                            && auctionInfo.Expires <= Common.fut15_config.Urgent_Bid_In_Seconds
                                            && auctionInfo.Expires > -1
                                            && auctionInfo.CurrentBid <= TB.targetBuyAt
                                            && Common.fut15_config.JobUserAccountStatus.TradeUser.Credit > Common.fut15_config.CoinsLimit
                                            && auctionInfo.StartingBid <= TB.targetBuyAt
                                            && (TB.targetSaleMax > Common.fut15_config.Low_Price_To_Avoid_Collect || TB.targetSaleMax == 0))
                                        {
                                            Common.fut15_config.AddLogToListItem(lstLog1, "  Get into single Player Card Bid Mode ... ResourceID:" + TB.ResourceId.ToString() + " TargetPrice:" + TB.targetBuyAt.ToString(), 4);
                                            BidFromWatchList = true;
                                            var bResult = await QuickBid(TB.targetBuyAt, auctionInfo);
                                            if (bResult == true)
                                            {
                                                Common.fut15_config.AddLogToListItem(lstLog1, "  Bid successfully!", 4);


                                                //Save and clear expired ones
                                                //1. Get Player list from Watchlist
                                                //2. if it's untradable, save to DB, then remove it from watchlist.
                                                await SaveAndRemoveExpiredOnesInWatchList();
                                                GetWatchList();
                                                //List Target Bid auctions
                                                await ListAuctions();

                                                //Relist
                                                if (!chkStopAutoRelist.Checked)
                                                {
                                                    Common.fut15_config.AddLogToListItem(lstLog1, "Relisting Cards in Transferlist", 3);
                                                    try
                                                    {
                                                        await Task.Delay(Common.fut15_config.Bid_Interval);
                                                        await Common.fut15_config.FUT_Client_Facade.ReListAsync();
                                                        Common.fut15_config.AddLogToListItem(lstLog1, " ", 3);
                                                    }
                                                    catch (Exception ee)
                                                    {
                                                        Common.fut15_config.AddLogToListItem(lstLog1, ee.Message);
                                                        if (ee is ExpiredSessionException)
                                                        {
                                                            if (Common.fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                                                Common.fut15_config.AddLogToListItem(lstLog1, "sending mail failed");
                                                            NeedReLogin = true;
                                                            break;
                                                        }

                                                    }
                                                }

                                            }
                                            break;
                                        }

                                        // If it's bought, show a message
                                        //if (TB.ResourceId == auctionInfo.ItemData.ResourceId
                                        //    && auctionInfo.Expires == -1
                                        //    && auctionInfo.BidState == "highest")
                                        //{
                                        //    Common.AddLogToListItem(lstLog1, "  Bought TradeId " + auctionInfo.TradeId + " at price of " + auctionInfo.CurrentBid.ToString());

                                        //}

                                        //// If it's outbid, show a message
                                        //if (TB.ResourceId == auctionInfo.ItemData.ResourceId
                                        //    && auctionInfo.Expires == -1
                                        //    && auctionInfo.BidState != "highest")
                                        //{
                                        //    Common.AddLogToListItem(lstLog1, "  TradeId " + auctionInfo.TradeId + " is outbid at price of " + auctionInfo.CurrentBid.ToString());

                                        //}

                                    }
                                }

                            }
                            catch (Exception EE)
                            {
                                Common.fut15_config.AddLogToListItem(lstLog1, EE.Message);
                                if (EE is ExpiredSessionException)
                                {
                                    if (Common.fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                        Common.fut15_config.AddLogToListItem(lstLog1, "sending mail failed");
                                    NeedReLogin = true;
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, e1.Message);
                        if (e1 is ExpiredSessionException)
                        {
                            if (Common.fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                Common.fut15_config.AddLogToListItem(lstLog1, "sending mail failed");
                            NeedReLogin = true;
                        }
                    }

                    //if Coins is less than coin limit, break, to do datacollection
                    await CalculateAndDisplayFUTCoins();
                    if (Common.fut15_config.JobUserAccountStatus.TradeUser.Credit < Common.fut15_config.CoinsLimit)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  Coins is less than limit. Quit Bid mode.", 4);
                        break;
                    }
                    Common.fut15_config.AddLogToListItem(lstLog1, "--------------------------------------New cycle of BID Loop Ends--------------------------------------", 4);
                }
                catch (Exception exc)
                {
                    Common.fut15_config.AddLogToListItem(lstLog1, "  Got Error:" + exc.Message);
                    if (exc is ExpiredSessionException)
                    {
                        if (Common.fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                            Common.fut15_config.AddLogToListItem(lstLog1, "sending mail failed");
                        NeedReLogin = true;
                    }
                }
            }
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
            Common.fut15_config.AddLogToListItem(lstLog1, "Data collection Started.", 4);


            int _count = 0;
            int _cycle = 0;
            int milliseconds = Convert.ToInt32(Common.fut15_config.Query_Interval);
            int iReloginTimes = 0;
            while (button1.Text == "Stop" && _count < 51)
            {
                try
                {
                    if (NeedReLogin)
                    {
                        while (NeedReLogin && iReloginTimes < 3)
                        {
                            //Got Expired Session Exception. Need Relogin.
                            //Try 3 times, if still failed, exit
                            //If Relogin Successfully, quit iReloginTimes = 0
                            try
                            {
                                //Common.fut15_config.FUT_Client_Facade = null;
                                //Common.fut15_config.FUT_Client_Facade = new FUTClientFacade(Common.fut15_config);
                                Common.fut15_config.AddLogToListItem(lstLog1, "  Wait 10 seconds. Reloging in (" + (iReloginTimes + 1).ToString() + ")...");
                                await Task.Delay(10000);
                                var loginResponse = await Common.fut15_config.JobUserAccountStatus.Login();
                                Common.fut15_config.AddLogToListItem(lstLog1, "  Relogin Successfully!");

                                NeedReLogin = false;
                                iReloginTimes = 0;
                                if (Common.fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Relogged in successfully", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Relogged in successfully") == -1)
                                    Common.fut15_config.AddLogToListItem(lstLog1, "sending mail failed");
                                break;
                            }
                            catch (FutException e1)
                            {
                                Common.fut15_config.AddLogToListItem(lstLog1, e1.Message);
                                iReloginTimes++;
                                Common.fut15_config.AddLogToListItem(lstLog1, "  Reloging in (" + (iReloginTimes + 1).ToString() + ") faled. Trying again...");
                            }
                        }
                        if (iReloginTimes == 3)
                        {
                            Common.fut15_config.AddLogToListItem(lstLog1, "  Tryied Relogin 3 times and failed. Now Exit.");
                            return;
                        }
                    }
                    //Fill TB_DataCollect
                    Common.fut15_config.AddLogToListItem(lstLog1, "  Refresh TargetBid Collection List", 3);
                    Common.fut15_config.Platform_DB.FillTargetBid_Collection(TB_DataCollect, Common.fut15_config);
                    Common.fut15_config.AddLogToListItem(lstLog1, "  Refresh TargetBid Collection List Done", 3);
                    if (txtCoinsLimit.Text != "")
                        Common.fut15_config.CoinsLimit = Convert.ToInt32(txtCoinsLimit.Text);
                    _cycle++;
                    await SearchSaveListDisplayCoins(_cycle);

                    if (Common.fut15_config.JobUserAccountStatus.TradeUser.Credit > Common.fut15_config.CoinsLimit)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  Coins is more than limit. Got to bid.", 4);

                        break;
                    }
                }
                catch (Exception exc)
                {
                    Common.fut15_config.AddLogToListItem(lstLog1, "  Got Error:" + exc.Message);
                    if (exc is ExpiredSessionException)
                    {
                        if (Common.fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                            Common.fut15_config.AddLogToListItem(lstLog1, "sending mail failed");
                        NeedReLogin = true;
                        break;
                    }

                }

            }

            Common.fut15_config.AddLogToListItem(lstLog1, "Datacollection Done", 4);

            button1.Enabled = true;

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
            var player = Common.fut15_config.Platform_DB.Players.FirstOrDefault(p => p.resourceID == TargetAucInf.ItemData.ResourceId);
            while (true)
            {
                try
                {
                    //if it's the first time coming at this point of this function, add player into watch list.
                    if (_i == 0 && !BidFromWatchList)
                    {
                        //add it to watch list
                        if (Common.fut15_config.Platform_DB.AddWatchListQueue(TargetAucInf, Common.fut15_config) == 0 || Common.fut15_config.Platform_DB.AddWatchListQueue(TargetAucInf, Common.fut15_config) == -1)
                        {
                            Common.fut15_config.AddLogToListItem(lstLog1, ("  -TradeId " + TargetAucInf.TradeId + " ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") BidState:"
                                                + TargetAucInf.BidState + " CurrentBid:" + TargetAucInf.CurrentBid.ToString() + " StartBid:" + TargetAucInf.StartingBid.ToString()
                                                + " Expires:" + TargetAucInf.Expires.ToString()), 4);
                            if(cbAutoRefresh.Checked)
                                DisplayAuctionItem(dgPlayers, TargetAucInf);
                            await Task.Delay(2500);
                            await Common.fut15_config.FUT_Client_Facade.AddToWatchlistRequestAsync(TargetAucInf);
                            Common.fut15_config.AddLogToListItem(lstLog1, "  -Player: " + TargetAucInf.ItemData.ResourceId + " Name:(" + player.Name + ") CurrentBid:" + TargetAucInf.CurrentBid.ToString() + " added to watch list", 4);
                        }
                    }
                    _i++;

                    //Query this Trade ID again to return to TargetAucInf
                    //1. Query Watchlist
                    await Task.Delay(2500);
                    var watchlistResponse = await Common.fut15_config.FUT_Client_Facade.GetWatchlistAsync();
                    var aucL = watchlistResponse.AuctionInfo;
                    if (aucL == null)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  -Bid has been expired or outbid(1). Quit Bid.", 4);
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
                            //    Common.AddLogToListItem(lstLog1, "  New status: ResourceID:" + TargetAucInf.ItemData.ResourceId + " Name:(" + player.Name + ") BidState:" + TargetAucInf.BidState + " Expires:" + TargetAucInf.Expires + " CurrentBid:" + TargetAucInf.CurrentBid.ToString());

                            //    break;
                            //}
                            //Change to any the most urgent biddable auction Information
                            foreach (var _tbe in TBE)
                            {
                                if (_tbe.ResourceId == aucc.ItemData.ResourceId &&
                                   _tbe.targetBuyAt >= aucc.CurrentBid &&
                                   _tbe.targetBuyAt >= aucc.StartingBid &&
                                   _tbe.Biddable == true &&
                                   aucc.Expires != -1 &&
                                   aucc.Expires <= Common.fut15_config.Urgent_Bid_In_Seconds &&
                                   aucc.BidState != "highest" &&
                                   TargetAucInf.CurrentBid <= Common.fut15_config.PriceLimit)
                                {
                                    TargetAucInf = aucc;
                                    targetBuyAt = _tbe.targetBuyAt;
                                    foundIt = true;
                                    Common.fut15_config.AddLogToListItem(lstLog1, "  -New status(biddable): ResourceID:" + TargetAucInf.ItemData.ResourceId + " BidState:" + TargetAucInf.BidState + " Expires:" + TargetAucInf.Expires + " CurrentBid:" + TargetAucInf.CurrentBid.ToString() + " TargetBuyAt:" + targetBuyAt.ToString(), 4);
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
                                    if (aucc.BidState == "highest" && aucc.Expires != -1 && _tbe.ResourceId == aucc.ItemData.ResourceId)
                                    {
                                        TargetAucInf = aucc;
                                        targetBuyAt = _tbe.targetBuyAt;
                                        foundIt = true;
                                        Common.fut15_config.AddLogToListItem(lstLog1, "  -New status(highest): ResourceID:" + TargetAucInf.ItemData.ResourceId + " BidState:" + TargetAucInf.BidState + " Expires:" + TargetAucInf.Expires + " CurrentBid:" + TargetAucInf.CurrentBid.ToString() + " TargetBuyAt:" + targetBuyAt.ToString(), 4);
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
                                foreach (var _tbe in TBE)
                                {
                                    if (aucc.BidState == "highest" && aucc.Expires == -1)
                                    {
                                        TargetAucInf = aucc;
                                        targetBuyAt = _tbe.targetBuyAt;
                                        foundIt = true;
                                        Common.fut15_config.AddLogToListItem(lstLog1, "  -TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Bid successfully at price " + TargetAucInf.CurrentBid.ToString(), 4);
                                        return true;
                                    }
                                }
                            }
                        }

                        if (!foundIt)
                        {
                            Common.fut15_config.AddLogToListItem(lstLog1, "  -Bid has been expired or exceeded expected price. Quit Bid.", 4);
                            return false;
                        }
                    }

                    //If coins not enough quit bid
                    if (Common.fut15_config.JobUserAccountStatus.TradeUser.Credit < TargetAucInf.CurrentBid || Common.fut15_config.JobUserAccountStatus.TradeUser.Credit < TargetAucInf.StartingBid)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  -Coins not enough. Quit Bid.", 4);
                        //return false;
                    }

                    //5. If it's outbidstate != highest and expires != -1 and current bid < targetprice then bid it
                    if (TargetAucInf.BidState != "highest" && TargetAucInf.Expires != -1 && TargetAucInf.CurrentBid <= targetBuyAt && TargetAucInf.StartingBid <= targetBuyAt && TargetAucInf.CurrentBid <= Common.fut15_config.PriceLimit)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  -(Watchlist)TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Bid an urgent deal at next price of " + TargetAucInf.CurrentBid.ToString() + " Expected Price:" + targetBuyAt.ToString(), 4);
                        await Task.Delay(2500);
                        try
                        {
                            var auctionResponse = await Common.fut15_config.FUT_Client_Facade.PlaceBidAsync(TargetAucInf);
                        }
                        catch (Exception EE)
                        {
                            if (EE is ExpiredSessionException)
                            {
                                if (Common.fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                    Common.fut15_config.AddLogToListItem(lstLog1, "sending mail failed");
                                throw EE;
                            }
                            if (EE is PermissionDeniedException)
                            {
                                Common.fut15_config.AddLogToListItem(lstLog1, "  -(Watchlist)TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Outbid at this moment. try again next time.", 4);
                            }
                        }
                    }
                    if (TargetAucInf.CurrentBid > Common.fut15_config.PriceLimit)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  -TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Price has been exceeded Price Limit.", 4);
                    }
                    if (TargetAucInf.CurrentBid > targetBuyAt)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  -TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Price has been exceeded expected.", 4);
                    }
                    //4. If it's outbidstate != highest and expires == -1 return false
                    if (TargetAucInf.BidState != "highest" && TargetAucInf.Expires == -1)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  -TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Bid has been expired(0).", 4);
                    }
                    //4. If it's outbidstate == highest and expires == -1, return true
                    if (TargetAucInf.BidState == "highest" && TargetAucInf.Expires == -1)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  -TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Bid successfully at price " + TargetAucInf.CurrentBid.ToString(), 4);
                    }
                    //6. If it's outbidstate == highest and expires != -1 Loop
                    if (TargetAucInf.BidState == "highest" && TargetAucInf.Expires != -1)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  -TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Keep at the highest price " + TargetAucInf.CurrentBid.ToString(), 4);
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
                        for (uint _i_page = 1; _i_page < 3; _i_page++)
                        {
                            var searchParameters = new PlayerSearchParameters
                            {
                                Page = _i_page,
                                PageSize = 36,
                                Level = Level.Gold,
                                League = currentLeague
                            };

                            Common.fut15_config.AddLogToListItem(lstLog1, "  -Page(" + _i_page.ToString() + ")Quick Search League " + currentLeague.ToString() + " when QuickBidding! Saving Time.", 4);
                            await Task.Delay(Common.fut15_config.Query_Interval);
                            var searchResponse = await Common.fut15_config.FUT_Client_Facade.SearchAsync(searchParameters);
                            foreach (var auctionInfo in searchResponse.AuctionInfo)
                            {
                                foreach (var TB in TBE)
                                {
                                    if (TB.ResourceId == auctionInfo.ItemData.ResourceId
                                           && TB.Biddable == true
                                           && auctionInfo.BidState != "highest"
                                           && auctionInfo.Watched != true
                                           && auctionInfo.Expires <= Common.fut15_config.Urgent_Bid_In_Seconds
                                           && auctionInfo.Expires > -1
                                           && auctionInfo.CurrentBid <= TB.targetBuyAt
                                           && Common.fut15_config.JobUserAccountStatus.TradeUser.Credit > Common.fut15_config.CoinsLimit
                                           && auctionInfo.StartingBid <= TB.targetBuyAt
                                           && (TB.targetSaleMax > Common.fut15_config.Low_Price_To_Avoid_Collect || TB.targetSaleMax == 0))
                                    {
                                        TargetAucInf = auctionInfo;
                                        try
                                        {
                                            if (Common.fut15_config.Platform_DB.AddWatchListQueue(TargetAucInf, Common.fut15_config) == 0 || Common.fut15_config.Platform_DB.AddWatchListQueue(TargetAucInf, Common.fut15_config) == -1)
                                            {
                                                Common.fut15_config.AddLogToListItem(lstLog1, ("  -(Search)TradeId " + TargetAucInf.TradeId + " ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") BidState:"
                                                                    + TargetAucInf.BidState + " CurrentBid:" + TargetAucInf.CurrentBid.ToString() + " StartBid:" + TargetAucInf.StartingBid.ToString()
                                                                    + " Expires:" + TargetAucInf.Expires.ToString()), 4);
                                                if (cbAutoRefresh.Checked)
                                                    DisplayAuctionItem(dgPlayers, TargetAucInf);
                                                await Task.Delay(2500);
                                                await Common.fut15_config.FUT_Client_Facade.AddToWatchlistRequestAsync(TargetAucInf);
                                                Common.fut15_config.AddLogToListItem(lstLog1, "  -Player: " + TargetAucInf.ItemData.ResourceId + " Name:(" + player.Name + ") CurrentBid:" + TargetAucInf.CurrentBid.ToString() + " added to watch list", 4);

                                                Common.fut15_config.AddLogToListItem(lstLog1, "  -(Search)TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Bid an urgent deal at next price of " + TargetAucInf.CurrentBid.ToString() + " Expected Price:" + TB.targetBuyAt.ToString(), 4);
                                                await Task.Delay(2500);
                                                var auctionResponse = await Common.fut15_config.FUT_Client_Facade.PlaceBidAsync(TargetAucInf);
                                            }
                                        }
                                        catch (Exception EE)
                                        {
                                            if (EE is ExpiredSessionException)
                                            {
                                                if (Common.fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                                    Common.fut15_config.AddLogToListItem(lstLog1, "sending mail failed");
                                                throw EE;
                                            }
                                            if (EE is PermissionDeniedException)
                                            {
                                                Common.fut15_config.AddLogToListItem(lstLog1, "  -(Search)TradeId(" + TargetAucInf.TradeId.ToString() + ") ResourceID(" + TargetAucInf.ItemData.ResourceId.ToString() + ") - Outbid at this moment. Try again next time.", 4);
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
                    Common.fut15_config.AddLogToListItem(lstLog1, "  " + fee.Message);
                    if (fee is ExpiredSessionException)
                    {
                        if (Common.fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                            Common.fut15_config.AddLogToListItem(lstLog1, "sending mail failed");
                        throw fee;
                    }
                }
            }
        }


        private void button10_Click(object sender, EventArgs e)
        {
            //if (txtCoinsLimit.Text == "")
            //    _coinsLimit = Common.CoinsLimit;
            //else
            //    _coinsLimit = Convert.ToInt32(txtCoinsLimit.Text);

            //if (button10.Text.Trim() == "Data Collection")
            //{
            //    button10.Text = "Stop";
            //    while (button1.Text == "Stop")
            //    {
            //        if (button10.Text == "Stop")
            //            await DataCollectionLoop();
            //        if (button10.Text == "Stop")
            //            await BIDLoop();
            //    }

            //    Common.AddLogToListItem(lstLog1, "Bid Job Ended.");
            //    Common.AddLogToListItem(lstLog1, " ");
            //    Common.AddLogToListItem(lstLog1, " ");
            //    
            //    button10.Enabled = true;
            //}
            //else
            //{
            //    button10.Text = "Data Collection";
            //    button10.Enabled = false;
            //}
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private async Task SaveAndRemoveExpiredOnesInWatchList()
        {
            try
            {
                Common.fut15_config.AddLogToListItem(lstLog1, "Saving&Removing Expired ones Started", 4);
                //1. Get Player list from Watchlist
                //2. if it's untradable, save to DB, then remove it from watchlist.
                await Task.Delay(Common.fut15_config.Bid_Interval);
                var watchlistResponse = await Common.fut15_config.FUT_Client_Facade.GetWatchlistAsync();
                if (watchlistResponse != null)
                {
                    if (cbAutoRefresh.Checked)
                        RetrieveWatchlistGridview(dgPlayers, watchlistResponse.AuctionInfo);
                    List<AuctionInfo> LAI = new List<AuctionInfo>();
                    foreach (var auctionInfo in watchlistResponse.AuctionInfo)
                    {
                        if (auctionInfo.Expires == -1)
                        {
                            if (auctionInfo.CurrentBid > 0)
                            {
                                Common.fut15_config.SaveAuctionInfo(auctionInfo);
                                Common.fut15_config.AddLogToListItem(lstLog1, "  Trade " + auctionInfo.TradeId.ToString() + " Price (ResrouceID: " + auctionInfo.ItemData.ResourceId.ToString() + ") saved", 3);
                            }

                            if (auctionInfo.BidState != "highest")
                            {
                                //Remove it from watch list;
                                LAI.Add(auctionInfo);
                                Common.fut15_config.AddLogToListItem(lstLog1, "  Trade " + auctionInfo.TradeId.ToString() + " (ResrouceID: " + auctionInfo.ItemData.ResourceId.ToString() + ") removed from watchlist", 3);

                            }
                            else if (auctionInfo.BidState == "highest")
                            {
                                try
                                {
                                    //Send to Transferlist
                                    var player = Common.fut15_config.Platform_DB.Players.FirstOrDefault(p => p.resourceID == auctionInfo.ItemData.ResourceId);
                                    var tmp = Common.fut15_config.SalePrices.FirstOrDefault(a => a.ResourceID == auctionInfo.ItemData.ResourceId);
                                    if (tmp == null)
                                    {
                                        Common.fut15_config.SalePrices.Add(new SalePrice(auctionInfo.ItemData.ResourceId,
                                                                            TBE.FirstOrDefault(a => a.ResourceId == auctionInfo.ItemData.ResourceId).targetSaleMin,
                                                                            TBE.FirstOrDefault(a => a.ResourceId == auctionInfo.ItemData.ResourceId).targetSaleMax)
                                                                            );
                                        Common.fut15_config.AddLogToListItem(lstLog1, "  Add SalePrice Resource:"
                                                                            + auctionInfo.ItemData.ResourceId.ToString() + " TargetSaleMin:"
                                                                            + TBE.FirstOrDefault(a => a.ResourceId == auctionInfo.ItemData.ResourceId).targetSaleMin.ToString() + " TargetSaleMax:"
                                                                            + TBE.FirstOrDefault(a => a.ResourceId == auctionInfo.ItemData.ResourceId).targetSaleMax, 3);
                                    }
                                    int i_result = Common.fut15_config.Platform_DB.UpdateWatchListQueue(auctionInfo, "closed");
                                    int i_result2 = Common.fut15_config.Platform_DB.AddSaleTracker(auctionInfo, Common.fut15_config);
                                    Common.fut15_config.AddLogToListItem(lstLog1, "  ResourceId: " + auctionInfo.ItemData.ResourceId + " Name: " + player.Name + " Price: " + auctionInfo.CurrentBid.ToString() + " status is updated in table WatchListQueue(" + i_result.ToString() + ") & SaleTracker(" + i_result2.ToString() + ").", 3);
                                    await Task.Delay(Common.fut15_config.Bid_Interval);
                                    var sendToTradePileResponse = await Common.fut15_config.FUT_Client_Facade.SendItemToTradePileAsync(auctionInfo.ItemData);
                                    Common.fut15_config.AddLogToListItem(lstLog1, "  ResourceId: " + auctionInfo.ItemData.ResourceId + " Name: " + player.Name + " Price: " + auctionInfo.CurrentBid.ToString() + " is sent to transfer list", 3);
                                }
                                catch (Exception fee)
                                {
                                    Common.fut15_config.AddLogToListItem(lstLog1, "  " + fee.Message);
                                    if (fee is ExpiredSessionException)
                                    {
                                        if (Common.fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                            Common.fut15_config.AddLogToListItem(lstLog1, "sending mail failed");
                                        NeedReLogin = true;
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
                            if (Common.fut15_config.Platform_DB.UpdateWatchListQueue(LAI, "closed") != 0)
                                Common.fut15_config.AddLogToListItem(lstLog1, "failed to update status 'closed' in table [WatchListQueues]");
                            await Task.Delay(Common.fut15_config.Bid_Interval);
                            await Common.fut15_config.FUT_Client_Facade.RemoveFromWatchlistAsync(LAI);
                        }
                        catch (Exception ee)
                        {
                            Common.fut15_config.AddLogToListItem(lstLog1, ee.Message);
                            if (ee is ExpiredSessionException)
                            {
                                if (Common.fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                                    Common.fut15_config.AddLogToListItem(lstLog1, "sending mail failed");
                                NeedReLogin = true;
                                return;
                            }
                        }
                    }

                }
                Common.fut15_config.AddLogToListItem(lstLog1, "Saving&Removing Expired ones Done", 4);
            }
            catch (Exception fee)
            {
                Common.fut15_config.AddLogToListItem(lstLog1, fee.Message);
                if (fee is ExpiredSessionException)
                {
                    if (Common.fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                        Common.fut15_config.AddLogToListItem(lstLog1, "sending mail failed");
                    NeedReLogin = true;
                    return;
                }
            }

        }

        private async void DisplayAuctionItem(DataGridView dg, AuctionInfo auctionInfo)
        {
            try
            {
                DataGridViewRow row = (DataGridViewRow)dg.Rows[0].Clone();

                row.Cells[0].Value = auctionInfo.TradeId;
                row.Cells[1].Value = auctionInfo.ItemData.ResourceId;
                var player = Common.fut15_config.Platform_DB.Players.FirstOrDefault(a => a.resourceID == auctionInfo.ItemData.ResourceId);
                if (player != null)
                {
                    row.Cells[2].Value = player.Name;
                }
                else
                {
                    row.Cells[2].Value = "";
                }
                var imageBytes = await Common.fut15_config.FUT_Client_Facade.GetPlayerImageAsync(auctionInfo);
                if (imageBytes != null)
                {
                    row.Cells[3].Value = imageBytes;
                }
                row.Cells[4].Value = auctionInfo.BidState;
                row.Cells[5].Value = auctionInfo.Expires;

                row.Cells[6].Value = auctionInfo.CurrentBid;
                row.Cells[7].Value = auctionInfo.StartingBid;
                row.Cells[8].Value = auctionInfo.BuyNowPrice;
                row.Cells[9].Value = auctionInfo.ItemData.PreferredPosition;
                row.Cells[10].Value = auctionInfo.ItemData.Rating;
                row.Cells[11].Value = auctionInfo.ItemData.RareFlag;
                row.Cells[12].Value = auctionInfo.ItemData.Contract;
                row.Cells[13].Value = auctionInfo.ItemData.Fitness;
                row.Height = 90;
                dg.Rows.Add(row);
            }
            catch (Exception EE)
            {
                Common.fut15_config.AddLogToListItem(lstLog1, EE.Message);
                if (EE is ExpiredSessionException)
                {
                    if (Common.fut15_config.SendEmail("lestrois@gmail.com", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired", "FUT15 Trade User (" + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + ") Session Expired") == -1)
                        Common.fut15_config.AddLogToListItem(lstLog1, "sending mail failed");
                    NeedReLogin = true;
                    return;
                }
            }
        }

        private async Task ListAuctions()
        {
            Common.fut15_config.AddLogToListItem(lstLog1, "  List Auctions at targetBid starts", 4);

            await Task.Delay(Common.fut15_config.Bid_Interval);
            var watchlistResponse1 = await Common.fut15_config.FUT_Client_Facade.GetTradePileAsync();
            List<AuctionInfo> SortedList = watchlistResponse1.AuctionInfo.OrderBy(o => o.Expires).ToList();
            dgTransferList.Rows.Clear();
            Common.fut15_config.AuctionValue = 0;
            lblAuctionValue.Text = "...";
            foreach (var auctionInfo in SortedList)
            {
                try
                {
                    DisplayAuctionItem(dgTransferList, auctionInfo);
                    if (auctionInfo.Expires == -1 && auctionInfo.CurrentBid != 0)
                    {
                        try
                        {
                            Common.fut15_config.SaveAuctionInfo(auctionInfo);
                            Common.fut15_config.AddLogToListItem(lstLog1, "Trade " + auctionInfo.TradeId.ToString() + " Price (ResrouceID: " + auctionInfo.ItemData.ResourceId.ToString() + ") saved", 3);
                            int i_result = Common.fut15_config.Platform_DB.UpdateSaleTracker(auctionInfo, Common.fut15_config);
                            Common.fut15_config.AddLogToListItem(lstLog1, "  ResourceId: " + auctionInfo.ItemData.ResourceId + " Price: " + auctionInfo.CurrentBid.ToString() + " status is updated in table SaleTracker(" + i_result.ToString() + ").", 3);
                            await Task.Delay(Common.fut15_config.Bid_Interval);
                            await Common.fut15_config.FUT_Client_Facade.RemoveFromTradePileAsync(auctionInfo);
                            Common.fut15_config.AddLogToListItem(lstLog1, "ResourceID(" + auctionInfo.ItemData.ResourceId.ToString() + ") sold at price " + auctionInfo.CurrentBid.ToString() + ". Now it's removed from Transfer List.", 3);
                        }
                        catch (Exception ee)
                        {
                            Common.fut15_config.AddLogToListItem(lstLog1, ee.Message);
                        }
                    }
                    if (auctionInfo.Expires == -1 && auctionInfo.CurrentBid == 0 && checkBox2.Checked)
                    {
                        SalePrice _tbe = new SalePrice(0, 0, 0);
                        //var _tbe = TBE.Where(a => a.ResourceId == auctionInfo.ItemData.ResourceId && a.Biddable == true).FirstOrDefault();
                        try
                        {
                            _tbe = Common.fut15_config.SalePrices.FirstOrDefault(a => a.ResourceID == auctionInfo.ItemData.ResourceId);
                            if (_tbe == null)
                            {
                                var _tbe2 = TBE.FirstOrDefault(a => a.ResourceId == auctionInfo.ItemData.ResourceId);
                                _tbe = new SalePrice(auctionInfo.ItemData.ResourceId, _tbe2.targetSaleMin, _tbe2.targetSaleMax);
                                Common.fut15_config.SalePrices.Add(_tbe);
                            }
                            if (_tbe != null)
                            {
                                await Task.Delay(Common.fut15_config.Bid_Interval);
                                var auctionDetails = new AuctionDetails(auctionInfo.ItemData.Id, AuctionDuration.OneHour, Convert.ToUInt32(_tbe.TargetSaleMin), Convert.ToUInt32(_tbe.TargetSaleMax));
                                Common.fut15_config.AddLogToListItem(lstLog1, " Listing Auction ResourceID" + auctionInfo.ItemData.ResourceId.ToString() + "  TargetSaleMin:" + _tbe.TargetSaleMin.ToString(), 3);
                                Common.fut15_config.SalePrices.Remove(_tbe);
                                var listAuctionResponse = await Common.fut15_config.FUT_Client_Facade.ListAuctionAsync(auctionDetails);
                            }
                            else if (auctionInfo.ItemData.Rating < 80)
                            {
                                await Task.Delay(Common.fut15_config.Bid_Interval);
                                var auctionDetails = new AuctionDetails(auctionInfo.ItemData.Id);
                                Common.fut15_config.AddLogToListItem(lstLog1, " Listing Auction ResourceID" + auctionInfo.ItemData.ResourceId.ToString() + "  at 150.", 3);
                                var listAuctionResponse = await Common.fut15_config.FUT_Client_Facade.ListAuctionAsync(auctionDetails);
                            }
                        }
                        catch (Exception EE)
                        {
                            if (EE is NotFoundException)
                            {
                                Common.fut15_config.AddLogToListItem(lstLog1, " List Auction ResourceID" + auctionInfo.ItemData.ResourceId.ToString() + "  TargetSaleMin:" + _tbe.TargetSaleMin.ToString(), 3);
                                Common.fut15_config.SalePrices.Remove(_tbe);
                            }
                        }
                    }
                    if (auctionInfo.Expires == 0)
                    {
                        SalePrice _tbe = new SalePrice(0, 0, 0);
                        try
                        {
                            _tbe = Common.fut15_config.SalePrices.FirstOrDefault(a => a.ResourceID == auctionInfo.ItemData.ResourceId);
                            if (_tbe == null)
                            {
                                var _tbe2 = TBE.FirstOrDefault(a => a.ResourceId == auctionInfo.ItemData.ResourceId);
                                if (_tbe2 != null)
                                {
                                    _tbe = new SalePrice(auctionInfo.ItemData.ResourceId, _tbe2.targetSaleMin, _tbe2.targetSaleMax);
                                }
                                else
                                {
                                    _tbe = new SalePrice(auctionInfo.ItemData.ResourceId, 150, Common.fut15_config.Low_Price_To_Avoid_Collect);
                                }
                                Common.fut15_config.SalePrices.Add(_tbe);
                            }
                            Common.fut15_config.AuctionValue = Common.fut15_config.AuctionValue + Convert.ToInt32(_tbe.TargetSaleMin);
                            await Task.Delay(Common.fut15_config.Bid_Interval);
                            var auctionDetails = new AuctionDetails(auctionInfo.ItemData.Id, AuctionDuration.OneHour, Convert.ToUInt32(_tbe.TargetSaleMin), Convert.ToUInt32(_tbe.TargetSaleMax));
                            Common.fut15_config.AddLogToListItem(lstLog1, " Listing Auction ResourceID" + auctionInfo.ItemData.ResourceId.ToString() + "  TargetSaleMin:" + _tbe.TargetSaleMin.ToString(), 3);
                            Common.fut15_config.SalePrices.Remove(_tbe);
                            var listAuctionResponse = await Common.fut15_config.FUT_Client_Facade.ListAuctionAsync(auctionDetails);
                        }
                        catch (Exception DE)
                        {
                            Common.fut15_config.AddLogToListItem(lstLog1, DE.Message);
                            if (DE is NotFoundException)
                            {
                                Common.fut15_config.AddLogToListItem(lstLog1, " List Auction ResourceID" + auctionInfo.ItemData.ResourceId.ToString() + "  TargetSaleMin:" + _tbe.TargetSaleMin.ToString(), 3);
                                Common.fut15_config.SalePrices.Remove(_tbe);
                            }
                        }
                    }
                    if (auctionInfo.Expires != -1 || (auctionInfo.Expires == -1 && auctionInfo.CurrentBid == 0))
                        Common.fut15_config.AuctionValue = Common.fut15_config.AuctionValue + Convert.ToInt32((auctionInfo.CurrentBid == 0) ? auctionInfo.StartingBid : auctionInfo.CurrentBid);

                }
                catch (Exception e)
                {
                    Common.fut15_config.AddLogToListItem(lstLog1, e.Message);
                }
                Common.fut15_config.AuctionValue = Convert.ToInt32(Convert.ToDouble(Common.fut15_config.AuctionValue) * 0.95);
                lblAuctionValue.Text = Common.fut15_config.AuctionValue.ToString();
                lblTotalCoins.Text = (Convert.ToInt32(lblCoins.Text) + Convert.ToInt32(lblAuctionValue.Text)).ToString();
                Common.fut15_config.AddLogToListItem(lstLog1, "  List Auctions at targetBid done", 4);
            }
        }

        private async void RetrieveWatchlistGridview(DataGridView dg, List<AuctionInfo> AUC_L)
        {
            try
            {
                dg.Rows.Clear();
                List<AuctionInfo> SortedList = AUC_L.OrderBy(o => o.Expires).ToList();
                foreach (var auctionInfo in SortedList)
                {
                    try
                    {
                        DataGridViewRow row = (DataGridViewRow)dg.Rows[0].Clone();

                        row.Cells[0].Value = auctionInfo.TradeId;
                        row.Cells[1].Value = auctionInfo.ItemData.ResourceId;
                        var player = Common.fut15_config.Platform_DB.Players.FirstOrDefault(a => a.resourceID == auctionInfo.ItemData.ResourceId);
                        if (player != null)
                        {
                            row.Cells[2].Value = player.Name;
                        }
                        else
                        {
                            row.Cells[2].Value = "";
                        }
                        var imageBytes = await Common.fut15_config.FUT_Client_Facade.GetPlayerImageAsync(auctionInfo);
                        row.Cells[3].Value = imageBytes;
                        row.Cells[4].Value = auctionInfo.BidState;
                        row.Cells[5].Value = auctionInfo.Expires;

                        row.Cells[6].Value = auctionInfo.CurrentBid;
                        row.Cells[7].Value = auctionInfo.StartingBid;
                        row.Cells[8].Value = auctionInfo.BuyNowPrice;
                        row.Cells[9].Value = auctionInfo.ItemData.PreferredPosition;
                        row.Cells[10].Value = auctionInfo.ItemData.Rating;
                        row.Cells[11].Value = auctionInfo.ItemData.RareFlag;
                        row.Cells[12].Value = auctionInfo.ItemData.Contract;
                        row.Cells[13].Value = auctionInfo.ItemData.Fitness;
                        row.Height = 90;
                        dg.Rows.Add(row);
                    }
                    catch (FutErrorException fee)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  " + fee.Message);

                    }
                }
            }
            catch (FutException e1)
            {
                Common.fut15_config.AddLogToListItem(lstLog1, "  " + e1.Message);

            }
        }

        private async void button13_Click(object sender, EventArgs e)
        {
            Common.fut15_config.AddLogToListItem(lstLog1, " Retrieving transferlist starts.", 4);
            try
            {
                await Common.fut15_config.RetrieveTransferlistGridview(dgTransferList, lblAuctionValue, lblCoins, lblTotalCoins);
            }
            catch (Exception ee)
            {
                Common.fut15_config.AddLogToListItem(lstLog1, ee.Message);
            }
            Common.fut15_config.AddLogToListItem(lstLog1, " Retrieving transferlist done.", 4);
        }

        private async void btnGetPrice_Click(object sender, EventArgs e)
        {
            Common.fut15_config.AddLogToListItem(lstLog1, " Start Re-List.", 4);

            await Task.Delay(Common.fut15_config.Bid_Interval);
            await Common.fut15_config.FUT_Client_Facade.ReListAsync();
            Common.fut15_config.AddLogToListItem(lstLog1, " End Re-List.", 43);

        }

        private async void button11_Click(object sender, EventArgs e)
        {
            Common.fut15_config.AddLogToListItem(lstLog1, "Refresh TargetBid List", 4);
            Common.fut15_config.Platform_DB.FillTargetBid(TBE, Common.fut15_config);
            Common.fut15_config.AddLogToListItem(lstLog1, "Refresh TargetBid List Done", 4);
            await ListAuctions();
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            Common.fut15_config.AddLogToListItem(lstLog1, " Clearing closed items starts.", 4);

            await Task.Delay(Common.fut15_config.Bid_Interval);
            var watchlistResponse = await Common.fut15_config.FUT_Client_Facade.GetTradePileAsync();
            List<AuctionInfo> SortedList = watchlistResponse.AuctionInfo.OrderBy(o => o.Expires).ToList();
            lblAuctionValue.Text = "...";
            dgTransferList.Rows.Clear();
            Common.fut15_config.AuctionValue = 0;
            foreach (var auctionInfo in SortedList)
            {
                if (auctionInfo.Expires == -1 && auctionInfo.CurrentBid != 0)
                {
                    try
                    {
                        Common.fut15_config.SaveAuctionInfo(auctionInfo);
                        Common.fut15_config.AddLogToListItem(lstLog1, "Trade " + auctionInfo.TradeId.ToString() + " Price (ResrouceID: " + auctionInfo.ItemData.ResourceId.ToString() + ") saved", 3);
                        int i_result = Common.fut15_config.Platform_DB.UpdateSaleTracker(auctionInfo, Common.fut15_config);
                        Common.fut15_config.AddLogToListItem(lstLog1, "  ResourceId: " + auctionInfo.ItemData.ResourceId + " Price: " + auctionInfo.CurrentBid.ToString() + " status is updated in table SaleTracker(" + i_result.ToString() + ").", 3);
                        await Task.Delay(Common.fut15_config.Bid_Interval);
                        await Common.fut15_config.FUT_Client_Facade.RemoveFromTradePileAsync(auctionInfo);
                        Common.fut15_config.AddLogToListItem(lstLog1, "ResourceID(" + auctionInfo.ItemData.ResourceId.ToString() + ") sold at price " + auctionInfo.CurrentBid.ToString() + ". Now it's removed from Transfer List.");
                    }
                    catch (Exception ee)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, ee.Message);
                    }
                }
                else
                {
                    DisplayAuctionItem(dgTransferList, auctionInfo);
                    if (auctionInfo.Expires != -1 || (auctionInfo.Expires == -1 && auctionInfo.CurrentBid == 0))
                    {
                        Common.fut15_config.AuctionValue = Common.fut15_config.AuctionValue + Convert.ToInt32((auctionInfo.CurrentBid == 0) ? auctionInfo.StartingBid : auctionInfo.CurrentBid);
                    }
                }
            }
            Common.fut15_config.AuctionValue = Convert.ToInt32(Convert.ToDouble(Common.fut15_config.AuctionValue) * 0.95);
            lblAuctionValue.Text = Common.fut15_config.AuctionValue.ToString();
            lblTotalCoins.Text = (Convert.ToInt32(lblCoins.Text) + Convert.ToInt32(lblAuctionValue.Text)).ToString();

            Common.fut15_config.AddLogToListItem(lstLog1, " Clearing closed items Done.", 4);
        }

        private async void button14_Click(object sender, EventArgs e)
        {
            await CalculateAndDisplayFUTCoins();
        }

        private async Task SearchSaveListDisplayCoins(int CycleNumber)
        {
            Common.fut15_config.AddLogToListItem(lstLog1, CycleNumber.ToString() + ".Query Barclays PL Players Started", 3);
            //1. Search England Leagues
            await SearchPlayer(5, 36, Level.Gold, League.BarclaysPremierLeague);
            Common.fut15_config.AddLogToListItem(lstLog1, CycleNumber.ToString() + ".Query Barclays PL Players Done", 3);

            //2. Search Spain leagues
            Common.fut15_config.AddLogToListItem(lstLog1, CycleNumber.ToString() + ".Query Liga BBVA Players Started", 3);
            await SearchPlayer(5, 36, Level.Gold, League.LigaBbva);
            Common.fut15_config.AddLogToListItem(lstLog1, CycleNumber.ToString() + ".Query Liga BBVA Players Done", 3);

            //3. Search Germany Leagues
            Common.fut15_config.AddLogToListItem(lstLog1, CycleNumber.ToString() + ".Query Bundesliga Players Started", 3);
            await SearchPlayer(5, 36, Level.Gold, League.Bundesliga);
            Common.fut15_config.AddLogToListItem(lstLog1, CycleNumber.ToString() + ".Query Bundesliga Players Done", 3);

            //4. Search Italian Leagues
            Common.fut15_config.AddLogToListItem(lstLog1, CycleNumber.ToString() + ".Query Seria A Players Started", 3);
            await SearchPlayer(5, 36, Level.Gold, League.SerieA);
            Common.fut15_config.AddLogToListItem(lstLog1, CycleNumber.ToString() + ".Query Seria A Players Done", 3);

            //5. Search France Leagues
            Common.fut15_config.AddLogToListItem(lstLog1, CycleNumber.ToString() + ".Query Ligue 1 Players Started", 3);
            await SearchPlayer(5, 36, Level.Gold, League.Ligue1);
            Common.fut15_config.AddLogToListItem(lstLog1, CycleNumber.ToString() + ".Query Ligue 1 Players Done", 3);

            await SaveAndRemoveExpiredOnesInWatchList();
            GetWatchList();
            await ListAuctions();

            //Relist
            if (!chkStopAutoRelist.Checked)
            {
                Common.fut15_config.AddLogToListItem(lstLog1, "Relisting Cards in Transferlist", 3);
                await Task.Delay(Common.fut15_config.Bid_Interval);
                await Common.fut15_config.FUT_Client_Facade.ReListAsync();
                Common.fut15_config.AddLogToListItem(lstLog1, " ", 3);
            }


            //Get Coins, if coins is less than coin limit break to jump to bid process.
            await CalculateAndDisplayFUTCoins();
        }

        private async void button15_Click(object sender, EventArgs e)
        {
            int _count;
            int _cycle;
            if (button1.Text == "Stop" || button1.Enabled == false) return;

            if (button15.Text == "Data Collection")
            {
                _count = 0;
                _cycle = 0;
                button15.Text = "Stop";
                dgPlayers.Rows.Clear();
                int milliseconds = Convert.ToInt32(Common.fut15_config.Query_Interval);
                while (button15.Text == "Stop" && _count < 51)
                {
                    try
                    {
                        //Fill TB_DataCollect
                        Common.fut15_config.AddLogToListItem(lstLog1, "  Refresh TargetBid Collection List", 3);
                        Common.fut15_config.Platform_DB.FillTargetBid_Collection(TB_DataCollect, Common.fut15_config);
                        Common.fut15_config.AddLogToListItem(lstLog1, "  Refresh TargetBid Collection List Done", 3);
                        _cycle++;
                        await SearchSaveListDisplayCoins(_cycle);
                    }
                    catch (Exception exc)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  Got Error:" + exc.Message);
                    }

                }

                Common.fut15_config.AddLogToListItem(lstLog1, "Done", 4);

                button15.Enabled = true;

            }
            else
            {
                button15.Text = "Data Collection";
                button15.Enabled = false;
                Common.fut15_config.AddLogToListItem(lstLog1, "You are stopping the job. Please wait...", 4);

            }
        }

        private async void button16_Click(object sender, EventArgs e)
        {
            int CycleNumber = 1;
            Common.fut15_config.AddLogToListItem(lstLog1, CycleNumber.ToString() + ".Query Barclays PL Players Started", 3);
            //1. Search England Leagues
            await SearchPlayer(5, 36, Level.Gold, League.BarclaysPremierLeague);
            Common.fut15_config.AddLogToListItem(lstLog1, CycleNumber.ToString() + ".Query Barclays PL Players Done", 3);

        }

        private void button17_Click(object sender, EventArgs e)
        {
            string ToBeCopied = string.Empty;
            Clipboard.Clear();
            for (int i = 0; i < lstLog1.Items.Count; i++)
            {
                ToBeCopied = ToBeCopied + lstLog1.Items[i].SubItems[0].Text + "\n";
            }
            Clipboard.SetText(ToBeCopied);
            Common.fut15_config.AddLogToListItem(lstLog1, "Logs are copied to Clipboard. Ctrl+V to paste it to a text file.", 4);
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Common.fut15_config.Log_Level = Convert.ToInt32(comboBox1.SelectedItem);
        }

        private void cbPriceLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            Common.fut15_config.PriceLimit = Convert.ToInt32(cbPriceLimit.SelectedItem);
        }

        private void txtCoinsLimit_TextChanged(object sender, EventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {
            //Save to file
            string ToBeCopied = string.Empty;
            Clipboard.Clear();
            for (int i = 0; i < lstLog1.Items.Count; i++)
            {
                ToBeCopied = ToBeCopied + lstLog1.Items[i].SubItems[0].Text + "\n";
            }
            Clipboard.SetText(ToBeCopied);
            Common.fut15_config.AddLogToListItem(lstLog1, "Logs are copied to Clipboard. Ctrl+V to paste it to a text file.", 4);
        }

        private async void dgTransferList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Send to Club
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                long TradeID = Convert.ToInt64(senderGrid.Rows[e.RowIndex].Cells[0].Value);
                Common.fut15_config.AddLogToListItem(lstLog1, "Sending Resource() Name() to club Started.", 4);
                await Task.Delay(Common.fut15_config.Bid_Interval);
                var transferlistResponse = await Common.fut15_config.FUT_Client_Facade.GetTradePileAsync();
                dgTransferList.Rows.Clear();
                lblAuctionValue.Text = "...";
                Common.fut15_config.AuctionValue = 0;
                foreach (var auctionInfo in transferlistResponse.AuctionInfo)
                {
                    try
                    {
                        if (auctionInfo.TradeId == TradeID)
                        {
                            await Task.Delay(Common.fut15_config.Bid_Interval);
                            await Common.fut15_config.FUT_Client_Facade.SendItemToClubAsync(auctionInfo.ItemData);
                        }
                        else
                        {
                            DisplayAuctionItem(dgTransferList, auctionInfo);

                            if (auctionInfo.Expires != -1 || (auctionInfo.Expires == -1 && auctionInfo.CurrentBid == 0))
                            {
                                Common.fut15_config.AuctionValue = Common.fut15_config.AuctionValue + Convert.ToInt32((auctionInfo.CurrentBid == 0) ? auctionInfo.StartingBid : auctionInfo.CurrentBid);
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, ee.Message);
                    }
                }

                Common.fut15_config.AuctionValue = Convert.ToInt32(Convert.ToDouble(Common.fut15_config.AuctionValue) * 0.95);
                lblAuctionValue.Text = Common.fut15_config.AuctionValue.ToString();
                lblTotalCoins.Text = (Convert.ToInt32(lblCoins.Text) + Convert.ToInt32(lblAuctionValue.Text)).ToString();

                Common.fut15_config.AddLogToListItem(lstLog1, "Sending Resource() Name() to club Done.", 4);
            }
        }

    }
}
