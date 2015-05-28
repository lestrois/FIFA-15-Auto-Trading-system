using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUT15_JOB.Models
{
    public class SaleTracker
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public long ResourceID { get; set; }
        public long BuyTradeID { get; set; }
        public DateTime BuyTime { get; set; }
        public int BuyPrice { get; set; }
        public long? SoldTradeID { get; set; }
        public int? SaleStarting { get; set; }
        public int? SaleBuyNow { get; set; }
        public DateTime? SoldTime { get; set; }
        public int? SoldPrice { get; set; }
        public int? Margin { get; set; }
        public int? MarginPercent { get; set; }
    }
}
