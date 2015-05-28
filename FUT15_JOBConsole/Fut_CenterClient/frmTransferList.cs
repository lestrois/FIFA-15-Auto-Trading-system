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
using System.Data.Entity;
using FUT_MonitorCenter.Modle;

namespace FUT_MonitorCenter
{
    public partial class frmTransferList : Form
    {
        List<TargetBidInfo_Enhanced> TBE;
        public static AuctionInfo auctionInfo { get; set; }
        public static Player player { get; set; }

        public frmTransferList()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            Common.fut15_config.AddLogToListItem(lstLog1, " Start retrieving transferlist.");
            
            await Common.fut15_config.RetrieveTransferlistGridview(dgPlayers);
            Common.fut15_config.AddLogToListItem(lstLog1, " End retrieving transferlist.");
            
        }
        
        //Re-List All
        private async void btnGetPrice_Click(object sender, EventArgs e)
        {
            Common.fut15_config.AddLogToListItem(lstLog1, " Start Re-List.");
            
            await Task.Delay(Common.fut15_config.Bid_Interval);
            await Common.fut15_config.FUT_Client_Facade.ReListAsync();
            Common.fut15_config.AddLogToListItem(lstLog1, " End Re-List.");
            
        }

        private void dgPlayers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //var senderGrid = (DataGridView)sender;
            //if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            //{
            //    //Auction
            //    //Get AuctionInfo by TradeId
            //    auctionInfo = Common.Platform_DB.AuctionInfos
            //                            .Include(a => a.ItemData)
            //                            .Include(a => a.ItemData.AttributeList)
            //                            .Include(a => a.ItemData.LifeTimeStats)
            //                            .Include(a => a.ItemData.StatsList)
            //                            .FirstOrDefault(a => a.TradeId == Convert.ToInt64(dgPlayers.Rows[e.RowIndex].Cells[0].Value));
            //    if (auctionInfo.Expires == -1 && auctionInfo.CurrentBid == 0)
            //    {
            //        player = Common.Platform_DB.Players.FirstOrDefault(p => p.resourceID == auctionInfo.ItemData.ResourceId);
            //        //Pop up dialog, ResourceID/Name/StartBid/BuyNowPrice/Duration

            //        frmTransferList_ListAuction childForm = new frmTransferList_ListAuction();
            //        childForm.MdiParent = this;
            //        childForm.ShowDialog();

            //        await Common.RetrieveTransferlistGridview(dgPlayers);
            //    }
            //    else
            //    {
            //        MessageBox.Show("Please do it when it's expired");
            //    }
            //}
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            Common.fut15_config.AddLogToListItem(lstLog1, " Start clearing closed items.");
            
            await Task.Delay(Common.fut15_config.Bid_Interval);
            var watchlistResponse = await Common.fut15_config.FUT_Client_Facade.GetTradePileAsync();
            List<AuctionInfo> SortedList = watchlistResponse.AuctionInfo.OrderBy(o => o.Expires).ToList();
            foreach (var auctionInfo in SortedList)
            {
                if (auctionInfo.Expires == -1 && auctionInfo.CurrentBid != 0)
                {
                    await Task.Delay(Common.fut15_config.Bid_Interval);
                    await Common.fut15_config.FUT_Client_Facade.RemoveFromTradePileAsync(auctionInfo);
                }
            }

            await Common.fut15_config.RetrieveTransferlistGridview(dgPlayers);

            Common.fut15_config.AddLogToListItem(lstLog1, " End clearing closed items.");
            
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            Common.fut15_config.AddLogToListItem(lstLog1, "  Refresh TargetBid List");
            
            Common.fut15_config.Platform_DB.FillTargetBid(TBE, Common.fut15_config);
            Common.fut15_config.AddLogToListItem(lstLog1, "  Refresh TargetBid List Done");
            

            Common.fut15_config.AddLogToListItem(lstLog1, " Start List TargetBid Auctions.");
            
            //6.List new at targetSaleMin/Max
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
                        Common.fut15_config.AddLogToListItem(lstLog1, " Remove ResourceID" + auctionInfo.ItemData.ResourceId.ToString() + "  from transferlist.");
                        
                    }
                    if (auctionInfo.Expires == -1 && auctionInfo.CurrentBid == 0)
                    {
                        var _tbe = TBE.Where(a => a.ResourceId == auctionInfo.ItemData.ResourceId).FirstOrDefault();
                        if (_tbe != null)
                        {
                            await Task.Delay(Common.fut15_config.Bid_Interval);
                            var auctionDetails = new AuctionDetails(auctionInfo.ItemData.Id, AuctionDuration.OneHour, Convert.ToUInt32(_tbe.targetSaleMin), Convert.ToUInt32(_tbe.targetSaleMax));
                            var listAuctionResponse = await Common.fut15_config.FUT_Client_Facade.ListAuctionAsync(auctionDetails);
                            Common.fut15_config.AddLogToListItem(lstLog1, " List Auction ResourceID" + auctionInfo.ItemData.ResourceId.ToString() + "  TargetSaleMin:" + _tbe.targetSaleMin.ToString());
                            
                        }
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
                catch (FutErrorException fee)
                {
                    Common.fut15_config.AddLogToListItem(lstLog1, " " + fee.Message);
                    
                }
            }
            Common.fut15_config.AddLogToListItem(lstLog1, " End List TargetBid Auctions.");
            

        }

        private void frmTransferList_Load(object sender, EventArgs e)
        {
            TBE = new List<TargetBidInfo_Enhanced>();

        }
    }
}
