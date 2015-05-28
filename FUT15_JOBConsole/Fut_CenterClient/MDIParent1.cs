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
using FUT15_Center.Models;
using FUT15_JOB;
using FUT15_JOB.Models;
using System.Configuration;
using UltimateTeam.Toolkit.Constants;
using UltimateTeam.Toolkit.Exceptions;
using UltimateTeam.Toolkit.Extensions;
using UltimateTeam.Toolkit.Models;
using UltimateTeam.Toolkit.Services;
using UltimateTeam.Toolkit;
using FUT_MonitorCenter.Modle;

namespace FUT_MonitorCenter
{
    public partial class MDIParent1 : Form
    {
        private int childFormNumber = 0;

        public MDIParent1()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(Form1))
                {
                    form.Activate();
                    return;
                }
            }
            quickReloginToolStripMenuItem1.Enabled = false;
            Form1 childForm = new Form1();
            //childForm.MdiParent = this;
            childForm.Text = "Log In " + childFormNumber++;
            childForm.ShowDialog();
            toolStripStatusLabel.Text = Common.fut15_config.JobUserAccountStatus.TradeUser.LoginStatus;
            quickReloginToolStripMenuItem1.Enabled = true;
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            if (Common.fut15_config.FUT_Client_Facade != null)
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(frmPlayerSearch))
                    {
                        form.Activate();
                        return;
                    }
                }
                frmPlayerSearch childForm = new frmPlayerSearch();
                childForm.MdiParent = this;
                childForm.Text = "Window " + childFormNumber++;
                childForm.Show();
            }
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmLoginSession))
                {
                    form.Activate();
                    return;
                }
            }
            frmLoginSession childForm = new frmLoginSession();
            //childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.ShowDialog();
            toolStripStatusLabel.Text = Common.fut15_config.JobUserAccountStatus.TradeUser.LoginStatus;
        }

        private void toolStripLabel4_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmWatchList))
                {
                    form.Activate();
                    return;
                }
            }
            frmWatchList childForm = new frmWatchList();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();

        }

        private void MDIParent1_Load(object sender, EventArgs e)
        {

        }

        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripLabel5_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(PlayerName))
                {
                    form.Activate();
                    return;
                }
            }
            PlayerName childForm = new PlayerName();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void toolStripLabel6_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmHistory))
                {
                    form.Activate();
                    return;
                }
            }
            frmHistory childForm = new frmHistory();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void loginBySessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmLoginSession))
                {
                    form.Activate();
                    return;
                }
            }
            frmLoginSession childForm = new frmLoginSession();
            //childForm.MdiParent = this;
            childForm.Text = "Login By Session " + childFormNumber++;
            childForm.ShowDialog();
            toolStripStatusLabel.Text = Common.fut15_config.JobUserAccountStatus.TradeUser.LoginStatus;
            DisplayUserNameOnTitle();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Common.fut15_config.FUT_Client_Facade != null)
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(frmPlayerSearch))
                    {
                        form.Activate();
                        return;
                    }
                }
                frmPlayerSearch childForm = new frmPlayerSearch();
                childForm.MdiParent = this;
                childForm.Text = "Window " + childFormNumber++;
                childForm.Show();
            }
        }


        private void viewWatchListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmWatchList))
                {
                    form.Activate();
                    return;
                }
            }
            frmWatchList childForm = new frmWatchList();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void playerNameUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(PlayerName))
                {
                    form.Activate();
                    return;
                }
            }
            PlayerName childForm = new PlayerName();
            childForm.MdiParent = this;
            childForm.Text = "Player Name Update " + childFormNumber++;
            childForm.Show();
        }

        private void viewHistoryPricesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmHistory))
                {
                    form.Activate();
                    return;
                }
            }
            frmHistory childForm = new frmHistory();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void toolStripLabel7_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmTargetBids))
                {
                    form.Activate();
                    return;
                }
            }
            frmTargetBids childForm = new frmTargetBids();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void targetBidUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmTargetBids))
                {
                    form.Activate();
                    return;
                }
            }
            frmTargetBids childForm = new frmTargetBids();
            childForm.MdiParent = this;
            childForm.Text = "Target Bid Update " + childFormNumber++;
            childForm.Show();
        }

        private void searchPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmTransferMarket))
                {
                    form.Activate();
                    return;
                }
            }
            frmTransferMarket childForm = new frmTransferMarket();
            childForm.MdiParent = this;
            childForm.Text = "FUT15 Market Search " + childFormNumber++;
            childForm.Show();
        }

        private void transferTargetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmWatchList))
                {
                    form.Activate();
                    return;
                }
            }
            frmWatchList childForm = new frmWatchList();
            childForm.MdiParent = this;
            childForm.Text = "Transfer Targets (Watchlist) " + childFormNumber++;
            childForm.Show();
        }

        private void transferListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmTransferList))
                {
                    form.Activate();
                    return;
                }
            }
            frmTransferList childForm = new frmTransferList();
            childForm.MdiParent = this;
            childForm.Text = "Transfer List " + childFormNumber++;
            childForm.Show();
        }

        private void myClubPlayersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmMyClubPlayers))
                {
                    form.Activate();
                    return;
                }
            }
            frmMyClubPlayers childForm = new frmMyClubPlayers();
            childForm.MdiParent = this;
            childForm.Text = "My Club Players " + childFormNumber++;
            childForm.Show();
        }

        private void myClubSquadsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmMyClubSquads))
                {
                    form.Activate();
                    return;
                }
            }
            frmMyClubSquads childForm = new frmMyClubSquads();
            childForm.MdiParent = this;
            childForm.Text = "My Club Squads " + childFormNumber++;
            childForm.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void autoQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Common.fut15_config.FUT_Client_Facade != null)
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(frmPlayerSearch))
                    {
                        form.Activate();
                        return;
                    }
                }
                frmPlayerSearch childForm = new frmPlayerSearch();
                childForm.MdiParent = this;
                childForm.Text = "Auto Query & Save " + childFormNumber++;
                childForm.Show();
            }
        }

        private void viewHistoryPricesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void autoBidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmJOB_AutoBid))
                {
                    form.Activate();
                    return;
                }
            }
                frmJOB_AutoBid childForm = new frmJOB_AutoBid();
            childForm.MdiParent = this;
            childForm.Text = "Auto Bid " + childFormNumber++;
            childForm.Show();
        }

        private void autoAuctionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach (Form form in Application.OpenForms)
            //{
            //    if (form.GetType() == typeof(frmAutoAuction))
            //    {
            //        form.Activate();
            //        return;
            //    }
            //}
            //frmAutoAuction childForm = new frmAutoAuction();
            //childForm.MdiParent = this;
            //childForm.Text = "Auto Auction " + childFormNumber++;
            //childForm.Show();
        }

        private void autoQuerySinglePlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmQueryPlayerPrices))
                {
                    form.Activate();
                    return;
                }
            }
            frmQueryPlayerPrices childForm = new frmQueryPlayerPrices();
            childForm.MdiParent = this;
            childForm.Text = "Auto Query Single Player & Save " + childFormNumber++;
            childForm.Show();
        }

        private void autoQuickBidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach (Form form in Application.OpenForms)
            //{
            //    if (form.GetType() == typeof(frmJOB_AutoQuickBid))
            //    {
            //        form.Activate();
            //        return;
            //    }
            //}
            //frmJOB_AutoQuickBid childForm = new frmJOB_AutoQuickBid();
            //childForm.MdiParent = this;
            //childForm.Text = "Auto Quick Bid " + childFormNumber++;
            //childForm.Show();
        }

        private async void quickReloginToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                quickReloginToolStripMenuItem1.Enabled = false;
                toolStripStatusLabel.Text = "EA User Loging in. Please Wait...";
                Common.fut15_config.FUT_Client_Facade = null;
                Common.fut15_config.FUT_Client_Facade = new FUTClientFacade(Common.fut15_config);

                var loginResponse = await Common.fut15_config.JobUserAccountStatus.Login();
                Common.fut15_config.JobUserAccountStatus.TradeUser.LoginStatus = ConfigurationSettings.AppSettings["sessionId"] + "  /  " + ConfigurationSettings.AppSettings["phishingToken"];
                toolStripStatusLabel.Text = "EA User Loging in Successfully!";
                quickReloginToolStripMenuItem1.Enabled = true;
                DisplayUserNameOnTitle();
            }
            catch (FutException e1)
            {
                Common.fut15_config.JobUserAccountStatus.TradeUser.LoginStatus = e1.Message.ToString();
                toolStripStatusLabel.Text = Common.fut15_config.JobUserAccountStatus.TradeUser.LoginStatus;
                quickReloginToolStripMenuItem1.Enabled = true;
            }
        }

        private void viewHistoryPricesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmHistory))
                {
                    form.Activate();
                    return;
                }
            }
            frmHistory childForm = new frmHistory();
            childForm.MdiParent = this;
            childForm.Text = "History Prices " + childFormNumber++;
            childForm.Show();
        }

        private void configurationsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void DisplayUserNameOnTitle()
        {
            if (Common.fut15_config.JobUserAccountStatus.TradeUser == null)
            {
                return;
            }
            this.Text = Common.fut15_config.JobUserAccountStatus.TradeUser.UserID + " -- FUT15 Trade";
        }

        private void listAllAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmOverviewMonitor))
                {
                    form.Activate();
                    return;
                }
            }
            frmOverviewMonitor childForm = new frmOverviewMonitor();
            childForm.MdiParent = this;
            childForm.Text = "Accounts Overview " + childFormNumber++;
            childForm.Show();
        }

        private void marketReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(frmMarketReport))
                {
                    form.Activate();
                    return;
                }
            }
            frmMarketReport childForm = new frmMarketReport();
            childForm.MdiParent = this;
            childForm.Text = "Accounts Overview " + childFormNumber++;
            childForm.Show();
        }
    }
}
