using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Math
{
    public class Mth
    {
        public static int Clamp(int value, int low, int high)
        {
            if (value < low)
            {
                return low;
            }

            return value > high ? high : value;
        }

        public const double PI = System.Math.PI;
        public const double PI2 = System.Math.PI * 2;

        public static double NormalizeAngle(double a, double center)
        {
            return a - PI2 * System.Math.Floor((a + PI - center) / PI2);
        }
    }
}
