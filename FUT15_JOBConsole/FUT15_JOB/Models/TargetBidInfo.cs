using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUT15_JOB.Models
{
    public class TargetBidInfo
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

    }
}
