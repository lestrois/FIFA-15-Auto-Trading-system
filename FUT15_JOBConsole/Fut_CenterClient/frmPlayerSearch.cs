using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UltimateTeam.Toolkit.Constants;
using UltimateTeam.Toolkit.Exceptions;
using UltimateTeam.Toolkit.Extensions;
using UltimateTeam.Toolkit.Models;
using UltimateTeam.Toolkit.Services;
using UltimateTeam.Toolkit;
using UltimateTeam.Toolkit.Parameters;
using System.Configuration;
using FUT15_Center;
using FUT15_Center.Models;
using FUT15_JOB;
using FUT15_JOB.Models;
using FUT_MonitorCenter.Modle;

namespace FUT_MonitorCenter
{
    public partial class frmPlayerSearch : Form
    {
        private int _count;
        private int _cycle;
        List<TargetBidInfo_Enhanced> TBE = new List<TargetBidInfo_Enhanced>();
        private List<TargetBidInfo> TB_DataCollect;
        public frmPlayerSearch()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //Fill TB_DataCollect
            Common.fut15_config.AddLogToListItem(lstLog1, "  Refresh TargetBid Collection List");

            Common.fut15_config.Platform_DB.FillTargetBid_Collection(TB_DataCollect, Common.fut15_config);
            Common.fut15_config.AddLogToListItem(lstLog1, "  Refresh TargetBid Collection List Done");
            
            if (button1.Text == "Start")
            {
                _count = 0;
                _cycle = 0;
                button1.Text = "Stop";
                dgPlayers.Rows.Clear();
                int milliseconds = Convert.ToInt32(Common.fut15_config.Query_Interval);
                while (button1.Text == "Stop" && _count < 51)
                {
                    try
                    {
                        _cycle++;
                        Common.fut15_config.AddLogToListItem(lstLog1, _cycle.ToString() + ".Query England Players Started");
                        //1. Search England Leagues
                        await SearchPlayer(1, 36, Level.Gold, League.BarclaysPremierLeague);
                        Common.fut15_config.AddLogToListItem(lstLog1, _cycle.ToString() + ".Query England Players Done");
                        

                        //2. Search Spain leagues
                        Common.fut15_config.AddLogToListItem(lstLog1, _cycle.ToString() + ".Query Spain Players Started");
                        await SearchPlayer(1, 36, Level.Gold, League.LigaBbva);
                        Common.fut15_config.AddLogToListItem(lstLog1, _cycle.ToString() + ".Query Spain Players Done");
                        

                        //3. Search Germany Leagues
                        Common.fut15_config.AddLogToListItem(lstLog1, _cycle.ToString() + ".Query Germany Players Started");
                        await SearchPlayer(1, 36, Level.Gold, League.Bundesliga);
                        Common.fut15_config.AddLogToListItem(lstLog1, _cycle.ToString() + ".Query Germany Players Done");
                        

                        //4. Search Italian Leagues
                        Common.fut15_config.AddLogToListItem(lstLog1, _cycle.ToString() + ".Query Italian Players Started");
                        await SearchPlayer(1, 36, Level.Gold, League.SerieA);
                        Common.fut15_config.AddLogToListItem(lstLog1, _cycle.ToString() + ".Query Italian Players Done");
                        
                        //5. Search France Leagues
                        Common.fut15_config.AddLogToListItem(lstLog1, _cycle.ToString() + ".Query France Players Started");
                        await SearchPlayer(1, 36, Level.Gold, League.Ligue1);
                        Common.fut15_config.AddLogToListItem(lstLog1, _cycle.ToString() + ".Query France Players Done");
                        


                        //Save to DB for expired ones.
                        //1. Get Player list from Watchlist
                        //2. if it's untradable, save to DB, then remove it from watchlist.
                        List<AuctionInfo> LAI = new List<AuctionInfo>();
                        Common.fut15_config.AddLogToListItem(lstLog1, "Saving Watchlist Started");
                        
                        await Task.Delay(Common.fut15_config.Bid_Interval);
                        var watchlistResponse = await Common.fut15_config.FUT_Client_Facade.GetWatchlistAsync();
                        if (watchlistResponse == null) return;
                        foreach (var auctionInfo in watchlistResponse.AuctionInfo)
                        {
                            if (auctionInfo.Expires == -1)
                            {
                                if (auctionInfo.CurrentBid > 0)
                                {
                                    Common.fut15_config.SaveAuctionInfo(auctionInfo);
                                    Common.fut15_config.AddLogToListItem(lstLog1, "Trade " + auctionInfo.TradeId.ToString() + " (ResrouceID: " + auctionInfo.ItemData.ResourceId.ToString() + ") saved");
                                    
                                }

                                //Remove it from watch list;
                                LAI.Add(auctionInfo);
                                Common.fut15_config.AddLogToListItem(lstLog1, "Trade " + auctionInfo.TradeId.ToString() + " (ResrouceID: " + auctionInfo.ItemData.ResourceId.ToString() + ") removed from watchlist");
                                
                            }
                        }
                        if (LAI.Count != 0)
                        {
                            await Task.Delay(Common.fut15_config.Bid_Interval);
                            await Common.fut15_config.FUT_Client_Facade.RemoveFromWatchlistAsync(LAI);
                        }

                        Common.fut15_config.Platform_DB.Dispose();
                        Common.fut15_config.Platform_DB = new FUT15Entities();
                        //Common.Platform_DB.RunSP("UpdateTargetBidInfor");
                        Common.fut15_config.AddLogToListItem(lstLog1, "Saving Watchlist Done");

                        Common.fut15_config.AddLogToListItem(lstLog1, "  Refresh TargetBid Collection List");

                        Common.fut15_config.Platform_DB.FillTargetBid_Collection(TB_DataCollect, Common.fut15_config);
                        Common.fut15_config.AddLogToListItem(lstLog1, "  Refresh TargetBid Collection List Done");
                        

                        Common.fut15_config.AddLogToListItem(lstLog1, "  Refresh TargetBid List");

                        Common.fut15_config.Platform_DB.FillTargetBid(TBE, Common.fut15_config);
                        Common.fut15_config.AddLogToListItem(lstLog1, "  Refresh TargetBid List Done");
                        

                        Common.fut15_config.AddLogToListItem(lstLog1, "  List Auctions at targetBid");
                        
                        await Task.Delay(Common.fut15_config.Bid_Interval);
                        var watchlistResponse1 = await Common.fut15_config.FUT_Client_Facade.GetTradePileAsync();
                        List<AuctionInfo> SortedList = watchlistResponse1.AuctionInfo.OrderBy(o => o.Expires).ToList();
                        foreach (var auctionInfo in SortedList)
                        {
                            try
                            {
                                if (auctionInfo.Expires == -1 && auctionInfo.CurrentBid != 0)
                                {
                                    await Task.Delay(Common.fut15_config.Bid_Interval);
                                    await Common.fut15_config.FUT_Client_Facade.RemoveFromTradePileAsync(auctionInfo);
                                }
                                if (auctionInfo.Expires == -1 && auctionInfo.CurrentBid == 0 && checkBox2.Checked)
                                {
                                    var _tbe = TBE.Where(a => a.ResourceId == auctionInfo.ItemData.ResourceId).FirstOrDefault();
                                    await Task.Delay(Common.fut15_config.Bid_Interval);
                                    var auctionDetails = new AuctionDetails(auctionInfo.ItemData.Id, AuctionDuration.OneHour, Convert.ToUInt32(_tbe.targetSaleMin), Convert.ToUInt32(_tbe.targetSaleMax));
                                    var listAuctionResponse = await Common.fut15_config.FUT_Client_Facade.ListAuctionAsync(auctionDetails);
                                    Common.fut15_config.AddLogToListItem(lstLog1, " List Auction ResourceID" + auctionInfo.ItemData.ResourceId.ToString() + "  TargetSaleMin:" + _tbe.targetSaleMin.ToString());
                                    
                                }
                                if (auctionInfo.Expires == 0)
                                {
                                    var _tbe = TBE.Where(a => a.ResourceId == auctionInfo.ItemData.ResourceId).FirstOrDefault();
                                    await Task.Delay(Common.fut15_config.Bid_Interval);
                                    var auctionDetails = new AuctionDetails(auctionInfo.ItemData.Id, AuctionDuration.OneHour, Convert.ToUInt32(_tbe.targetSaleMin), Convert.ToUInt32(_tbe.targetSaleMax));
                                    var listAuctionResponse = await Common.fut15_config.FUT_Client_Facade.ListAuctionAsync(auctionDetails);
                                    Common.fut15_config.AddLogToListItem(lstLog1, " List Auction ResourceID" + auctionInfo.ItemData.ResourceId.ToString() + "  TargetSaleMin:" + _tbe.targetSaleMin.ToString());
                                    
                                }
                            }
                            catch (Exception ee)
                            {
                                Common.fut15_config.AddLogToListItem(lstLog1, ee.Message);
                            }
                        }
                        Common.fut15_config.AddLogToListItem(lstLog1, "  List Auctions at targetBid Done");
                        

                        GetWatchList();

                        //Relist
                        Common.fut15_config.AddLogToListItem(lstLog1, "Relisting Cards in Transferlist");
                        await Task.Delay(Common.fut15_config.Bid_Interval);
                        await Common.fut15_config.FUT_Client_Facade.ReListAsync();
                        Common.fut15_config.AddLogToListItem(lstLog1, " ");
                        

                        //Common.AddLogToListItem(lstLog1, "Couting Watchlist count ... ");
                        //
                        //await Task.Delay(Common.Bid_Interval);
                        //watchlistResponse = await Common.FUT_Client_Facade.GetWatchlistAsync();
                        //_count = watchlistResponse.AuctionInfo.Count;
                        //Common.AddLogToListItem(lstLog1, "Watchlist count is " + _count.ToString() + " ... ");
                        //
                        //if (_count >= 50)
                        //{
                        //    Common.AddLogToListItem(lstLog1, "Watchlist is full. Waiting for 5 minutes ... ");
                        //    
                        //}
                    }
                    catch (Exception exc)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  Got Error:" + exc.Message);
                    }

                }

                Common.fut15_config.AddLogToListItem(lstLog1, "Done");
                
                button1.Enabled = true;

            }
            else
            {
                button1.Text = "Start";
                button1.Enabled = false;
                Common.fut15_config.AddLogToListItem(lstLog1, "You are stopping the job. Please wait...");
                
            }
        }

        private async void GetWatchList()
        {
            Common.fut15_config.AddLogToListItem(lstLog1, "Start retrieving watchlist");
            
            await Common.fut15_config.RetrieveWatchlistGridview(dgPlayers);
            lblBidCoins.Text = Common.fut15_config.BID_Coins.ToString();
            Common.fut15_config.AddLogToListItem(lstLog1, "End retrieving watchlist");
            
        }

        private async Task SearchPlayer(uint pageNumber, byte pageSize, Level level, uint league)
        {
            bool NeedCollect;
            var searchParameters = new PlayerSearchParameters
            {
                Page = pageNumber,
                PageSize = pageSize,
                Level = level,
                League = league
            };
            //1. Get Player list
            //1.1 rating larger than 79
            //1.2 current bid <> 0
            //1.3 max bid > 5000
            //2. Add to Watch list
            await Task.Delay(Common.fut15_config.Bid_Interval);
            var searchResponse = await Common.fut15_config.FUT_Client_Facade.SearchAsync(searchParameters);
            Common.fut15_config.AddLogToListItem(lstLog1, searchResponse.AuctionInfo.Count + " Players found");
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
                if (auctionInfo.ItemData.Rating > 79 && auctionInfo.CurrentBid != 0 && auctionInfo.Expires < 180 && auctionInfo.Watched != true && NeedCollect)
                {
                    DataGridViewRow row = (DataGridViewRow)dgPlayers.Rows[0].Clone();

                    row.Cells[0].Value = auctionInfo.ItemData.ResourceId;
                    row.Cells[1].Value = auctionInfo.ItemData.Rating;
                    row.Cells[2].Value = auctionInfo.ItemData.RareFlag;
                    row.Cells[3].Value = auctionInfo.CurrentBid;
                    row.Cells[4].Value = auctionInfo.StartingBid;
                    row.Cells[5].Value = auctionInfo.BuyNowPrice;
                    var imageBytes = await Common.fut15_config.FUT_Client_Facade.GetPlayerImageAsync(auctionInfo);
                    row.Cells[6].Value = imageBytes;
                    row.Cells[7].Value = auctionInfo.TradeState;
                    row.Height = 90;
                    dgPlayers.Rows.Add(row);
                    await Task.Delay(Common.fut15_config.Bid_Interval);
                    await Common.fut15_config.FUT_Client_Facade.AddToWatchlistRequestAsync(auctionInfo);
                    Common.fut15_config.AddLogToListItem(lstLog1, "Player: " + auctionInfo.ItemData.ResourceId.ToString() + " CurrentBid:" + auctionInfo.CurrentBid.ToString() + " added to watch list");
                    
                }
            }
        }
        private void frmPlayerSearch_Load(object sender, EventArgs e)
        {
            TB_DataCollect = new List<TargetBidInfo>();
            TBE = new List<TargetBidInfo_Enhanced>();
        }

        private void dgPlayers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            lstLog1.Items.Clear();
        }
    }
}
