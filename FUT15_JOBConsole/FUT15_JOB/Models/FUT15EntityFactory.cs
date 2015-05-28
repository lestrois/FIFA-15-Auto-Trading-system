using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUT15_JOB.Models
{
    public class FUT15EntityFactory
    {
        public FUT15EntityFactory()
        {
        }

        public FUT15Entities GetFUT15Entity(int PlatformID)
        {
            if (PlatformID == 1)
                return new FUT15Entities_PC();
            if (PlatformID == 2)
                return new FUT15Entities_PS3();
            if (PlatformID == 3)
                return new FUT15Entities_PS4();
            if (PlatformID == 4)
                return new FUT15Entities_XBox360();
            if (PlatformID == 5)
                return new FUT15Entities_XBoxOne();

            return null;
        }
    }
}
