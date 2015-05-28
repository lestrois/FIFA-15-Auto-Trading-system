using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUT15_Center.Models
{
    public class TradeConfiguration
    {
        public int Id { get; set; }
        public int AccountID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime LastModified { get; set; }
    }
}
