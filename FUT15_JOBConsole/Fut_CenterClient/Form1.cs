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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtUser.Text = Common.fut15_config.JobUserAccountStatus.TradeUser.UserID;
            txtPassword.Text = Common.fut15_config.JobUserAccountStatus.TradeUser.Password;
            txtAnswer.Text = Common.fut15_config.JobUserAccountStatus.TradeUser.SecurityAnswer;
            txtSessionID.Text = "";
            txtToken.Text = "";
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                gbLogin.Enabled = false;
                var loginResponse = await Common.fut15_config.JobUserAccountStatus.Login();

                txtSessionID.Text = loginResponse.SessionId;
                txtToken.Text = loginResponse.PhishingToken;
                Common.fut15_config.JobUserAccountStatus.TradeUser.LoginStatus = txtSessionID.Text + "  /  " + txtToken.Text;
                gbLogin.Enabled = true;

                ConfigurationSettings.AppSettings["sessionId"] = txtSessionID.Text.Trim();
                ConfigurationSettings.AppSettings["phishingToken"] = txtToken.Text.Trim();

                this.Close();
            }
            catch (FutException e1)
            {
                txtSessionID.Text = e1.Message.ToString();
                txtToken.Text = "";
                gbLogin.Enabled = true;
                Common.fut15_config.JobUserAccountStatus.TradeUser.LoginStatus = e1.Message.ToString();
            }
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
            }
            catch (Exception e1)
            {
                txtSessionID.Text = e1.Message.ToString();
                txtToken.Text = "";
                gbLogin.Enabled = true;
                Common.fut15_config.JobUserAccountStatus.TradeUser.LoginStatus = e1.Message.ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
