using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUT15_Center.Models
{
    public class JobPlatform
    {
        public int Id { get; set; }
        public int PlatformID { get; set; }
        public string PlatformName { get; set; }
        public string DBConnection { get; set; }
        public Decimal? CoinsBuyPrice { get; set; }
        public Decimal? CoinsSellPrice { get; set; }
        public Decimal? MarketAdjuster { get; set; }
    }
}
