using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace FUT_MonitorCenter
{
    public partial class frmWatchList : Form
    {
        public frmWatchList()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await GetWatchList();
        }

        private async Task GetWatchList()
        {
            await Common.fut15_config.RetrieveWatchlistGridview(dgPlayers);
        }

        private async void btnGetPrice_Click(object sender, EventArgs e)
        {
            List<AuctionInfo> LAI = new List<AuctionInfo>();
            Common.fut15_config.AddLogToListItem(lstLog1, "Start");
            //1. Get Player list from Watchlist
            //2. if it's untradable, save to DB, then remove it from watchlist.
            await Task.Delay(Common.fut15_config.Bid_Interval);
            var watchlistResponse = await Common.fut15_config.FUT_Client_Facade.GetWatchlistAsync();
            if (watchlistResponse == null) return;
            foreach (var auctionInfo in watchlistResponse.AuctionInfo)
            {
                if (auctionInfo.Expires == -1)
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
            Common.fut15_config.AddLogToListItem(lstLog1, "Done");
        }
    }
}
