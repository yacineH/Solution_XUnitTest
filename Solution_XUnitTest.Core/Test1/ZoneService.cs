using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution_XUnitTest.Core.Test1
{
    public class ZoneService : IZoneService
    {
        private static Random random = new Random();
        public bool IsDangerZone(string dutyStation)
        {
            // Huge Logic Goes here

            // 1 / 10 probability 
            return random.Next(1, 10) == 3;
        }
    }
}
