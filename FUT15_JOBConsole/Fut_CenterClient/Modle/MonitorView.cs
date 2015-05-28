using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUT_MonitorCenter.Modle
{
    public class MonitorView
    {
        public MonitorView()
        {
        }
        public int AccountID { get; set; }
        public string UserID { get; set; }
        public string LoginStatus { get; set; }
        public int JobGroupNumber { get; set; }
        public int TotalValue { get; set; }
        public int CoinsInHand { get; set; }
        public int AuctionValue { get; set; }
        public DateTime LastModified { get; set; }
        public int JobID { get; set; }
        public string SessionID { get; set; }
        public string PhishingToken { get; set; }
        public string Platform { get; set; }
    }
}
