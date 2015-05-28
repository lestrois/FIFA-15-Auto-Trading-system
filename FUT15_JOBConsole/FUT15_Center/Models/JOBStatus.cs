using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FUT15_Center.Models
{
    public class JOBStatus
    {
        public int Id { get; set; }
        public int JOBID { get; set; }
        public int JOBGroupNumber { get; set; }
        public string Status { get; set; }
        public DateTime JobStartAt { get; set; }
        
        public DateTime? JobEndAt { get; set; }
    }
}
