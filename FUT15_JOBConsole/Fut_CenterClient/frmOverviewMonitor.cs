using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using FUT15_Center;
using FUT_MonitorCenter.Modle;
using System.Configuration;
using System.Diagnostics;
using UltimateTeam.Toolkit.Models;
using FUT15_JOB;
using FUT15_JOB.Models;
using System.Data.Entity;
using System.Runtime.InteropServices;

namespace FUT_MonitorCenter
{
    public partial class frmOverviewMonitor : Form
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        static List<Process> CurrentRunningConsoles = new List<Process>();

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr zeroOnly, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private const int WM_CLOSE = 0x10;
        private const int WM_QUIT = 0x12;

        string sessionid;
        string phishingtoken;
        string platform;

        string ConsoleApp;
        string PricingAppPath;
        string ConsoleBuyContractApp;
        int msGroupRunningInterval;
        FUT15_Center.FUT15_Center Center = new FUT15_Center.FUT15_Center();
        public frmOverviewMonitor()
        {
            InitializeComponent();
        }

        private void frmOverviewMonitor_Load(object sender, EventArgs e)
        {
            ConsoleApp = ConfigurationManager.AppSettings["ConsoleAppPath"].ToString();
            PricingAppPath = ConfigurationManager.AppSettings["PricingAppPath"].ToString();
            ConsoleBuyContractApp = ConfigurationManager.AppSettings["ConsoleBuyContractApp"].ToString();
            msGroupRunningInterval = Convert.ToInt32(ConfigurationManager.AppSettings["GroupRunningInterval"].ToString()) * 1000;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var tt = Center.Database.Database.SqlQuery<MonitorView>("select tua.accountid, tua.userid, tua.loginstatus, tua.jobgroupnumber, tua.credit totalvalue, " +
                                                                    "isnull(acs.CoinsInHand,0) CoinsInHand, isnull(acs.AuctionValue,0) AuctionValue, isnull(acs.LastModified,getdate()) LastModified, isnull(j.jobid,-1) jobid, acs.SessionID, acs.PhishingToken, jp.PlatformName Platform " +
                                                                    "from [TradeUserAccounts] tua " +
                                                                    "left join ( " +
                                                                    " select a.* from [AccountStatus] a, " +
                                                                    " (select AccountID, max(LastModified) md from [AccountStatus] group by AccountID) b " +
                                                                    " where a.AccountID = b.AccountID and a.LastModified = b.md " +
                                                                    ") acs on tua.AccountID = acs.AccountID " +
                                                                    "left join JobPlatforms jp on jp.PlatformID = tua.PlatformID " +
                                                                    "left join [JOBStatus] j on acs.JobID = j.JobID " +
                                                                    "where tua.jobgroupnumber <> 0 " +
                                                                    "order by LastModified desc").ToList();
            dgMonitorView.Rows.Clear();
            foreach (var t in tt)
            {
                DataGridViewRow drow = (DataGridViewRow)dgMonitorView.Rows[0].Clone();
                drow.Cells[0].Value = t.AccountID;
                drow.Cells[1].Value = t.UserID;
                drow.Cells[2].Value = t.LoginStatus;
                drow.Cells[3].Value = t.JobGroupNumber;
                drow.Cells[4].Value = t.TotalValue;
                drow.Cells[5].Value = t.CoinsInHand;
                drow.Cells[6].Value = t.AuctionValue;
                drow.Cells[7].Value = t.LastModified.ToString("yyyy/MM/dd HH:mm:ss");
                drow.Cells[8].Value = t.JobID;
                drow.Cells[9].Value = t.SessionID;
                drow.Cells[10].Value = t.PhishingToken;
                drow.Cells[11].Value = t.Platform;
                dgMonitorView.Rows.Add(drow);
            }
        }

        private void dgMonitorView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(Convert.ToInt32(dgMonitorView.Rows[e.RowIndex].Cells[3].Value) == 0) return;

            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0 && e.ColumnIndex == 16)
            {
                //find out current console window and activate it
                //AppActivate
                string uniqueTitle = "FUT15_JOB_" + dgMonitorView.Rows[e.RowIndex].Cells[3].Value.ToString();
                IntPtr handle = FindWindowByCaption(IntPtr.Zero, uniqueTitle);

                if (handle != IntPtr.Zero)
                {
                    SetForegroundWindow(handle);
                }
                else
                {
                    //run sql
                    Center.Database.Database.ExecuteSqlCommand("update [TradeConfigurations] set value = '' where name = 'ControlCommand' and accountid = " + dgMonitorView.Rows[e.RowIndex].Cells[0].Value.ToString());
                    Center.Database = null;
                    Center.Database = new FUT15_Center.Models.FUT15Entities_CenterDB();

                    string str_grp = dgMonitorView.Rows[e.RowIndex].Cells[3].Value.ToString();
                    ProcessStartInfo ps = new ProcessStartInfo();
                    ps.FileName = ConsoleApp;
                    ps.Arguments = str_grp;
                    Process.Start(ps);
                }

            }
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0 && e.ColumnIndex == 15)
            {
                var record = Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == Common.fut15_config.JobUserAccountStatus.TradeUser.AccountID && a.Name == "ControlCommand");
                record.Value = "";
                Center.Database.SaveChanges();
            }
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0 && e.ColumnIndex == 13)
            {
                var record = Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == Common.fut15_config.JobUserAccountStatus.TradeUser.AccountID && a.Name == "ControlCommand");
                record.Value = "stopbid";
                Center.Database.SaveChanges();
            }
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0 && e.ColumnIndex == 14)
            {
                var record = Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == Common.fut15_config.JobUserAccountStatus.TradeUser.AccountID && a.Name == "ControlCommand");
                record.Value = "exit";
                Center.Database.SaveChanges();
            }
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0 && e.ColumnIndex == 12)
            {
                //Session Log in
                sessionid = dgMonitorView.Rows[e.RowIndex].Cells[9].Value.ToString();
                phishingtoken = dgMonitorView.Rows[e.RowIndex].Cells[10].Value.ToString();
                platform = dgMonitorView.Rows[e.RowIndex].Cells[11].Value.ToString().ToLower();
                string userid = dgMonitorView.Rows[e.RowIndex].Cells[1].Value.ToString();
                int jobid = Convert.ToInt32(dgMonitorView.Rows[e.RowIndex].Cells[8].Value.ToString());
                int accountid = Convert.ToInt32(dgMonitorView.Rows[e.RowIndex].Cells[0].Value.ToString());
                if (platform == "xboxone")
                {
                    Common.fut15_config.FUT_Client_Facade.UpdateToken(sessionid, phishingtoken, Platform.Xbox360);
                }
                else
                    Common.fut15_config.FUT_Client_Facade.UpdateToken(sessionid, phishingtoken, Platform.Pc);
                Common.fut15_config.JobUserAccountStatus = new FUT15_JOB.Models.JobUserAccount(userid, jobid, Common.fut15_config);
                Common.fut15_config.Platform_DB = (new FUT15EntityFactory()).GetFUT15Entity(Center.Database.TradeUserAccounts.FirstOrDefault(a => a.UserID == userid).PlatformID);

                var configs = Center.Database.TradeConfigurations.Where(a => a.AccountID == accountid).ToList();
                dgConfig.Rows.Clear();
                foreach(var config in configs)
                {
                    DataGridViewRow row = (DataGridViewRow)dgConfig.Rows[0].Clone();
                    row.Cells[0].Value = config.AccountID;
                    row.Cells[1].Value = config.Name;
                    row.Cells[2].Value = config.Value;

                    dgConfig.Rows.Add(row);
                }

                lblCurrentUser.Text = userid;

                //Market Adjuster
                Decimal d_adjust;
                var adjust = Center.Database.JobPlatforms.FirstOrDefault(a => a.PlatformID == Common.fut15_config.JobUserAccountStatus.TradeUser.PlatformID);
                if (adjust != null)
                {
                    d_adjust = (Decimal)adjust.MarketAdjuster;
                    trkAdjuster.Value = Convert.ToInt32(d_adjust * (Decimal)100);
                    textBox3.Text = ((decimal)trkAdjuster.Value / (decimal)100).ToString();
                }

                textBox4.Text = Common.fut15_config.JobUserAccountStatus.TradeUser.UserID;
                textBox1.Text = string.Empty;
                button4_Click(this, null);
            }
        }

        private void dgConfig_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                //Update config value
                int accountid = Convert.ToInt32(dgConfig.Rows[e.RowIndex].Cells[0].Value.ToString());
                string name = dgConfig.Rows[e.RowIndex].Cells[1].Value.ToString();
                var config = Center.Database.TradeConfigurations.FirstOrDefault(a => a.AccountID == accountid && a.Name == name);
                if(dgConfig.Rows[e.RowIndex].Cells[2].Value == null)
                    config.Value = "";
                else
                    config.Value = dgConfig.Rows[e.RowIndex].Cells[2].Value.ToString();
                Center.Database.SaveChanges();
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            Common.fut15_config.Center.Database.Database.ExecuteSqlCommand("update [TradeConfigurations] set value = '' where name = 'ControlCommand'");
            ProcessStartInfo ps = new ProcessStartInfo();
            //ps.RedirectStandardOutput = true;
            //ps.UseShellExecute = false;
            string pricingtitle = "FUT15 Pricing Server";
            IntPtr handle_pricing = FindWindowByCaption(IntPtr.Zero, pricingtitle);
            if (handle_pricing != IntPtr.Zero)
            {
                SetForegroundWindow(handle_pricing);
            }
            else
                Process.Start(PricingAppPath);
            var jobs = Center.Database.TradeUserAccounts.Where(a => a.JobGroupNumber != 0).ToList();
            foreach (var job in jobs)
            {
                //find out current console window and activate it
                //AppActivate
                int groupNumber = job.JobGroupNumber;
                string uniqueTitle = "FUT15_JOB_" + groupNumber.ToString();
                IntPtr handle = FindWindowByCaption(IntPtr.Zero, uniqueTitle);

                if (handle != IntPtr.Zero)
                {
                    SetForegroundWindow(handle);
                }
                else
                {
                    //run console
                    string str_grp = groupNumber.ToString();
                    ps.FileName = ConsoleApp;
                    ps.Arguments = str_grp;
                    Process.Start(ps);
                    await Task.Delay(msGroupRunningInterval);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i_adj = trkAdjuster.Value;
            decimal d_adj = (decimal)i_adj / 100;
            var pl = Center.Database.JobPlatforms.FirstOrDefault(a => a.PlatformID == Common.fut15_config.JobUserAccountStatus.TradeUser.PlatformID);
            pl.MarketAdjuster = d_adj;
            Center.Database.SaveChanges();
        }

        private void trkAdjuster_Scroll(object sender, EventArgs e)
        {
            textBox3.Text = ((decimal)trkAdjuster.Value / (decimal)100).ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Common.fut15_config.Platform_DB.Dispose();
            Common.fut15_config.Platform_DB = (new FUT15EntityFactory()).GetFUT15Entity(Common.fut15_config.Center.Database.TradeUserAccounts.FirstOrDefault(a => a.UserID == Common.fut15_config.JobUserAccountStatus.TradeUser.UserID).PlatformID);

            List<TargetPriceView> TPList = new List<TargetPriceView>();

            List<Player> LP = new List<Player>();
            long lRid;
            dgTargetPrice.Rows.Clear();
            //Get auction info data from DB
            if (txtResourceID.Text.Trim() == string.Empty)
            {
                //LP = Common.Platform_DB.Players.ToList();
                TPList = Common.fut15_config.Platform_DB.Database.SqlQuery<TargetPriceView>("select distinct a.id, a.ResourceId, b.name, a.position, d.Rating, d.RareFlag, a.targetbuyat,a.targetsalemin, a.targetsalemax, a.targetsalemargin, " +
                                                                                "(select count(*) from [AuctionInfoes] c, ItemDatas d, TradeTimes tt " +
                                                                                " where c.ItemData_Id=d.Id and d.ResourceId =a.ResourceId and tt.Trade_Id=c.TradeId and d.PreferredPosition=a.Position " +
                                                                                "   and datediff(hh, tt.Transaction_Time, getdate()) < 25 " +
                                                                                ") Count " +
                                                                                "from [dbo].[TargetBidInfoes] a, Players b, [AuctionInfoes] c, ItemDatas d " +
                                                                                "where a.ResourceId = b.resourceID and a.ResourceId=d.ResourceId and c.ItemData_Id=d.Id").ToList();
            }
            else
            {
                lRid = Convert.ToInt64(txtResourceID.Text);
                //LP = Common.Platform_DB.Players.Where(a => a.resourceID == lRid).ToList();
                TPList = Common.fut15_config.Platform_DB.Database.SqlQuery<TargetPriceView>("select distinct a.id, a.ResourceId, b.name, a.position, d.Rating, d.RareFlag, a.targetbuyat,a.targetsalemin, a.targetsalemax, a.targetsalemargin, " +
                                                                                "(select count(*) from [AuctionInfoes] c, ItemDatas d, TradeTimes tt " +
                                                                                " where c.ItemData_Id=d.Id and d.ResourceId =a.ResourceId and tt.Trade_Id=c.TradeId and d.PreferredPosition=a.Position " +
                                                                                "   and datediff(hh, tt.Transaction_Time, getdate()) < 25 " +
                                                                                ") Count " +
                                                                                "from [dbo].[TargetBidInfoes] a, Players b, [AuctionInfoes] c, ItemDatas d " +
                                                                                "where a.resourceid= " + lRid.ToString() + " and a.ResourceId = b.resourceID and a.ResourceId=d.ResourceId and c.ItemData_Id=d.Id").ToList();
            }
            foreach (var tp in TPList)
            {
                DataGridViewRow row = (DataGridViewRow)dgTargetPrice.Rows[0].Clone();
                row.Cells[0].Value = tp.Id;
                row.Cells[1].Value = tp.ResourceID;

                row.Cells[2].Value = tp.Name;
                row.Cells[3].Value = tp.Position;
                row.Cells[4].Value = tp.Rating;
                row.Cells[5].Value = tp.RareFlag;
                row.Cells[6].Value = tp.TargetBuyAt;
                row.Cells[7].Value = tp.TargetSaleMin;
                row.Cells[8].Value = tp.TargetSaleMax;
                row.Cells[9].Value = tp.TargetSaleMargin;
                row.Cells[10].Value = tp.Count;
                dgTargetPrice.Rows.Add(row);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ProcessStartInfo ps = new ProcessStartInfo();
            ps.FileName = ConsoleApp;
            ps.Arguments = textBox2.Text;
            Process.Start(ps);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Center.Database.Database.ExecuteSqlCommand("update [TradeConfigurations] set value = 'exit' where name = 'ControlCommand'");
            Center.Database = null;
            Center.Database = new FUT15_Center.Models.FUT15Entities_CenterDB();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<SaleTracker> lst = new List<SaleTracker>();
            long resourceID;
            if(textBox1.Text != string.Empty)
                resourceID = Convert.ToInt64(textBox1.Text);
            else
                resourceID = 0;
            string userID = textBox4.Text;

            int totalMargin = 0;
            txtTotalMargin.Text = "...";
            dgSaleTrack.Rows.Clear();
            if (resourceID == 0)
            {
                if (userID == string.Empty)
                    lst = Common.fut15_config.Platform_DB.SaleTrackers.OrderByDescending(a => a.BuyTime).ToList();
                else
                    lst = Common.fut15_config.Platform_DB.SaleTrackers.Where(a => a.UserID == userID).OrderByDescending(a => a.BuyTime).ToList();
            }
            else
            {
                if (userID == string.Empty)
                    lst = Common.fut15_config.Platform_DB.SaleTrackers.Where(a => a.ResourceID == resourceID).OrderByDescending(a => a.BuyTime).ToList();
                else
                    lst = Common.fut15_config.Platform_DB.SaleTrackers.Where(a => a.UserID == userID && a.ResourceID == resourceID).OrderByDescending(a => a.BuyTime).ToList();
            }
            
            foreach (var st in lst)
            {
                DataGridViewRow row = (DataGridViewRow)dgSaleTrack.Rows[0].Clone();
                row.Cells[0].Value = st.Id.ToString();

                row.Cells[1].Value = st.ResourceID.ToString();
                row.Cells[2].Value = st.BuyTradeID.ToString();
                row.Cells[3].Value = st.BuyTime.ToString("yyyy/MM/dd HH:mm:ss");
                row.Cells[4].Value = st.BuyPrice.ToString();
                row.Cells[5].Value = st.SoldTradeID.ToString();
                if (st.SoldTime != null)
                {
                    DateTime dt = (DateTime)st.SoldTime;
                    row.Cells[6].Value = dt.ToString("yyyy/MM/dd HH:mm:ss");
                }
                else
                {
                    row.Cells[6].Value = st.SoldTime;
                }
                row.Cells[7].Value = st.SoldPrice.ToString();
                row.Cells[8].Value = st.Margin.ToString();
                row.Cells[9].Value = st.MarginPercent.ToString();
                row.Cells[10].Value = st.SaleStarting.ToString();
                row.Cells[11].Value = st.SaleBuyNow.ToString();

                dgSaleTrack.Rows.Add(row);
                if(st.Margin != null)
                    totalMargin = totalMargin + (int)st.Margin;
            }
            txtTotalMargin.Text = totalMargin.ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            List<HistoryView> historyList = new List<HistoryView>();
            long lRcID;
            int recentRows;
            try
            {
                if (checkBox1.Checked)
                    recentRows = 50;
                else
                    recentRows = 500;

                dgHistory.Rows.Clear();
                //Get auction info data from DB
                if (textBox6.Text.Trim() != string.Empty)
                {
                    lRcID = Convert.ToInt64(textBox6.Text.Trim());
                    historyList = Common.fut15_config.Platform_DB.Database.SqlQuery<HistoryView>("select distinct a.TradeId, c.ResourceId, d.Name,a.BidState,a.CurrentBid, a.StartingBid, a.BuyNowPrice, c.PreferredPosition, c.Rating, c.RareFlag, c.Contract, c.Fitness, b.Transaction_Time " +
                                                                                        "from [dbo].[AuctionInfoes] a, [dbo].[TradeTimes] b, [dbo].[ItemDatas] c, Players d " +
                                                                                        "where a.TradeId = b.Trade_Id " +
                                                                                        "  and c.ResourceId = " + lRcID.ToString() + " " +
                                                                                        "  and a.ItemData_Id = c.id " +
                                                                                        "  and c.ResourceId = d.resourceID " +
                                                                                        "order by b.Transaction_Time DESC").Take(recentRows).ToList();
                }
                else
                {
                    historyList = Common.fut15_config.Platform_DB.Database.SqlQuery<HistoryView>("select distinct a.TradeId, c.ResourceId, d.Name,a.BidState,a.CurrentBid, a.StartingBid, a.BuyNowPrice, c.PreferredPosition, c.Rating, c.RareFlag, c.Contract, c.Fitness, b.Transaction_Time " +
                                                                                        "from [dbo].[AuctionInfoes] a, [dbo].[TradeTimes] b, [dbo].[ItemDatas] c, Players d " +
                                                                                        "where a.TradeId = b.Trade_Id " +
                                                                                        "  and a.ItemData_Id = c.id " +
                                                                                        "  and c.ResourceId = d.resourceID " +
                                                                                        "order by b.Transaction_Time DESC").Take(recentRows).ToList();
                }
                foreach (var historyItem in historyList)
                {
                    DataGridViewRow row = (DataGridViewRow)dgHistory.Rows[0].Clone();

                    row.Cells[0].Value = historyItem.TradeId;
                    row.Cells[1].Value = historyItem.ResourceId;
                    row.Cells[2].Value = historyItem.Name;
                    row.Cells[3].Value = historyItem.BidState;
                    row.Cells[4].Value = historyItem.CurrentBid;
                    row.Cells[5].Value = historyItem.StartingBid;
                    row.Cells[6].Value = historyItem.BuyNowPrice;
                    row.Cells[7].Value = historyItem.PreferredPosition;
                    row.Cells[8].Value = historyItem.Rating;
                    row.Cells[9].Value = historyItem.RareFlag;
                    row.Cells[10].Value = historyItem.Contract;
                    row.Cells[11].Value = historyItem.Fitness;
                    row.Cells[12].Value = historyItem.Transaction_Time.ToString("yyyy/MM/dd HH:mm:ss");

                    dgHistory.Rows.Add(row);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

        }

        private void dgSaleTrack_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                //find out current console window and activate it
                //AppActivate
                long resourceID = Convert.ToInt64(dgSaleTrack.Rows[e.RowIndex].Cells[1].Value);
                txtResourceID.Text = resourceID.ToString();
                textBox6.Text = resourceID.ToString();

                button8_Click(this, null);
                button3_Click(this, null);
            }

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0 && e.ColumnIndex == 12)
            {
                //delete a record
                //
                int id = Convert.ToInt32(dgSaleTrack.Rows[e.RowIndex].Cells[0].Value);
                var rec = Common.fut15_config.Platform_DB.SaleTrackers.FirstOrDefault(a => a.Id == id);
                Common.fut15_config.Platform_DB.SaleTrackers.Remove(rec);
                Common.fut15_config.Platform_DB.SaveChanges();
                button4_Click(this, null);
            }

        }

        private void btnBuyContract_Click(object sender, EventArgs e)
        {
            ProcessStartInfo ps = new ProcessStartInfo();
            ps.FileName = ConsoleBuyContractApp;
            ps.Arguments = txtContractPrice.Text + "," + txtContractAmount.Text + "," + sessionid + "," + phishingtoken + "," + platform + "," + Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + "," + Common.fut15_config.JobUserAccountStatus.UserJobRunningStatus.JobID.ToString();
            Process.Start(ps);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //ps.RedirectStandardOutput = true;
            //ps.UseShellExecute = false;
            //Process.Start(PricingAppPath);
            string pricingtitle = "FUT15 Pricing Server";
            IntPtr handle_pricing = FindWindowByCaption(IntPtr.Zero, pricingtitle);
            if (handle_pricing != IntPtr.Zero)
            {
                SetForegroundWindow(handle_pricing);
                SendMessage(handle_pricing, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
            var jobs = Center.Database.TradeUserAccounts.Where(a => a.JobGroupNumber != 0).ToList();
            foreach (var job in jobs)
            {
                //find out current console window and activate it
                //AppActivate
                int groupNumber = job.JobGroupNumber;
                string uniqueTitle = "FUT15_JOB_" + groupNumber.ToString();
                IntPtr handle = FindWindowByCaption(IntPtr.Zero, uniqueTitle);

                if (handle != IntPtr.Zero)
                {
                    SetForegroundWindow(handle);
                    SendMessage(handle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                }
            }
        }

        private void btnUpdateSession_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgMonitorView.Rows.Count-1; i++)
            {

                int accountid = Convert.ToInt32(dgMonitorView.Rows[i].Cells[0].Value);
                int jobid = Convert.ToInt32(dgMonitorView.Rows[i].Cells[8].Value);
                if (dgMonitorView.Rows[i].Cells[9].Value != null)
                {
                    string sessionid = dgMonitorView.Rows[i].Cells[9].Value.ToString();
                    string phishingtoken = dgMonitorView.Rows[i].Cells[10].Value.ToString();

                    var record = Center.Database.AccountStatuses.FirstOrDefault(a => a.AccountID == accountid && a.JobID == jobid);
                    if (record != null)
                    {
                        record.SessionID = sessionid;
                        record.PhishingToken = phishingtoken;
                        Center.Database.SaveChanges();
                    }
                }
            }

            for (int i = 0; i < dgMonitorView.Rows.Count - 1; i++)
            {

                int accountid = Convert.ToInt32(dgMonitorView.Rows[i].Cells[0].Value);
                var record = Center.Database.TradeUserAccounts.FirstOrDefault(a => a.AccountID == accountid);
                record.JobGroupNumber = Convert.ToInt32(dgMonitorView.Rows[i].Cells[3].Value.ToString());
                Center.Database.SaveChanges();
            }

        }
    }
}
