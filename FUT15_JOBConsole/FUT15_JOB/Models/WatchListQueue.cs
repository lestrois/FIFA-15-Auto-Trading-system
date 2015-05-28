using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUT15_JOB.Models
{
    public class WatchListQueue
    {
        public int Id { get; set; }
        public long TradeID { get; set; }
        public long ResourceID { get; set; }
        public string Status { get; set; }
        public string BeingUsedBy { get; set; }
        public DateTime LastModified { get; set; }
    }
}
