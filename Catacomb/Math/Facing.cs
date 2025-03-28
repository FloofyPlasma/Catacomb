using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Math
{
    public class Facing
    {
        public const int NORTH = 0;
        public const int SOUTH = 1;
        public const int WEST = 2;
        public const int EAST = 3;

        public static Vec2 GetVector(int dir)
        {
            return GetVector(dir, 1.0);
        }

        public static Vec2 GetVector(int dir, double length)
        {
            switch (dir)
            {
                case NORTH:
                    return new Vec2(0, -length);
                case SOUTH:
                    return new Vec2(0, +length);
                case WEST:
                    return new Vec2(-length, 0);
                case EAST:
                    return new Vec2(+length, 0);
            }

            return null;
        }
    }
}
