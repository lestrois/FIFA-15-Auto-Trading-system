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
    public partial class frmTargetBids : Form
    {
        long _currentResourceId;

        public frmTargetBids()
        {
            InitializeComponent();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            List<Player> LP = new List<Player>();
            long lRid;
            dgPlayers.Rows.Clear();
            //Get auction info data from DB
            if (txtResourceID.Text.Trim() == string.Empty)
            {
                LP = Common.fut15_config.Platform_DB.Players.ToList();
            }
            else
            {
                lRid = Convert.ToInt64(txtResourceID.Text);
                LP = Common.fut15_config.Platform_DB.Players.Where(a => a.resourceID == lRid).ToList();
            }
            foreach (var player in LP)
            {
                DataGridViewRow row = (DataGridViewRow)dgPlayers.Rows[0].Clone();

                row.Cells[0].Value = player.Id;
                row.Cells[1].Value = player.resourceID;

                row.Cells[2].Value = player.Name;

                //Get AuctionInfo
                var auctionInfo = Common.fut15_config.Platform_DB.AuctionInfos.Include(a => a.ItemData)
                                                            .Include(a => a.ItemData.AttributeList)
                                                            .Include(a => a.ItemData.LifeTimeStats)
                                                            .Include(a => a.ItemData.StatsList)
                                                            .FirstOrDefault(a => a.ItemData.ResourceId == player.resourceID);
                if (auctionInfo != null)
                {
                    var imageBytes = await Common.fut15_config.FUT_Client_Facade.GetPlayerImageAsync(auctionInfo);
                    row.Cells[3].Value = imageBytes;
                }
                row.Cells[4].Value = player.Rating;
                row.Cells[5].Value = player.RareFlag;

                //Get Target bid
                Common.fut15_config.Platform_DB.Dispose();
                Common.fut15_config.Platform_DB = new FUT15Entities();
                var tbi = Common.fut15_config.Platform_DB.TargetBidInfos.FirstOrDefault(p => p.ResourceId == player.resourceID);
                if (tbi != null)
                {
                    row.Cells[6].Value = tbi.NeedAutoUpdate;
                    row.Cells[7].Value = tbi.Biddable;
                    row.Cells[8].Value = tbi.targetBuyAt;
                    row.Cells[9].Value = tbi.targetBuyAmount;

                    row.Cells[10].Value = tbi.targetSaleMin;
                    row.Cells[11].Value = tbi.targetSaleMax;
                    row.Cells[12].Value = tbi.targetSaleMargin;
                }

                var _count = Common.fut15_config.Platform_DB.AuctionInfos.Where(a => a.ItemData.ResourceId == player.resourceID).Count();
                row.Cells[13].Value = _count;

                row.Height = 90;
                dgPlayers.Rows.Add(row);
            }
        }

        private void dgPlayers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                //Save Target Bid
                _currentResourceId = Convert.ToInt64(dgPlayers.Rows[e.RowIndex].Cells[1].Value);

                SaveTargetBids(_currentResourceId,
                    Convert.ToBoolean(dgPlayers.Rows[e.RowIndex].Cells[6].Value),
                    Convert.ToBoolean(dgPlayers.Rows[e.RowIndex].Cells[7].Value),
                    Convert.ToInt32(dgPlayers.Rows[e.RowIndex].Cells[8].Value),
                    Convert.ToByte(dgPlayers.Rows[e.RowIndex].Cells[9].Value),
                    Convert.ToInt32(dgPlayers.Rows[e.RowIndex].Cells[10].Value),
                    Convert.ToInt32(dgPlayers.Rows[e.RowIndex].Cells[11].Value)
                    );
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Run SP of calculating target bid and update target bid table
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for( int i = 0; i < dgPlayers.Rows.Count; i++)
                SaveTargetBids(Convert.ToInt64(dgPlayers.Rows[i].Cells[1].Value),
                    Convert.ToBoolean(dgPlayers.Rows[i].Cells[6].Value),
                    Convert.ToBoolean(dgPlayers.Rows[i].Cells[7].Value),
                    Convert.ToInt32(dgPlayers.Rows[i].Cells[8].Value),
                    Convert.ToByte(dgPlayers.Rows[i].Cells[9].Value),
                    Convert.ToInt32(dgPlayers.Rows[i].Cells[10].Value),
                    Convert.ToInt32(dgPlayers.Rows[i].Cells[11].Value)
                    );
        }

        private void SaveTargetBids(long resource_ID, bool _NeedAutoUpdate, bool _Biddable, int _targetBuyAt, byte _targetBuyAmount, int _targetSaleMin, int _targetSaleMax)
        {
            Common.fut15_config.Platform_DB.Dispose();
            Common.fut15_config.Platform_DB = new FUT15Entities();
            var tbi = Common.fut15_config.Platform_DB.TargetBidInfos.FirstOrDefault(p => p.ResourceId == resource_ID);
            if (tbi != null)
            {
                tbi.NeedAutoUpdate = _NeedAutoUpdate;
                tbi.Biddable = _Biddable;
                tbi.targetBuyAt = _targetBuyAt;
                tbi.targetBuyAmount = _targetBuyAmount;
                tbi.targetSaleMin = _targetSaleMin;
                tbi.targetSaleMax = _targetSaleMax;
                if (tbi.targetSaleMin != 0)
                    tbi.targetSaleMargin = 100 * Convert.ToDecimal(tbi.targetSaleMax - tbi.targetSaleMin) / Convert.ToDecimal(tbi.targetSaleMin);
                Common.fut15_config.Platform_DB.SaveChanges();
            }
            else
            {
                tbi = new TargetBidInfo();
                tbi.ResourceId = resource_ID;
                tbi.NeedAutoUpdate = _NeedAutoUpdate;
                tbi.Biddable = _Biddable;
                tbi.targetBuyAt = _targetBuyAt;
                tbi.targetBuyAmount = _targetBuyAmount;
                tbi.targetSaleMin = _targetSaleMin;
                tbi.targetSaleMax = _targetSaleMax;
                if (tbi.targetSaleMin != 0)
                    tbi.targetSaleMargin = 100 * Convert.ToDecimal(tbi.targetSaleMax - tbi.targetSaleMin) / Convert.ToDecimal(tbi.targetSaleMin);
                Common.fut15_config.Platform_DB.TargetBidInfos.Add(tbi);
                Common.fut15_config.Platform_DB.SaveChanges();
            }

        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //Test Entity framwork SP running
            //List<TargetBidInfo> TBE_BiddableTargetPlayers = new List<TargetBidInfo>();

            //TBE_BiddableTargetPlayers.Clear();
            //Common.Platform_DB.Dispose();
            //Common.Platform_DB = new FUT15Entities();
            ////Common.Platform_DB.RunSP("UpdateTargetBidInfor");
            //var tbl = Common.Platform_DB.TargetBidInfos.Where(a => (a.targetSaleMin >= Common.TargetPriceStart
            //                                                && a.targetSaleMin <= Common.TargetPriceEnd || a.targetSaleMin == 0)
            //                                                && a.Biddable == true)
            //                                                .ToList();
            //foreach (var tb in tbl)
            //{
            //    //count transactions
            //    if(tb.ResourceId == 1879225195)
            //        TBE_BiddableTargetPlayers.Add(tb);
            //}

        }
    }
}
