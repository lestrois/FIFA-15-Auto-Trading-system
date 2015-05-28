using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateTeam.Toolkit.Models;
using FUT15_JOB.Models;
using FUT15_JOB;

namespace FUT_MonitorCenter.Modle
{
    public class HistoryView
    {
        public long TradeId { get; set; }
        public long ResourceId { get; set; }
        public string Name { get; set; }
        public string BidState { get; set; }
        public long CurrentBid { get; set; }
        public long StartingBid { get; set; }
        public long BuyNowPrice { get; set; }
        public string PreferredPosition { get; set; }
        public byte Rating { get; set; }
        public byte RareFlag { get; set; }
        public byte Contract { get; set; }
        public byte Fitness { get; set; }
        public DateTime Transaction_Time { get; set; }
    }
}
