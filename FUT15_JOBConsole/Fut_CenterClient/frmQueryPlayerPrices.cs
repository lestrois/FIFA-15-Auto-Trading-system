using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
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
using FUT_MonitorCenter.Modle;

namespace FUT_MonitorCenter
{
    public partial class frmQueryPlayerPrices : Form
    {
        private int _count;
        private int _cycle;
        private long _resourceId;

        public frmQueryPlayerPrices()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            _resourceId = Convert.ToInt64(textBox1.Text.Trim());
            if (button1.Text == "Start")
            {
                _count = 0;
                _cycle = 0;
                dgPlayers.Rows.Clear();
                button1.Text = "Stop";
                int milliseconds = Convert.ToInt32(ConfigurationSettings.AppSettings["query_interval"]);
                while (button1.Text == "Stop" && _count < 51)
                {
                    _cycle++;
                    Common.fut15_config.AddLogToListItem(lstLog1, _cycle.ToString() + ".Query Players Started");
                    //1. Search England Leagues
                    await SearchPlayer(1, 36, _resourceId);
                    Common.fut15_config.AddLogToListItem(lstLog1, _cycle.ToString() + ".Query Players Done");
                    Common.fut15_config.AddLogToListItem(lstLog1, _cycle.ToString() + ".Waiting for " + (milliseconds / 1000).ToString() + " seconds");
                    

                    await SearchPlayer(2, 36, _resourceId);
                    Common.fut15_config.AddLogToListItem(lstLog1, _cycle.ToString() + ".Query Players Done");
                    Common.fut15_config.AddLogToListItem(lstLog1, _cycle.ToString() + ".Waiting for " + (milliseconds / 1000).ToString() + " seconds");
                    

                    await SearchPlayer(3, 36, _resourceId);
                    Common.fut15_config.AddLogToListItem(lstLog1, _cycle.ToString() + ".Query Players Done");
                    Common.fut15_config.AddLogToListItem(lstLog1, _cycle.ToString() + ".Waiting for " + (milliseconds / 1000).ToString() + " seconds");
                    

                    //Save to DB for expired ones.
                    //1. Get Player list from Watchlist
                    //2. if it's untradable, save to DB, then remove it from watchlist.
                    Common.fut15_config.AddLogToListItem(lstLog1, "Saving Watchlist Started");
                    
                    await Task.Delay(Common.fut15_config.Bid_Interval);
                    var watchlistResponse = await Common.fut15_config.FUT_Client_Facade.GetWatchlistAsync();
                    if (watchlistResponse == null) return;
                    List<AuctionInfo> LAI = new List<AuctionInfo>();
                    foreach (var auctionInfo in watchlistResponse.AuctionInfo)
                    {
                        if ((auctionInfo.TradeState == "expired" || auctionInfo.TradeState == "closed") && auctionInfo.ItemData.ResourceId == _resourceId)
                        {
                            Common.fut15_config.SaveAuctionInfo(auctionInfo);
                            Common.fut15_config.AddLogToListItem(lstLog1, "Trade " + auctionInfo.TradeId.ToString() + " Price (ResrouceID: " + auctionInfo.ItemData.ResourceId.ToString() + ") saved");
                            

                            //Remove it from watch list;
                            LAI.Add(auctionInfo);
                            Common.fut15_config.AddLogToListItem(lstLog1, "Trade " + auctionInfo.TradeId.ToString() + " Price (ResrouceID: " + auctionInfo.ItemData.ResourceId.ToString() + ") removed from watchlist");
                            
                        }
                    }
                    if (LAI.Count != 0)
                    {
                        await Task.Delay(Common.fut15_config.Bid_Interval);
                        await Common.fut15_config.FUT_Client_Facade.RemoveFromWatchlistAsync(LAI);
                    }

                    Common.fut15_config.AddLogToListItem(lstLog1, "Saving Watchlist Done");

                    Common.fut15_config.AddLogToListItem(lstLog1, "Couting Watchlist count ... ");
                    
                    await Task.Delay(Common.fut15_config.Bid_Interval);
                    watchlistResponse = await Common.fut15_config.FUT_Client_Facade.GetWatchlistAsync();
                    _count = watchlistResponse.AuctionInfo.Count;
                    Common.fut15_config.AddLogToListItem(lstLog1, "Watchlist count is " + _count.ToString() + " ... ");
                    
                    if (_count >= 50)
                    {
                        Common.fut15_config.AddLogToListItem(lstLog1, "Watchlist is full. Waiting for 5 minutes ... ");
                        
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


        private async Task SearchPlayer(uint pageNumber, byte pageSize, long resourceId)
        {
            var searchParameters = new PlayerSearchParameters
            {
                Page = pageNumber,
                PageSize = pageSize,
                ResourceId = resourceId
            };
            //1. Get Player list
            //1.1 rating larger than 79
            //1.2 current bid <> 0
            //1.3 max bid > 5000
            //2. Add to Watch list
            await Task.Delay(Common.fut15_config.Query_Interval);
            var searchResponse = await Common.fut15_config.FUT_Client_Facade.SearchAsync(searchParameters);
            Common.fut15_config.AddLogToListItem(lstLog1, searchResponse.AuctionInfo.Count + " Players found");
            foreach (var auctionInfo in searchResponse.AuctionInfo)
            {
                if (auctionInfo.Watched != true && auctionInfo.CurrentBid != 0)
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
                    Common.fut15_config.AddLogToListItem(lstLog1, "Player: " + auctionInfo.ItemData.ResourceId + " CurrentBid:" + auctionInfo.CurrentBid.ToString() + " added to watch list");
                    
                    Common.fut15_config.AddLogToListItem(lstLog1, "    Player " + auctionInfo.ItemData.ResourceId.ToString() + " added into Watchlist");
                    
                }
            }
        }
    }
}
