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
using System.Configuration;
using FUT15_JOB;
using FUT_MonitorCenter.Modle;

namespace FUT_MonitorCenter
{
    public partial class frmLoginSession : Form
    {
        public frmLoginSession()
        {
            InitializeComponent();
        }

        private void btnSessionLogin_Click(object sender, EventArgs e)
        {
            try
            {
                gbLogin.Enabled = false;
                Common.fut15_config.FUT_Client_Facade.UpdateToken(txtSessionID.Text, txtToken.Text, Platform.Pc);
                //var sessionDetails = new SessionDetails(txtSessionID.Text, txtToken.Text, Platform.Pc);
                //Common.FUT_Client_Facade.LoginSessionIDAsync(sessionDetails);

                gbLogin.Enabled = true;
                lblStatus.Text = "Session Login Successfully.";
                ConfigurationSettings.AppSettings["sessionId"] = txtSessionID.Text.Trim();
                ConfigurationSettings.AppSettings["phishingToken"] = txtToken.Text.Trim();
                
                this.Close();
                Common.fut15_config.JobUserAccountStatus.TradeUser.LoginStatus = txtSessionID.Text + "  /  " + txtToken.Text;

            }
            catch (Exception e1)
            {
                gbLogin.Enabled = true;
                Common.fut15_config.JobUserAccountStatus.TradeUser.LoginStatus = e1.Message.ToString();
                lblStatus.Text = "Session Login Failed.";
            }
        }

        private void frmLoginSession_Load(object sender, EventArgs e)
        {
            txtSessionID.Text = ConfigurationSettings.AppSettings["sessionId"];
            txtToken.Text = ConfigurationSettings.AppSettings["phishingToken"];
            //txtSecondSessionID.Text = ConfigurationSettings.AppSettings["second_sessionId"];
            //txtSecondToken.Text = ConfigurationSettings.AppSettings["second_phishingToken"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    gbLogin.Enabled = false;
            //    Common.FUT_Client = new FutClient();
            //    Common.FUT_Client_Facade.UpdateToken(txtSecondSessionID.Text, txtSecondToken.Text);
            //    gbLogin.Enabled = true;
            //    lblStatus.Text = "Session Login Successfully.";
            //    ConfigurationSettings.AppSettings["second_sessionId"] = txtSecondSessionID.Text.Trim();
            //    ConfigurationSettings.AppSettings["second_phishingToken"] = txtSecondToken.Text.Trim();

            //    this.Close();
            //    Common.LoginStatus = txtSecondSessionID.Text + "  /  " + txtSecondToken.Text;

            //}
            //catch (Exception e1)
            //{
            //    gbLogin.Enabled = true;
            //    Common.LoginStatus = e1.Message.ToString();
            //    lblStatus.Text = "Session Login Failed.";
            //}
        }
    }
}
