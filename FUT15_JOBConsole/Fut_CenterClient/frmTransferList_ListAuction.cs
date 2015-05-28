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
using FUT_MonitorCenter.Modle;
using System.IO;
using FUT_MonitorCenter.Modle;

namespace FUT_MonitorCenter
{
    public partial class frmTransferList_ListAuction : Form
    {
        public frmTransferList_ListAuction()
        {
            InitializeComponent();
        }

        private async void frmTransferList_ListAuction_Load(object sender, EventArgs e)
        {
            //Pop up dialog, ResourceID/Name/StartBid/BuyNowPrice/Duration
            textBox1.Text = frmTransferList.auctionInfo.ItemData.ResourceId.ToString();
            textBox1.Text = frmTransferList.player.Name;
            textBox1.Text = frmTransferList.auctionInfo.StartingBid.ToString();
            textBox1.Text = frmTransferList.auctionInfo.BuyNowPrice.ToString();

            ComboBoxItem_AuctionDuration item = new ComboBoxItem_AuctionDuration();
            item.Text = "OneHour";
            item.Value = AuctionDuration.OneHour;
            comboBox1.Items.Add(item);

            item.Text = "ThreeHours";
            item.Value = AuctionDuration.ThreeDays;
            comboBox1.Items.Add(item);

            item.Text = "SixHours";
            item.Value = AuctionDuration.SixHours;
            comboBox1.Items.Add(item);

            item.Text = "TwelveHours";
            item.Value = AuctionDuration.TwelveHours;
            comboBox1.Items.Add(item);

            item.Text = "OneDay";
            item.Value = AuctionDuration.OneDay;
            comboBox1.Items.Add(item);

            item.Text = "ThreeDays";
            item.Value = AuctionDuration.ThreeDays;
            comboBox1.Items.Add(item);

            comboBox1.SelectedIndex = 0;

            //pictureBox1

            var imageBytes = await Common.fut15_config.FUT_Client_Facade.GetPlayerImageAsync(frmTransferList.auctionInfo);
            MemoryStream mStream = new MemoryStream();
            mStream.Write(imageBytes, 0, Convert.ToInt32(imageBytes.Length));
            Bitmap bm = new Bitmap(mStream, false);

            pictureBox1.Image = bm;
            mStream.Dispose();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //List New Auction
            //AuctionDuration ad = comboBox1.SelectedItem;
            var auctionDetails = new AuctionDetails(frmTransferList.auctionInfo.ItemData.Id, AuctionDuration.OneHour, Convert.ToUInt32(textBox3.Text), Convert.ToUInt32(textBox4.Text));
            await Task.Delay(Common.fut15_config.Bid_Interval); 
            var listAuctionResponse = await Common.fut15_config.FUT_Client_Facade.ListAuctionAsync(auctionDetails);

            this.Close();
        }
    }
}
