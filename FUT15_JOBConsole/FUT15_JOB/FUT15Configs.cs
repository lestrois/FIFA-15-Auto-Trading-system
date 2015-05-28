using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UltimateTeam.Toolkit.Constants;
using UltimateTeam.Toolkit.Exceptions;
using UltimateTeam.Toolkit.Extensions;
using UltimateTeam.Toolkit.Models;
using UltimateTeam.Toolkit.Services;
using UltimateTeam.Toolkit;
using FUT15_JOB.Models;
using FUT15_Center.Models;
using FUT15_Center;
using System.Windows.Forms;

namespace FUT15_JOB
{
    public class FUT15Configs
    {
        public FUT15_Center.FUT15_Center Center { get; set; }
        public FUTClientFacade FUT_Client_Facade { get; set; }
        public FUT15Entities Platform_DB { get; set; }

        public JobUserAccount JobUserAccountStatus { get; set; }
        public List<SalePrice> SalePrices { get; set; }
        public Logger logger { get; set; }
        public int Query_Interval;
        public int Bid_Interval;
        public int Relist_Interval_Mins;
        public int Max_Singleplayers_Tobid;
        public int CoinsLimit { get; set; }
        public int Expired_Hour { get; set; }
        public int Expired_Seconds { get; set; }
        public int Bid_In_Seconds { get; set; }
        public int Urgent_Bid_In_Seconds { get; set; }
        public int BID_Coins { get; set; }
        public string Enable_BIN { get; set; }
        public string Enable_Login_App_Start { get; set; }
        public int Low_Price_To_Avoid_Collect { get; set; }
        public int Log_Level { get; set; }
        public int PriceLimit { get; set; }
        public int AuctionValue { get; set; }
        public int CoinsInHand { get; set; }
        public int TotalValue { get; set; }
        public string StopAutoRelist { get; set; }
        public string AutoListLatestTargetPrice { get; set; }
        public string DataCollectionWhileBidding { get; set; }
        public string ControlCommand { get; set; }//1. ""; 2. "exit"; 3. "suspend"
        public int RunningHours { get; set; }
        public int SuspendMinutes { get; set; }
        public int TradePileSize { get; set; }

        public int SendEmail(string email, string subject, string body)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(email);
                message.Subject = subject;
                message.From = new System.Net.Mail.MailAddress(JobUserAccountStatus.TradeUser.UserID);
                message.Body = body;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.Send(message);
                return 0;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public async void SaveExpiredClosedInWatchlist()
        {
            try
            {
                //1. Get Player list from Watchlist
                //2. if it's untradable, save to DB, then remove it from watchlist.
                List<AuctionInfo> LAI = new List<AuctionInfo>();
                Thread.Sleep(Bid_Interval);
                //await Task.Delay(Bid_Interval);
                var watchlistResponse = await FUT_Client_Facade.GetWatchlistAsync();
                if (watchlistResponse == null) return;
                foreach (var auctionInfo in watchlistResponse.AuctionInfo)
                {
                    if (auctionInfo.Expires == -1)
                    {
                        SaveAuctionInfo(auctionInfo);
                        //Remove it from watch list;
                        LAI.Add(auctionInfo);
                    }
                }
                if (LAI.Count != 0)
                {
                    Thread.Sleep(Bid_Interval);
                    //await Task.Delay(Bid_Interval);
                    await FUT_Client_Facade.RemoveFromWatchlistAsync(LAI);
                }

            }
            catch (Exception e1)
            {
                //MessageBox.Show(e1.Message);
            }

        }

        public void AddLogToListItem(ListView lstLog, string Message, int OptionalLevel = 5)
        {
            if (OptionalLevel > Log_Level)
            {
                lstLog.Items.Add(DateTime.Now.ToString() + ": " + Message);
                lstLog.EnsureVisible(lstLog.Items.Count - 1);
            }
        }
        
        public void MessageDisplay(string Message, int OptionalLevel = 5)
        {
            if (OptionalLevel > Log_Level)
            {
                //lstLog.Items.Add(DateTime.Now.ToString() +": " + Message);
                //lstLog.EnsureVisible(lstLog.Items.Count - 1);
                Console.WriteLine("(Grp:" + JobUserAccountStatus.TradeUser.JobGroupNumber + " Acct:" + JobUserAccountStatus.TradeUser.AccountID.ToString() + ") " + Message);
                logger.WriteLog(LogLevelL4N.INFO,
                                "(Grp:" + JobUserAccountStatus.TradeUser.JobGroupNumber + " Acct:" + JobUserAccountStatus.TradeUser.AccountID.ToString() + ") " + Message);
            }
        }

        public void AddLogToFile(string Message, int OptionalLevel = 5)
        {
            if (OptionalLevel > Log_Level)
            {
                //lstLog.Items.Add(DateTime.Now.ToString() + ": " + Message);
                //lstLog.EnsureVisible(lstLog.Items.Count - 1);
            }
        }

        public async Task RetrieveWatchlistGridview(DataGridView dg)
        {
            try
            {
                BID_Coins = 0;
                dg.Rows.Clear();
                Thread.Sleep(Bid_Interval);
                //await Task.Delay(Bid_Interval);
                var watchlistResponse = await FUT_Client_Facade.GetWatchlistAsync();
                List<AuctionInfo> SortedList = watchlistResponse.AuctionInfo.OrderBy(o => o.Expires).ToList();
                foreach (var auctionInfo in SortedList)
                {
                    try
                    {
                        DataGridViewRow row = (DataGridViewRow)dg.Rows[0].Clone();

                        row.Cells[0].Value = auctionInfo.TradeId;
                        row.Cells[1].Value = auctionInfo.ItemData.ResourceId;
                        var player = Platform_DB.Players.FirstOrDefault(a => a.resourceID == auctionInfo.ItemData.ResourceId);
                        if (player != null)
                        {
                            row.Cells[2].Value = player.Name;
                        }
                        else
                        {
                            row.Cells[2].Value = "";
                        }
                        //await Task.Delay(Common.Bid_Interval);
                        var imageBytes = await FUT_Client_Facade.GetPlayerImageAsync(auctionInfo);
                        row.Cells[3].Value = imageBytes;
                        row.Cells[4].Value = auctionInfo.BidState;
                        row.Cells[5].Value = auctionInfo.Expires;

                        row.Cells[6].Value = auctionInfo.CurrentBid;
                        row.Cells[7].Value = auctionInfo.StartingBid;
                        row.Cells[8].Value = auctionInfo.BuyNowPrice;
                        row.Cells[9].Value = auctionInfo.ItemData.PreferredPosition;
                        row.Cells[10].Value = auctionInfo.ItemData.Rating;
                        row.Cells[11].Value = auctionInfo.ItemData.RareFlag;
                        row.Cells[12].Value = auctionInfo.ItemData.Contract;
                        row.Cells[13].Value = auctionInfo.ItemData.Fitness;
                        row.Height = 90;
                        dg.Rows.Add(row);
                        //if auctionInfo.Bidstate is highest add to BID_Coins
                        if (auctionInfo.BidState == "highest")
                            BID_Coins = BID_Coins + Convert.ToInt32(auctionInfo.CurrentBid);
                    }
                    catch (Exception EE)
                    {
                    }
                }
            }
            catch (Exception e1)
            {

            }



        }

        public async Task RetrieveTransferlistGridview(DataGridView dg, Label lbl1 = null, Label lbl2 = null, Label lbl3 = null)
        {
            AuctionValue = 0;
            dg.Rows.Clear();
            Thread.Sleep(Bid_Interval);
            //await Task.Delay(Bid_Interval);
            var watchlistResponse = await FUT_Client_Facade.GetTradePileAsync();
            List<AuctionInfo> SortedList = watchlistResponse.AuctionInfo.OrderBy(o => o.Expires).ToList();
            foreach (var auctionInfo in SortedList)
            {
                try
                {
                    DataGridViewRow row = (DataGridViewRow)dg.Rows[0].Clone();

                    row.Cells[0].Value = auctionInfo.TradeId;
                    row.Cells[1].Value = auctionInfo.ItemData.ResourceId;
                    var player = Platform_DB.Players.FirstOrDefault(a => a.resourceID == auctionInfo.ItemData.ResourceId);
                    if (player != null)
                    {
                        row.Cells[2].Value = player.Name;
                    }
                    else
                    {
                        row.Cells[2].Value = "";
                    }
                    var imageBytes = await FUT_Client_Facade.GetPlayerImageAsync(auctionInfo);
                    if (imageBytes != null)
                    {
                        row.Cells[3].Value = imageBytes;
                    }
                    row.Cells[4].Value = auctionInfo.BidState;
                    row.Cells[5].Value = auctionInfo.Expires;

                    row.Cells[6].Value = auctionInfo.CurrentBid;
                    row.Cells[7].Value = auctionInfo.StartingBid;
                    row.Cells[8].Value = auctionInfo.BuyNowPrice;
                    row.Cells[9].Value = auctionInfo.ItemData.PreferredPosition;
                    row.Cells[10].Value = auctionInfo.ItemData.Rating;
                    row.Cells[11].Value = auctionInfo.ItemData.RareFlag;
                    row.Cells[12].Value = auctionInfo.ItemData.Contract;
                    row.Cells[13].Value = auctionInfo.ItemData.Fitness;
                    row.Height = 90;
                    dg.Rows.Add(row);

                    if (auctionInfo.Expires != -1 || (auctionInfo.Expires == -1 && auctionInfo.CurrentBid == 0))
                    {
                        AuctionValue = AuctionValue + Convert.ToInt32((auctionInfo.CurrentBid == 0) ? auctionInfo.StartingBid : auctionInfo.CurrentBid);
                    }
                }
                catch (Exception EE)
                {

                }
            }
            if (lbl1 != null)
            {
                AuctionValue = Convert.ToInt32(Convert.ToDouble(AuctionValue) * 0.95);
                lbl1.Text = AuctionValue.ToString();
                lbl3.Text = (Convert.ToInt32(lbl2.Text) + Convert.ToInt32(lbl1.Text)).ToString();
            }
        }
        public void SaveAuctionInfo(AuctionInfo auctionInfo)
        {
            try
            {
                //Save Trade to DB
                //Common.FUT_DB.Attributes.Add(auctionInfo.ItemData.AttributeList);
                var auc = Platform_DB.AuctionInfos.FirstOrDefault(p => p.TradeId == auctionInfo.TradeId);
                if (auc == null)
                {
                    foreach (var attribute in auctionInfo.ItemData.AttributeList)
                    {
                        Platform_DB.Attributes.Add(attribute);
                        Platform_DB.SaveChanges();
                    };
                    foreach (var lifetime in auctionInfo.ItemData.LifeTimeStats)
                    {
                        Platform_DB.Attributes.Add(lifetime);
                        Platform_DB.SaveChanges();
                    };
                    foreach (var state in auctionInfo.ItemData.StatsList)
                    {
                        Platform_DB.Attributes.Add(state);
                        Platform_DB.SaveChanges();
                    };
                    Platform_DB.ItemDatas.Add(auctionInfo.ItemData);
                    Platform_DB.AuctionInfos.Add(auctionInfo);
                    Platform_DB.SaveChanges();
                    //Query Player by resourceID
                    //If player is not in DB yet, Save player to DB
                    //Player p = new Player();
                    //p.resourceID = auctionInfo.ItemData.ResourceId;
                    //Common.FUT_DB.Players.Add(p);
                    //Common.FUT_DB.SaveChanges();
                    var player = Platform_DB.Players.FirstOrDefault(p => p.resourceID == auctionInfo.ItemData.ResourceId);
                    if (player == null)
                    {
                        player = new Player();
                        player.resourceID = auctionInfo.ItemData.ResourceId;
                        player.Name = "";
                        player.PreferredPosition = auctionInfo.ItemData.PreferredPosition;
                        player.Rating = auctionInfo.ItemData.Rating;
                        player.RareFlag = auctionInfo.ItemData.RareFlag;
                        player.PAC = auctionInfo.ItemData.AttributeList[0].Value;
                        player.SHO = auctionInfo.ItemData.AttributeList[1].Value;
                        player.PAS = auctionInfo.ItemData.AttributeList[2].Value;
                        player.DRI = auctionInfo.ItemData.AttributeList[3].Value;
                        player.DEF = auctionInfo.ItemData.AttributeList[4].Value;
                        player.PHY = auctionInfo.ItemData.AttributeList[5].Value;

                        Platform_DB.Players.Add(player);
                        Platform_DB.SaveChanges();
                    }

                    //Save TradeTime
                    var tt = new TradeTime();
                    tt.Trade_Id = auctionInfo.TradeId;
                    tt.Transaction_Time = DateTime.Now;
                    Platform_DB.TradeTimes.Add(tt);
                    Platform_DB.SaveChanges();

                }
            }
            catch (Exception e1)
            {
                MessageDisplay(e1.Message);
            }

        }
    }
}
