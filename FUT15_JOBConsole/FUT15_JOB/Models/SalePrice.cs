using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUT15_JOB.Models
{
    public class SalePrice
    {
        public long ResourceID { get; set; }
        public int TargetSaleMin { get; set; }
        public int TargetSaleMax { get; set; }
        public SalePrice(long resourceID, int targetSaleMin, int targetSaleMax)
        {
            ResourceID = resourceID;
            TargetSaleMax = targetSaleMax;
            TargetSaleMin = targetSaleMin;
        }
    }
}
