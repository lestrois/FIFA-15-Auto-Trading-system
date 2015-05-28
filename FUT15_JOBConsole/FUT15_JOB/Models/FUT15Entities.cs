using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using UltimateTeam.Toolkit.Constants;
using UltimateTeam.Toolkit.Exceptions;
using UltimateTeam.Toolkit.Extensions;
using UltimateTeam.Toolkit.Models;
using UltimateTeam.Toolkit.Services;
using UltimateTeam.Toolkit;
using UltimateTeam.Toolkit.Parameters;

namespace FUT15_JOB.Models
{
    public class FUT15Entities : DbContext
    {
        public DbSet<AuctionInfo> AuctionInfos { get; set; }
        public DbSet<ItemData> ItemDatas { get; set; }
        public DbSet<UltimateTeam.Toolkit.Models.Attribute> Attributes { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<TargetBidInfo> TargetBidInfos { get; set; }
        public DbSet<TradeTime> TradeTimes { get; set; }

        public DbSet<WatchListQueue> WatchListQueues { get; set; }
        public DbSet<SaleTracker> SaleTrackers { get; set; }

        public int AddSaleTracker(AuctionInfo auc, FUT15Configs config)
        {
            try
            {
                SaleTracker st = new SaleTracker();
                //if auc exists in table SaleTrackers, return 1
                var _st = this.SaleTrackers.FirstOrDefault(a => a.BuyTradeID == auc.TradeId);
                if (_st != null) return 1;
                st.UserID = config.JobUserAccountStatus.TradeUser.UserID;
                st.ResourceID = auc.ItemData.ResourceId;
                st.BuyTradeID = auc.TradeId;
                st.BuyTime = DateTime.Now;
                st.BuyPrice = Convert.ToInt32(auc.CurrentBid);
                //st.SaleStarting = Convert.ToInt32(auc.StartingBid);
                //st.SaleBuyNow = Convert.ToInt32(auc.BuyNowPrice);

                this.SaleTrackers.Add(st);
                this.SaveChanges();
                return 0;
            }
            catch (Exception ee)
            {
                return -1;
            }
        }

        public int UpdateSaleTrackerStartingBid(long ResourceID, int startingbid, int buynowprice, FUT15Configs config)
        {
            try
            {
                //if auc not exists in table SaleTrackers, return -1
                var _st = this.SaleTrackers.OrderByDescending(a => a.BuyTime).FirstOrDefault(a => a.ResourceID == ResourceID && a.UserID == config.JobUserAccountStatus.TradeUser.UserID && a.SaleStarting == null);
                if (_st == null) return -1;
                //st.SaleStarting = Convert.ToInt32(auc.StartingBid);
                //st.SaleBuyNow = Convert.ToInt32(auc.BuyNowPrice);
                _st.SaleStarting = startingbid;
                _st.SaleBuyNow = buynowprice;
                this.SaveChanges();
                return 0;
            }
            catch (Exception ee)
            {
                return -1;
            }
        }
        public int UpdateSaleTracker(AuctionInfo auc, FUT15Configs config)
        {
            try
            {
                //if auc not exists in table SaleTrackers, return -1
                var _st = this.SaleTrackers.OrderByDescending(a => a.BuyTime).FirstOrDefault(a => a.ResourceID == auc.ItemData.ResourceId && a.UserID == config.JobUserAccountStatus.TradeUser.UserID && a.SoldPrice == null);
                if (_st == null) return -1;
                _st.SoldPrice = Convert.ToInt32(auc.CurrentBid);
                _st.SaleStarting = Convert.ToInt32(auc.StartingBid);
                _st.SaleBuyNow = Convert.ToInt32(auc.BuyNowPrice);
                _st.SoldTime = DateTime.Now;
                _st.SoldTradeID = auc.TradeId;
                _st.Margin = Convert.ToInt32(_st.SoldPrice*0.95) - _st.BuyPrice;
                _st.MarginPercent = Convert.ToInt32(100 * (Convert.ToDecimal((_st.Margin)/Convert.ToDecimal(_st.BuyPrice))));
                this.SaveChanges();
                return 0;
            }
            catch (Exception ee)
            {
                return -1;
            }
        }

        public int AddWatchListQueue(AuctionInfo auc, FUT15Configs config)
        {
            try
            {
                WatchListQueue wq = new WatchListQueue();
                //if it's already added in WatchListQue by other userid, return 2
                //if it's already added by self userid, return 1
                var watchedAuc = this.WatchListQueues.FirstOrDefault(a => a.TradeID == auc.TradeId);
                if (watchedAuc != null)
                {
                    if (watchedAuc.BeingUsedBy == config.JobUserAccountStatus.TradeUser.UserID)
                        return 1;
                    else return 2;
                }

                //if it's not added yet, add a record in watchlistqueue. If it's added successfully, return 0, else return -1
                wq.BeingUsedBy = config.JobUserAccountStatus.TradeUser.UserID;
                wq.LastModified = DateTime.Now;
                wq.ResourceID = auc.ItemData.ResourceId;
                wq.Status = "new";
                wq.TradeID = auc.TradeId;
                this.WatchListQueues.Add(wq);
                this.SaveChanges();
                return 0;
            }
            catch (Exception ee)
            {
                return -1;
            }
        }

        public int UpdateWatchListQueue(List<AuctionInfo> aucList, string status)
        {
            int result = 0;
            int finalResult = 0;
            try
            {
                foreach (var auc in aucList)
                {
                    result = UpdateWatchListQueue(auc, status);
                    if (result != 0)
                        finalResult = 1;
                }
                return finalResult;
            }
            catch (Exception ee)
            {
                return -1;
            }
        }

        public int UpdateWatchListQueue(AuctionInfo auc, string status)
        {
            try
            {
                //if it's not existed, return 1
                var watchedAuc = this.WatchListQueues.FirstOrDefault(a => a.TradeID == auc.TradeId);
                if (watchedAuc == null)
                {
                    return 1;
                }

                //update status
                watchedAuc.Status = status;
                watchedAuc.LastModified = DateTime.Now;

                this.SaveChanges();
                return 0;
            }
            catch (Exception ee)
            {
                return -1;
            }
        }

        public void RunSP(string spName)
        {
            //this.Database.SqlQuery<TargetBidInfo>(spName);
            this.Database.ExecuteSqlCommand(spName);
        }

        public void RunSQL(string SQL)
        {
            this.Database.ExecuteSqlCommand(SQL);
            //this.Database.SqlQuery<TargetBidInfo>(SQL);
        }

        //Fill out target bid list with below logic
        //a) Biddable
        //c) TargetSaleMargin > 10%
        //a.targetSaleMax > 2500 || a.targetSaleMax == 0)
        public void FillTargetBid(List<TargetBidInfo_Enhanced> _TBE, FUT15Configs config)
        {
            _TBE.Clear();
            var platform_db = (new FUT15EntityFactory()).GetFUT15Entity(config.Center.Database.TradeUserAccounts.FirstOrDefault(a => a.UserID == config.JobUserAccountStatus.TradeUser.UserID).PlatformID);
            //Common.Platform_DB.RunSP("UpdateTargetBidInfor");
            var tbl = platform_db.TargetBidInfos.Where(a => (a.targetSaleMax >= config.Low_Price_To_Avoid_Collect || a.targetSaleMax == 0) && a.targetSaleMin >= 1000).ToList();
            foreach (var tb in tbl)
            {
                //count transactions
                int _count = platform_db.AuctionInfos.Include(a => a.ItemData).Include(a => a.ItemData.AttributeList).Include(a => a.ItemData.LifeTimeStats)
                                                       .Include(a => a.ItemData.StatsList).Where(a => a.ItemData.ResourceId == tb.ResourceId && a.ItemData.PreferredPosition == tb.Position).Count();
                TargetBidInfo_Enhanced tbe = new TargetBidInfo_Enhanced(tb);
                tbe.TransactionCount = _count;
                _TBE.Add(tbe);
            }
        }

        //Fill out target bid list with below logic for data collection
        public void FillTargetBid_Collection(List<TargetBidInfo> _TB, FUT15Configs config)
        {
            _TB.Clear();
            var platform_db = (new FUT15EntityFactory()).GetFUT15Entity(config.Center.Database.TradeUserAccounts.FirstOrDefault(a => a.UserID == config.JobUserAccountStatus.TradeUser.UserID).PlatformID);
            //Common.Platform_DB.RunSP("UpdateTargetBidInfor");
            var tbl = platform_db.TargetBidInfos.ToList();
            foreach (var tb in tbl)
            {
                //count transactions
                int _count = platform_db.AuctionInfos.Include(a => a.ItemData).Include(a => a.ItemData.AttributeList).Include(a => a.ItemData.LifeTimeStats)
                                                       .Include(a => a.ItemData.StatsList).Where(a => a.ItemData.ResourceId == tb.ResourceId && a.ItemData.PreferredPosition == tb.Position).Count();
                TargetBidInfo_Enhanced tbe = new TargetBidInfo_Enhanced(tb);
                _TB.Add(tb);
            }
        }

        ////ticket1: Load TargetPlayers func LoadTargetPlayers
        //public void LoadTargetPlayers(List<TargetBidInfo_Enhanced> _TBE, int startPrice, int endPrice, int Margin)
        //{
        //    _TBE.Clear();
        //    Common.Platform_DB.Dispose();
        //    Common.Platform_DB = (new FUT15EntityFactory()).GetFUT15Entity(Common.Center.Database.TradeUserAccounts.FirstOrDefault(a => a.UserID == Common.JobUserAccountStatus.TradeUser.UserID).PlatformID);
        //    //Common.Platform_DB.RunSP("UpdateTargetBidInfor");
        //    var tbl = Common.Platform_DB.TargetBidInfos.Where(a => (a.targetSaleMin >= startPrice && a.targetSaleMin <= endPrice || a.targetSaleMin == 0) && a.targetSaleMargin >= Margin).ToList();
        //    foreach (var tb in tbl)
        //    {
        //        //count transactions
        //        int _count = Common.Platform_DB.AuctionInfos.Include(a => a.ItemData).Include(a => a.ItemData.AttributeList).Include(a => a.ItemData.LifeTimeStats)
        //                                               .Include(a => a.ItemData.StatsList).Where(a => a.ItemData.ResourceId == tb.ResourceId).Count();
        //        TargetBidInfo_Enhanced tbe = new TargetBidInfo_Enhanced(tb);
        //        tbe.TransactionCount = _count;
        //        _TBE.Add(tbe);
        //    }
        //}

        //public void LoadBiddableTargetPlayers(List<TargetBidInfo_Enhanced> _TBE, int startPrice, int endPrice, int Margin)
        //{
        //    _TBE.Clear();
        //    Common.Platform_DB.Dispose();
        //    Common.Platform_DB = (new FUT15EntityFactory()).GetFUT15Entity(Common.Center.Database.TradeUserAccounts.FirstOrDefault(a => a.UserID == Common.JobUserAccountStatus.TradeUser.UserID).PlatformID);
        //    //Common.Platform_DB.RunSP("UpdateTargetBidInfor");
        //    var tbl = Common.Platform_DB.TargetBidInfos.Where(a => (a.targetSaleMin >= startPrice && a.targetSaleMin <= endPrice || a.targetSaleMin == 0) && a.targetSaleMargin >= Margin && a.Biddable == true).ToList();
        //    foreach (var tb in tbl)
        //    {
        //        //count transactions
        //        int _count = Common.Platform_DB.AuctionInfos.Include(a => a.ItemData).Include(a => a.ItemData.AttributeList).Include(a => a.ItemData.LifeTimeStats)
        //                                               .Include(a => a.ItemData.StatsList).Where(a => a.ItemData.ResourceId == tb.ResourceId).Count();
        //        TargetBidInfo_Enhanced tbe = new TargetBidInfo_Enhanced(tb);
        //        tbe.TransactionCount = _count;
        //        _TBE.Add(tbe);
        //    }
        //}

    }
}
