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
using System.Data.Entity;
using FUT_MonitorCenter.Modle;
using UltimateTeam.Toolkit.Models;

namespace FUT_MonitorCenter
{
    public partial class frmHistory : Form
    {
        List<AuctionInfo> auctionInfolist = new List<AuctionInfo>();
        public static Player _Player1 = new Player();
        public frmHistory()
        {
            InitializeComponent();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            long lRcID;
            int recentRows;
            try
            {
                if (checkBox1.Checked)
                    recentRows = 50;
                else
                    recentRows = 500;

                dgPlayers.Rows.Clear();
                //Get auction info data from DB
                if (textBox6.Text.Trim() != string.Empty)
                {
                    lRcID = Convert.ToInt64(textBox6.Text.Trim());
                    auctionInfolist = Common.fut15_config.Platform_DB.AuctionInfos
                                        .Include(a => a.ItemData).Include(a => a.ItemData.AttributeList)
                                        .Include(a => a.ItemData.StatsList).Include(a => a.ItemData.LifeTimeStats)
                                        .Where(a => a.ItemData.ResourceId == lRcID).OrderByDescending(a => a.TradeId).Take(recentRows).ToList();
                }
                else
                {
                    auctionInfolist = Common.fut15_config.Platform_DB.AuctionInfos
                                        .Include(a => a.ItemData)
                                        .Include(a => a.ItemData.AttributeList).Include(a => a.ItemData.StatsList)
                                        .Include(a => a.ItemData.LifeTimeStats)
                                        .OrderByDescending(a => a.TradeId).Take(recentRows).ToList();
                }
                foreach (var auctionInfo in auctionInfolist)
                {
                    DataGridViewRow row = (DataGridViewRow)dgPlayers.Rows[0].Clone();

                    row.Cells[0].Value = auctionInfo.TradeId;
                    row.Cells[1].Value = auctionInfo.ItemData.ResourceId;

                    //var player = (from item in Common.Platform_DB.Players where item.Id == auctionInfo.ItemData.ResourceId select item).ToList();
                    var player = Common.fut15_config.Platform_DB.Players.FirstOrDefault(p => p.resourceID == auctionInfo.ItemData.ResourceId);
                    //Common.Platform_DB.Players.Find(auctionInfo.ItemData.ResourceId);
                    if (player == null)
                    {
                        player = new Player();
                        player.resourceID = auctionInfo.ItemData.ResourceId;
                        player.Name = "";
                    }
                    //Common.Platform_DB.Players.Find(auctionInfo.ItemData.ResourceId);
                    row.Cells[2].Value = player.Name;
                    row.Cells[3].Value = auctionInfo.BidState;
                    row.Cells[4].Value = auctionInfo.CurrentBid;
                    row.Cells[5].Value = auctionInfo.ItemData.LastSalePrice;
                    row.Cells[6].Value = auctionInfo.StartingBid;
                    row.Cells[7].Value = auctionInfo.BuyNowPrice;
                    row.Cells[8].Value = auctionInfo.ItemData.PreferredPosition;
                    row.Cells[9].Value = auctionInfo.ItemData.Rating;
                    row.Cells[10].Value = auctionInfo.ItemData.RareFlag;
                    row.Cells[11].Value = auctionInfo.ItemData.AttributeList[0].Value;
                    row.Cells[12].Value = auctionInfo.ItemData.AttributeList[1].Value;
                    row.Cells[13].Value = auctionInfo.ItemData.AttributeList[2].Value;
                    row.Cells[14].Value = auctionInfo.ItemData.AttributeList[3].Value;
                    row.Cells[15].Value = auctionInfo.ItemData.AttributeList[4].Value;
                    row.Cells[16].Value = auctionInfo.ItemData.AttributeList[5].Value;
                    row.Cells[17].Value = auctionInfo.ItemData.Contract;
                    row.Cells[18].Value = auctionInfo.ItemData.Fitness;

                    row.Height = 25;

                    dgPlayers.Rows.Add(row);



                }
            }
            catch (Exception ee)
            {
            }


            List<Player> LP = new List<Player>();
            long lRid;
            dgTG.Rows.Clear();
            //Get auction info data from DB
            if (textBox6.Text.Trim() == string.Empty)
            {
                LP = Common.fut15_config.Platform_DB.Players.ToList();
            }
            else
            {
                lRid = Convert.ToInt64(textBox6.Text);
                LP = Common.fut15_config.Platform_DB.Players.Where(a => a.resourceID == lRid).ToList();
            }
            foreach (var player in LP)
            {
                DataGridViewRow row = (DataGridViewRow)dgTG.Rows[0].Clone();


                //Get AuctionInfo
                var auctionInfo = Common.fut15_config.Platform_DB.AuctionInfos.Include(a => a.ItemData)
                                                            .Include(a => a.ItemData.AttributeList)
                                                            .Include(a => a.ItemData.LifeTimeStats)
                                                            .Include(a => a.ItemData.StatsList)
                                                            .FirstOrDefault(a => a.ItemData.ResourceId == player.resourceID);
                if (auctionInfo != null)
                {
                    var imageBytes = await Common.fut15_config.FUT_Client_Facade.GetPlayerImageAsync(auctionInfo);
                    row.Cells[0].Value = imageBytes;
                }
                //Get Target bid
                Common.fut15_config.Platform_DB.Dispose();
                Common.fut15_config.Platform_DB = new FUT15Entities();
                var tbi = Common.fut15_config.Platform_DB.TargetBidInfos.FirstOrDefault(p => p.ResourceId == player.resourceID);
                if (tbi != null)
                {
                    row.Cells[1].Value = tbi.NeedAutoUpdate;
                    row.Cells[2].Value = tbi.Biddable;
                    row.Cells[3].Value = tbi.targetBuyAt;

                    row.Cells[4].Value = tbi.targetSaleMin;
                    row.Cells[5].Value = tbi.targetSaleMax;
                    row.Cells[6].Value = tbi.targetSaleMargin;
                }

                var _count = Common.fut15_config.Platform_DB.AuctionInfos.Where(a => a.ItemData.ResourceId == player.resourceID).Count();
                row.Cells[7].Value = _count;

                row.Height = 90;
                dgTG.Rows.Add(row);
            }

        }

        private void frmHistory_Load(object sender, EventArgs e)
        {

        }

        private void dgPlayers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(Dialog_SelectPlayer))
                {
                    form.Activate();
                    return;
                }
            }
            Dialog_SelectPlayer_History childForm = new Dialog_SelectPlayer_History();
            //childForm.MdiParent = this;
            childForm.Text = "Select Player";
            childForm.ShowDialog();

            textBox1.Text = _Player1.Name;
            textBox6.Text = _Player1.resourceID.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            textBox6.Text = string.Empty;
        }
    }
}
