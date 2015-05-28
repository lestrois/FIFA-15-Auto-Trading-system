using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUT15_Center.Models
{
    public class AccountStatus
    {
        public int Id { get; set; }
        public int JobID { get; set; }
        public int AccountID { get; set; }
        public string Status { get; set; }
        public DateTime LastModified { get; set; }
        public int CoinsInHand { get; set; }
        public int AuctionValue { get; set; }
        public int TotalValue { get; set; }
        public string PhishingToken { get; set; }
        public string SessionID { get; set; }
    }
}
