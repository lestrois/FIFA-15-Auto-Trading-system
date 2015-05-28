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
    public partial class PlayerName : Form
    {
        public PlayerName()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
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

                    //Get AuctionInfo
                    //auctionInfo.
                    var auc = Common.fut15_config.Platform_DB.AuctionInfos.Include(a => a.ItemData)
                        .Include(a => a.ItemData.AttributeList)
                        .Include(a => a.ItemData.StatsList)
                        .Include(a => a.ItemData.LifeTimeStats)
                        .FirstOrDefault(p => p.ItemData.ResourceId == player.resourceID);
                    if (auc != null)
                    {
                        var imageBytes = await Common.fut15_config.FUT_Client_Facade.GetPlayerImageAsync(auc);
                        row.Cells[2].Value = imageBytes;
                    }

                    row.Cells[3].Value = player.Name;

                    row.Height = 90;
                    dgPlayers.Rows.Add(row);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

        }

        private void PlayerName_Load(object sender, EventArgs e)
        {

        }

        private void dgPlayers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                //Save Player name
                var pp = Common.fut15_config.Platform_DB.Players.Find(dgPlayers.Rows[e.RowIndex].Cells[0].Value);
                if (dgPlayers.Rows[e.RowIndex].Cells[3].Value == null)
                {
                    pp.Name = "";
                }
                else
                {
                    pp.Name = dgPlayers.Rows[e.RowIndex].Cells[3].Value.ToString();
                }

                Common.fut15_config.Platform_DB.SaveChanges();
            }

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            dgPlayers.Rows.Clear();
            //Get auction info data from DB
            var Players = Common.fut15_config.Platform_DB.Players.Where(p => (p.Name == null || p.Name == "")).ToList();
            foreach (var player in Players)
            {
                DataGridViewRow row = (DataGridViewRow)dgPlayers.Rows[0].Clone();

                row.Cells[0].Value = player.Id;
                row.Cells[1].Value = player.resourceID;
                
                //Get AuctionInfo
                //auctionInfo.
                var auc = Common.fut15_config.Platform_DB.AuctionInfos.Include(a => a.ItemData)
                    .Include(a => a.ItemData.AttributeList)
                    .Include(a => a.ItemData.StatsList)
                    .Include(a => a.ItemData.LifeTimeStats)
                    .FirstOrDefault(p => p.ItemData.ResourceId == player.resourceID);
                var imageBytes = await Common.fut15_config.FUT_Client_Facade.GetPlayerImageAsync(auc);
                row.Cells[2].Value = imageBytes;

                row.Cells[3].Value = player.Name;

                row.Height = 90;
                dgPlayers.Rows.Add(row);
            }
        }

    }
}
