using UltimateTeam.Toolkit.Extensions;
using System.Collections.Generic;

namespace UltimateTeam.Toolkit.Models
{
    public class AuctionInfo
    {
        public const uint MinBid = 150;
        public int Id { get; set; }
        public string BidState { get; set; }

        public long BuyNowPrice { get; set; }

        public long CurrentBid { get; set; }

        public int Expires { get; set; }

        public ItemData ItemData { get; set; }

        public long Offers { get; set; }

        public string SellerEstablished { get; set; }

        public long SellerId { get; set; }

        public string SellerName { get; set; }

        public long StartingBid { get; set; }

        public long TradeId { get; set; }

        public string TradeState { get; set; }

        public bool? Watched { get; set; }

        public AuctionInfo Clone()
        {
            AuctionInfo ac = new AuctionInfo();
            ItemData itemData = new ItemData();
            List<Attribute> attributeList = new List<Attribute>();
            List<Attribute> lifeTimeStats = new List<Attribute>();
            List<Attribute> statsList = new List<Attribute>();
            Attribute _AB;
            foreach (var d in this.ItemData.AttributeList)
            {
                _AB = new Attribute();
                _AB.Id = d.Id;
                _AB.Index = d.Index;
                _AB.Value = d.Value;
                attributeList.Add(_AB);
            }

            foreach (var d in this.ItemData.LifeTimeStats)
            {
                _AB = new Attribute();
                _AB.Id = d.Id;
                _AB.Index = d.Index;
                _AB.Value = d.Value;
                lifeTimeStats.Add(_AB);
            }

            foreach (var d in this.ItemData.StatsList)
            {
                _AB = new Attribute();
                _AB.Id = d.Id;
                _AB.Index = d.Index;
                _AB.Value = d.Value;
                statsList.Add(_AB);
            }
            itemData.AttributeList = attributeList;
            itemData.LifeTimeStats = lifeTimeStats;
            itemData.StatsList = statsList;

            itemData.AssetId = this.ItemData.AssetId;
            itemData.Assists = this.ItemData.Assists;
            itemData.CardSubTypeId = this.ItemData.CardSubTypeId;
            itemData.Contract = this.ItemData.Contract;
            itemData.DiscardValue = this.ItemData.DiscardValue;
            itemData.Fitness = this.ItemData.Fitness;
            itemData.Formation = this.ItemData.Formation;
            itemData.Id = this.ItemData.Id;
            itemData.InjuryGames = this.ItemData.InjuryGames;
            itemData.InjuryType = this.ItemData.InjuryType;
            itemData.ItemState = this.ItemData.ItemState;
            itemData.ItemType = this.ItemData.ItemType;
            itemData.LastSalePrice = this.ItemData.LastSalePrice;
            itemData.LeagueId = this.ItemData.LeagueId;
            itemData.LifeTimeAssists = this.ItemData.LifeTimeAssists;
            itemData.LifeTimeStats = this.ItemData.LifeTimeStats;
            itemData.LoyaltyBonus = this.ItemData.LoyaltyBonus;
            itemData.Morale = this.ItemData.Morale;
            itemData.Owners = this.ItemData.Owners;
            itemData.Pile = this.ItemData.Pile;
            itemData.PlayStyle = this.ItemData.PlayStyle;
            itemData.PreferredPosition = this.ItemData.PreferredPosition;
            itemData.RareFlag = this.ItemData.RareFlag;
            itemData.Rating = this.ItemData.Rating;
            itemData.ResourceId = this.ItemData.ResourceId;
            itemData.StatsList = this.ItemData.StatsList;
            itemData.Suspension = this.ItemData.Suspension;
            itemData.TeamId = this.ItemData.TeamId;
            itemData.Timestamp = this.ItemData.Timestamp;
            itemData.Training = this.ItemData.Training;
            itemData.Untradeable = this.ItemData.Untradeable;

            ac.ItemData = itemData;
            ac.BidState = this.BidState;
            ac.BuyNowPrice = this.BuyNowPrice;
            ac.CurrentBid = this.CurrentBid;
            ac.Expires = this.Expires;
            ac.Id = this.Id;
            ac.Offers = this.Offers;
            ac.SellerEstablished = this.SellerEstablished;
            ac.SellerId = this.SellerId;
            ac.SellerName = this.SellerName;
            ac.StartingBid = this.StartingBid;
            ac.TradeId = this.TradeId;
            ac.TradeState = this.TradeState;
            ac.Watched = this.Watched;
               
            return ac;
        }

        public long CalculateBid()
        {
            return CurrentBid == 0 ? StartingBid : CalculateNextBid(CurrentBid);
        }

        public static long CalculateNextBid(long currentBid)
        {
            if (currentBid == 0)
                return MinBid;

            if (currentBid < 1000)
                return currentBid + 50;

            if (currentBid < 10000)
                return currentBid + 100;

            if (currentBid < 50000)
                return currentBid + 250;

            if (currentBid < 100000)
                return currentBid + 500;

            return currentBid + 1000;
        }

        public static long CalculatePreviousBid(long currentBid)
        {
            if (currentBid <= MinBid)
                return 0;

            if (currentBid <= 1000)
                return currentBid - 50;

            if (currentBid <= 10000)
                return currentBid - 100;

            if (currentBid <= 50000)
                return currentBid - 250;

            if (currentBid <= 100000)
                return currentBid - 500;

            return currentBid - 1000;
        }

        public long CalculateBaseId()
        {
            return ItemData.ResourceId.CalculateBaseId();
        }
    }
}