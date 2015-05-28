using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using FUT15_Center.Models;
using UltimateTeam.Toolkit.Constants;
using UltimateTeam.Toolkit.Exceptions;
using UltimateTeam.Toolkit.Extensions;
using UltimateTeam.Toolkit.Models;
using UltimateTeam.Toolkit.Services;
using UltimateTeam.Toolkit;
using MailReader;

namespace FUT15_JOB.Models
{
    public class JobUserAccount
    {
        int JobID;
        public TradeUserAccount TradeUser { get; set; }
        public JobPlatform UserPlatform { get; set; }
        public AccountStatus UserJobRunningStatus { get; set; }
        private FUT15Configs fut15_config;
        public JobUserAccount(string userid, int jobid, FUT15Configs config)
        {
            fut15_config = config;
            TradeUser = fut15_config.Center.Database.TradeUserAccounts.FirstOrDefault(a => a.UserID == userid);
            UserPlatform = fut15_config.Center.Database.JobPlatforms.FirstOrDefault(a => a.PlatformID == TradeUser.PlatformID);
            JobID = jobid;
        }

        public void SessionLogin(string sessionid, string phishingtoken, Platform platform)
        {
            fut15_config.FUT_Client_Facade.UpdateToken(sessionid, phishingtoken, platform);
            UserJobRunningStatus = new AccountStatus();
            UserJobRunningStatus.AccountID = TradeUser.AccountID;
            UserJobRunningStatus.JobID = JobID;
            UserJobRunningStatus.LastModified = DateTime.Now;
            UserJobRunningStatus.PhishingToken = fut15_config.FUT_Client_Facade.fc.RequestFactories.PhishingToken;
            UserJobRunningStatus.SessionID = fut15_config.FUT_Client_Facade.fc.RequestFactories.SessionId;
            UserJobRunningStatus.Status = "Session Logged in";
            UserJobRunningStatus.CoinsInHand = 0;
            UserJobRunningStatus.AuctionValue = 0;
            UserJobRunningStatus.TotalValue = 0;
            fut15_config.Center.Database.AccountStatuses.Add(UserJobRunningStatus);
            fut15_config.Center.Database.SaveChanges();

            var user = fut15_config.Center.Database.TradeUserAccounts.FirstOrDefault(b => b.ID == TradeUser.ID);
            user.LoginStatus = "Session Logged in";
            fut15_config.Center.Database.SaveChanges();
            TradeUser = user;

        }

        public async Task<LoginResponse> Login()
        {
            LoginDetails loginDetails;
            MailReader.MailReader MR = new MailReader.MailReader();
            switch (TradeUser.PlatformID)
            {
                case 1:
                    loginDetails = new LoginDetails(TradeUser.UserID, TradeUser.Password, TradeUser.SecurityAnswer, Platform.Pc, TradeUser.MailPassword, MR);
                    break;
                case 2:
                    loginDetails = new LoginDetails(TradeUser.UserID, TradeUser.Password, TradeUser.SecurityAnswer, Platform.Ps3, TradeUser.MailPassword, MR);
                    break;
                case 3:
                    loginDetails = new LoginDetails(TradeUser.UserID, TradeUser.Password, TradeUser.SecurityAnswer, Platform.Ps3, TradeUser.MailPassword, MR);
                    break;
                case 4:
                    loginDetails = new LoginDetails(TradeUser.UserID, TradeUser.Password, TradeUser.SecurityAnswer, Platform.Xbox360, TradeUser.MailPassword, MR);
                    break;
                case 5:
                    loginDetails = new LoginDetails(TradeUser.UserID, TradeUser.Password, TradeUser.SecurityAnswer, Platform.Xbox360, TradeUser.MailPassword, MR);
                    break;
                default:
                    loginDetails = new LoginDetails(TradeUser.UserID, TradeUser.Password, TradeUser.SecurityAnswer, Platform.Pc, TradeUser.MailPassword, MR);
                    break;
            }
            Thread.Sleep(fut15_config.Bid_Interval);
            //await Task.Delay(fut15_config.Bid_Interval);
            var lR = await fut15_config.FUT_Client_Facade.LoginAsync(loginDetails);

            UserJobRunningStatus = new AccountStatus();
            UserJobRunningStatus.AccountID = TradeUser.AccountID;
            UserJobRunningStatus.JobID = JobID;
            UserJobRunningStatus.LastModified = DateTime.Now;
            UserJobRunningStatus.PhishingToken = fut15_config.FUT_Client_Facade.fc.RequestFactories.PhishingToken;
            UserJobRunningStatus.SessionID = fut15_config.FUT_Client_Facade.fc.RequestFactories.SessionId;
            UserJobRunningStatus.Status = "Logged in";
            UserJobRunningStatus.CoinsInHand = 0;
            UserJobRunningStatus.AuctionValue = 0;
            UserJobRunningStatus.TotalValue = 0;
            fut15_config.Center.Database.AccountStatuses.Add(UserJobRunningStatus);
            fut15_config.Center.Database.SaveChanges();

            var user = fut15_config.Center.Database.TradeUserAccounts.FirstOrDefault(b => b.ID == TradeUser.ID);
            user.LoginStatus = "Logged in";
            fut15_config.Center.Database.SaveChanges();
            TradeUser = user;

            return lR;
        }

        public int UpdateAccountStatus(string status, string phishingtoken = "", string sessionid = "")
        {
            try
            {
                var astatus = fut15_config.Center.Database.AccountStatuses.FirstOrDefault(b => b.Id == UserJobRunningStatus.Id);
                astatus.Status = status;
                astatus.LastModified = DateTime.Now;
                if (phishingtoken != "")
                {
                    astatus.PhishingToken = phishingtoken;
                    astatus.SessionID = sessionid;
                }
                fut15_config.Center.Database.SaveChanges();
                UserJobRunningStatus = astatus;

                var user = fut15_config.Center.Database.TradeUserAccounts.FirstOrDefault(b => b.ID == TradeUser.ID);
                user.LoginStatus = status;
                fut15_config.Center.Database.SaveChanges();
                TradeUser = user;

                return 0;
            }
            catch (Exception ee)
            {
                return -1;
            }
        }

        public int UpdateAccountCredits(int auctionvalue, int coinsinhand)
        {
            try
            {
                var astatus = fut15_config.Center.Database.AccountStatuses.FirstOrDefault(b => b.Id == UserJobRunningStatus.Id);
                astatus.CoinsInHand = coinsinhand;
                astatus.AuctionValue = auctionvalue;
                astatus.TotalValue = coinsinhand + auctionvalue;
                astatus.LastModified = DateTime.Now;
                fut15_config.Center.Database.SaveChanges();
                UserJobRunningStatus = astatus;

                var user = fut15_config.Center.Database.TradeUserAccounts.FirstOrDefault(b => b.ID == TradeUser.ID);
                user.Credit = coinsinhand + auctionvalue;
                fut15_config.Center.Database.SaveChanges();
                TradeUser = user;

                return 0;
            }
            catch (Exception ee)
            {
                return -1;
            }
        }

        public async Task GetCoins()
        {
            uint _credit = 0;
            if (fut15_config.FUT_Client_Facade != null)
            {
                try
                {
                    Thread.Sleep(fut15_config.Bid_Interval);
                    //await Task.Delay(fut15_config.Bid_Interval);
                    var creditsResponse = await fut15_config.FUT_Client_Facade.GetCreditsAsync();
                    _credit = creditsResponse.Credits;
                }
                catch (Exception e1)
                {
                    _credit = 0;
                }
            }
            TradeUser.Credit = Convert.ToInt32(_credit);
        }
    }
}
