using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FUT15_Center;
using FUT15_Center.Models;
using FUT15_JOB;
using FUT15_JOB.Models;
using UltimateTeam.Toolkit.Constants;
using UltimateTeam.Toolkit.Exceptions;
using UltimateTeam.Toolkit.Extensions;
using UltimateTeam.Toolkit.Models;
using UltimateTeam.Toolkit.Services;
using UltimateTeam.Toolkit;
using UltimateTeam.Toolkit.Parameters;
using System.Data.Entity;
using FUT_MonitorCenter.Modle;

namespace FUT_MonitorCenter
{
    public partial class frmTransferMarket : Form
    {
        List<AuctionInfo> CurrentAIList = new List<AuctionInfo>();
        AuctionResponse searchResponse;
        List<TargetBidInfo_Enhanced> TBE = new List<TargetBidInfo_Enhanced>();
        public frmTransferMarket()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
        private async Task SaveAndRemoveExpiredOnesInWatchList()
        {
            List<AuctionInfo> LAI = new List<AuctionInfo>();
            try
            {
                Common.fut15_config.AddLogToListItem(lstLog1, "Saving&Removing Expired ones Started");
                //1. Get Player list from Watchlist
                //2. if it's untradable, save to DB, then remove it from watchlist.
                await Task.Delay(Common.fut15_config.Bid_Interval);
                var watchlistResponse = await Common.fut15_config.FUT_Client_Facade.GetWatchlistAsync();
                if (watchlistResponse != null)
                {
                    foreach (var auctionInfo in watchlistResponse.AuctionInfo)
                    {
                        if (auctionInfo.Expires == -1)
                        {
                            if (auctionInfo.CurrentBid > 0)
                            {
                                Common.fut15_config.SaveAuctionInfo(auctionInfo);
                                Common.fut15_config.AddLogToListItem(lstLog1, "Trade " + auctionInfo.TradeId.ToString() + " Price (ResrouceID: " + auctionInfo.ItemData.ResourceId.ToString() + ") saved");
                            }

                            if (auctionInfo.BidState != "highest")
                            {
                                //Remove it from watch list;
                                LAI.Add(auctionInfo);
                                Common.fut15_config.AddLogToListItem(lstLog1, "Trade " + auctionInfo.TradeId.ToString() + " (ResrouceID: " + auctionInfo.ItemData.ResourceId.ToString() + ") removed from watchlist");
                                
                            }
                            else if (auctionInfo.BidState == "highest")
                            {
                                try
                                {
                                    //Send to Transferlist
                                    var player = Common.fut15_config.Platform_DB.Players.FirstOrDefault(p => p.resourceID == auctionInfo.ItemData.ResourceId);
                                    await Task.Delay(Common.fut15_config.Bid_Interval);
                                    var sendToTradePileResponse = await Common.fut15_config.FUT_Client_Facade.SendItemToTradePileAsync(auctionInfo.ItemData);
                                    Common.fut15_config.AddLogToListItem(lstLog1, "  ResourceId: " + auctionInfo.ItemData.ResourceId + " Name: " + player.Name + " Price: " + auctionInfo.CurrentBid.ToString() + " is sent to transfer list");
                                }
                                catch (FutErrorException fee)
                                {
                                    Common.fut15_config.AddLogToListItem(lstLog1, "  " + fee.Message);
                                }
                            }
                        }
                    }
                    if (LAI.Count != 0)
                    {
                        await Task.Delay(Common.fut15_config.Bid_Interval);
                        await Common.fut15_config.FUT_Client_Facade.RemoveFromWatchlistAsync(LAI);
                    }

                    Common.fut15_config.AddLogToListItem(lstLog1, "  Refresh TargetBid List");

                    Common.fut15_config.Platform_DB.FillTargetBid(TBE, Common.fut15_config);
                    Common.fut15_config.AddLogToListItem(lstLog1, "  Refresh TargetBid List Done");
                    

                }
                Common.fut15_config.AddLogToListItem(lstLog1, "Saving&Removing Expired ones Done");
            }
            catch (FutErrorException fee)
            {
                Common.fut15_config.AddLogToListItem(lstLog1, "  " + fee.Message);
                Common.fut15_config.AddLogToListItem(lstLog1, "  Refresh TargetBid List");

                Common.fut15_config.Platform_DB.FillTargetBid(TBE, Common.fut15_config);
                Common.fut15_config.AddLogToListItem(lstLog1, "  Refresh TargetBid List Done");
                
            }

        }
        private async void dgPlayers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            //Add to watchlist
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex == 20)
            {
                try
                {
                    long tradeID = Convert.ToInt64(dgPlayers.Rows[e.RowIndex].Cells[0].Value);
                    var aucWatch = CurrentAIList.FirstOrDefault(a => a.TradeId == tradeID);

                    if (aucWatch != null)
                    {
                        await Task.Delay(2500);
                        await Common.fut15_config.FUT_Client_Facade.AddToWatchlistRequestAsync(aucWatch);
                        Common.fut15_config.AddLogToListItem(lstLog1, "Resource(" + aucWatch.ItemData.ResourceId.ToString() + ") has been added to watchlist.");
                    }
                }
                catch (Exception EE)
                {
                    Common.fut15_config.AddLogToListItem(lstLog1, EE.Message);
                }

            }
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex == 21)
            {
                //Get refreshed TargetBid List
                //Get into QuickBid Mode
                Common.fut15_config.Platform_DB.FillTargetBid(TBE, Common.fut15_config);
                foreach (var _TB in TBE)
                {
                    if(_TB.ResourceId == Convert.ToInt64(dgPlayers.Rows[e.RowIndex].Cells[1].Value))
                    {
                        long lTradeID = Convert.ToInt64(dgPlayers.Rows[e.RowIndex].Cells[0].Value);

                        foreach (var auctionInfo in CurrentAIList)
                        {
                            if (auctionInfo.TradeId == lTradeID)
                            {
                                Common.fut15_config.AddLogToListItem(lstLog1, "  Get into single Player Card Bid Mode ... ResourceID:" + _TB.ResourceId.ToString() + " TargetPrice:" + _TB.targetBuyAt.ToString());
                                
                                if (!(_TB.ResourceId == auctionInfo.ItemData.ResourceId
                                    && _TB.Biddable == true
                                    && auctionInfo.BidState != "highest"
                                    && auctionInfo.Expires <= Common.fut15_config.Urgent_Bid_In_Seconds
                                    && auctionInfo.Expires > -1
                                    && auctionInfo.CurrentBid <= _TB.targetBuyAt
                                    && auctionInfo.StartingBid <= _TB.targetBuyAt))
                                {
                                    Common.fut15_config.AddLogToListItem(lstLog1, "  Can't bid it due to the current state!");
                                    return;
                                }
                                var bResult = await QuickBid(_TB.targetBuyAt, auctionInfo);
                                if (bResult == true)
                                {
                                    Common.fut15_config.AddLogToListItem(lstLog1, "  Bid successfully!");
                                    

                                    //Save and clear expired ones
                                    //1. Get Player list from Watchlist
                                    //2. if it's untradable, save to DB, then remove it from watchlist.
                                    await SaveAndRemoveExpiredOnesInWatchList();

                                    //List Target Bid auctions
                                    await Task.Delay(Common.fut15_config.Bid_Interval);
                                    var watchlistResponse1 = await Common.fut15_config.FUT_Client_Facade.GetTradePileAsync();
                                    List<AuctionInfo> SortedList = watchlistResponse1.AuctionInfo.OrderBy(o => o.Expires).ToList();
                                    foreach (var auctionInfo_Watch in SortedList)
                                    {
                                        if (auctionInfo_Watch.Expires == -1 && auctionInfo_Watch.CurrentBid != 0)
                                        {
                                            await Task.Delay(Common.fut15_config.Bid_Interval);
                                            await Common.fut15_config.FUT_Client_Facade.RemoveFromTradePileAsync(auctionInfo_Watch);
                                        }
                                        if (auctionInfo_Watch.Expires == 0)
                                        {
                                            var _tbe = TBE.Where(a => a.ResourceId == auctionInfo_Watch.ItemData.ResourceId).FirstOrDefault();
                                            await Task.Delay(Common.fut15_config.Bid_Interval);
                                            var auctionDetails = new AuctionDetails(auctionInfo_Watch.ItemData.Id, AuctionDuration.OneHour, Convert.ToUInt32(_tbe.targetSaleMin), Convert.ToUInt32(_tbe.targetSaleMax));
                                            var listAuctionResponse = await Common.fut15_config.FUT_Client_Facade.ListAuctionAsync(auctionDetails);
                                            Common.fut15_config.AddLogToListItem(lstLog1, " List Auction ResourceID" + auctionInfo_Watch.ItemData.ResourceId.ToString() + "  TargetSaleMin:" + _tbe.targetSaleMin.ToString());
                                            
                                        }
                                    }

                                    //Relist
                                    Common.fut15_config.AddLogToListItem(lstLog1, "Relisting Cards in Transferlist");
                                    
                                    await Task.Delay(Common.fut15_config.Bid_Interval);
                                    await Common.fut15_config.FUT_Client_Facade.ReListAsync();
                                    Common.fut15_config.AddLogToListItem(lstLog1, " ");
                                    

                                }
                                Common.fut15_config.AddLogToListItem(lstLog1, "Bid Failed.");
                                
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }

        private async Task<bool> QuickBid(int targetBuyAt, AuctionInfo TargetAucInf)
        {
            int _i = 0;
            bool foundIt = false;
            //send to tradepile
            await Common.fut15_config.JobUserAccountStatus.GetCoins();
            var player = Common.fut15_config.Platform_DB.Players.FirstOrDefault(p => p.resourceID == TargetAucInf.ItemData.ResourceId);
            while (true)
            {
                try
                {
                    //if it's not the first time coming at this point of this function Query
                    if (_i == 0)
                    {
                        //add it to watch list
                        await Task.Delay(2500);
                        await Common.fut15_config.FUT_Client_Facade.AddToWatchlistRequestAsync(TargetAucInf);
                        Common.fut15_config.AddLogToListItem(lstLog1, "  Player: " + TargetAucInf.ItemData.ResourceId + " Name:(" + player.Name + ") CurrentBid:" + TargetAucInf.CurrentBid.ToString() + " added to watch list");
                        
                    }
                    _i++;

                    //Query this Trade ID again to return to TargetAucInf
                    //1. Query Watchlist
                    await Task.Delay(2500);
                    var watchlistResponse = await Common.fut15_config.FUT_Client_Facade.GetWatchlistAsync();
                    var aucL = watchlistResponse.AuctionInfo;
                    if (aucL == null)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  Bid has been expired or outbid(1). Quit Bid.");
                        

                        return false;
                    }
                    else
                    {
                        foundIt = false;
                        //Loop to find out the right Trade ID
                        foreach (var aucc in aucL)
                        {
                            if (aucc.TradeId == TargetAucInf.TradeId)
                            {
                                TargetAucInf = aucc;
                                foundIt = true;
                                Common.fut15_config.AddLogToListItem(lstLog1, "  New status: ResourceID:" + TargetAucInf.ItemData.ResourceId + " Name:(" + player.Name + ") BidState:" + TargetAucInf.BidState + " Expires:" + TargetAucInf.Expires + " CurrentBid:" + TargetAucInf.CurrentBid.ToString());
                                
                                break;
                            }
                        }
                        if (!foundIt)
                        {
                            Common.fut15_config.AddLogToListItem(lstLog1, "  Bid has been expired(2). Quit Bid.");
                            

                            return false;
                        }
                    }

                    //If coins not enough quit bid
                    if (Common.fut15_config.JobUserAccountStatus.TradeUser.Credit < TargetAucInf.CurrentBid || Common.fut15_config.JobUserAccountStatus.TradeUser.Credit < TargetAucInf.StartingBid)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  Coins not enough. Quit Bid.");
                        
                        return false;
                    }

                    //5. If it's outbidstate != highest and expires != -1 and current bid < targetprice then bid it
                    if (TargetAucInf.BidState != "highest" && TargetAucInf.Expires != -1 && TargetAucInf.CurrentBid <= targetBuyAt && TargetAucInf.StartingBid <= targetBuyAt && TargetAucInf.CurrentBid <= Common.fut15_config.PriceLimit)
                    {
                        await Task.Delay(2500);
                        var auctionResponse = await Common.fut15_config.FUT_Client_Facade.PlaceBidAsync(TargetAucInf);
                        Common.fut15_config.AddLogToListItem(lstLog1, "  Bid an urgent TradeId " + TargetAucInf.TradeId + " at next price of " + TargetAucInf.CurrentBid.ToString());
                        
                    }
                    if (TargetAucInf.CurrentBid > Common.fut15_config.PriceLimit)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  Price has been exceeded Price Limit. Quit Bid.");
                        return false;
                    }
                    if (TargetAucInf.CurrentBid > targetBuyAt)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  Price has been exceeded expected. Quit Bid.");
                        

                        return false;
                    }
                    //4. If it's outbidstate != highest and expires == -1 return false
                    if (TargetAucInf.BidState != "highest" && TargetAucInf.Expires == -1)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  Bid has been expired(0). Quit Bid.");
                        

                        return false;
                    }
                    //4. If it's outbidstate == highest and expires == -1, return true
                    if (TargetAucInf.BidState == "highest" && TargetAucInf.Expires == -1)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  Bid successfully at price " + TargetAucInf.CurrentBid.ToString());
                        

                        return true;
                    }
                    //6. If it's outbidstate == highest and expires != -1 Loop
                    if (TargetAucInf.BidState == "highest" && TargetAucInf.Expires != -1)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "  Keep at the highest price " + TargetAucInf.CurrentBid.ToString());
                        
                    }
                }
                catch (FutErrorException fee)
                {
                    Common.fut15_config.AddLogToListItem(lstLog1, "  " + fee.Message);
                    
                }
            }
        }

        private async void button2_Click_1(object sender, EventArgs e)
        {
            dgPlayers.Rows.Clear();
            var searchParameters = new PlayerSearchParameters
            {
                Page = 1,
                PageSize = 36,
                ResourceId =  Convert.ToInt64(textBox1.Text)
            };
            //1. Get Player list
            //1.1 rating larger than 79
            //1.2 current bid <> 0
            //1.3 max bid > 5000
            //2. Add to Watch list
            await Task.Delay(Common.fut15_config.Bid_Interval);
            searchResponse = await Common.fut15_config.FUT_Client_Facade.SearchAsync(searchParameters);
            CurrentAIList = searchResponse.AuctionInfo.OrderBy(o => o.Expires).ToList();
            foreach (var auctionInfo in CurrentAIList)
            {
                DataGridViewRow row = (DataGridViewRow)dgPlayers.Rows[0].Clone();

                row.Cells[0].Value = auctionInfo.TradeId;
                row.Cells[1].Value = auctionInfo.ItemData.ResourceId;
                row.Cells[2].Value = "";
                var imageBytes = await Common.fut15_config.FUT_Client_Facade.GetPlayerImageAsync(auctionInfo);
                row.Cells[3].Value = imageBytes;
                //row.Cells[2].Value = auctionInfo.ItemData.RareFlag;
                row.Cells[4].Value = auctionInfo.CurrentBid;
                row.Cells[5].Value = auctionInfo.StartingBid;
                row.Cells[6].Value = auctionInfo.BuyNowPrice;
                row.Cells[8].Value = auctionInfo.Expires;
                row.Cells[9].Value = auctionInfo.ItemData.PreferredPosition;
                row.Cells[10].Value = auctionInfo.ItemData.Rating;
                row.Cells[11].Value = auctionInfo.ItemData.RareFlag;
                //row.Cells[10].Value = auctionInfo.ItemData.;
                //row.Cells[11].Value = auctionInfo.ItemData.PreferredPosition;
                //row.Cells[12].Value = auctionInfo.ItemData.PreferredPosition;

                row.Height = 90;
                dgPlayers.Rows.Add(row);
            }
        }

    }
}
