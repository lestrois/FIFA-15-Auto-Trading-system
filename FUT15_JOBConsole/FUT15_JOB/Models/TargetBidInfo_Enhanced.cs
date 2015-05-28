using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUT15_JOB.Models
{
    public class TargetBidInfo_Enhanced
    {
        public int Id { get; set; }

        public long ResourceId { get; set; }
        public string Position { get; set; }

        public bool Biddable { get; set; }         //if targetSaleMargin > 0.05 then it's true or false
        public int targetBuyAt { get; set; }
        public int targetSaleMin { get; set; }
        public int targetSaleMax { get; set; }
        public decimal targetSaleMargin { get; set; } //(targetSaleMax - targetSaleMin)/targetSaleMin

        public byte targetBuyAmount { get; set; }
        public bool NeedAutoUpdate { get; set; }    //If being updated by auto calculation

        public int TransactionCount { get; set; }
        public TargetBidInfo_Enhanced(TargetBidInfo TB)
        {
            this.Biddable = TB.Biddable;
            this.Position = TB.Position;
            this.Id = TB.Id;
            this.NeedAutoUpdate = TB.NeedAutoUpdate;
            this.ResourceId = TB.ResourceId;
            this.targetBuyAmount = TB.targetBuyAmount;
            this.targetBuyAt = TB.targetBuyAt;
            this.targetSaleMargin = TB.targetSaleMargin;
            this.targetSaleMax = TB.targetSaleMax;
            this.targetSaleMin = TB.targetSaleMin;
        }
    }
}
