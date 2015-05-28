using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUT15_Center.Models
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public int AccountID { get; set; }
        public int ErrorPosition { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
