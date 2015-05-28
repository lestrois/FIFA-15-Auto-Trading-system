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
using FUT_MonitorCenter.Modle;

namespace FUT_MonitorCenter
{
    public partial class Dialog_SelectPlayer_History : Form
    {
        public Dialog_SelectPlayer_History()
        {
            InitializeComponent();
        }

        private void dgPlayers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                //select player and close
                //_currentResourceId = Convert.ToInt64(dgPlayers.Rows[e.RowIndex].Cells[1].Value);
                frmHistory._Player1.resourceID = Convert.ToInt64(dgPlayers.Rows[e.RowIndex].Cells[1].Value);
                frmHistory._Player1.Name = dgPlayers.Rows[e.RowIndex].Cells[2].Value.ToString();

                this.Close();
            }
        }

        private void Dialog_SelectPlayer_History_Load(object sender, EventArgs e)
        {
            dgPlayers.Rows.Clear();
            //Get auction info data from DB
            var Players = Common.fut15_config.Platform_DB.Players.ToList();
            foreach (var player in Players)
            {
                DataGridViewRow row = (DataGridViewRow)dgPlayers.Rows[0].Clone();

                row.Cells[0].Value = player.Id;
                row.Cells[1].Value = player.resourceID;

                row.Cells[2].Value = player.Name;

                //Get AuctionInfo
                //auctionInfo.
                //var imageBytes = await Common.FUT_Client_Facade.GetPlayerImageAsync(auc);
                //row.Cells[3].Value = imageBytes;

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

                row.Height = 30;
                dgPlayers.Rows.Add(row);
            }
        }
    }
}
