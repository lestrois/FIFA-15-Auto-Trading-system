using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUT_MonitorCenter.Modle
{
    public class TargetPriceView
    {
        public TargetPriceView()
        {
        }
        public int Id { get; set; }
        public long ResourceID { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public byte Rating { get; set; }
        public byte RareFlag { get; set; }
        public int TargetBuyAt { get; set; }
        public int TargetSaleMin { get; set; }
        public int TargetSaleMax { get; set; }
        public decimal TargetSaleMargin { get; set; }
        public int Count { get; set; }
    }
}
