﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Math
{
    public class BB
    {
        public double x0, y0;
        public double x1, y1;
        public IBBOwner owner;

        public BB(IBBOwner owner, double x0, double y0, double x1, double y1)
        {
            this.owner = owner;
            this.x0 = x0;
            this.y0 = y0;
            this.x1 = x1;
            this.y1 = y1;
        }

        public bool Intersects(double xx0, double yy0, double xx1, double yy1)
        {
            if (xx0 >= x1 || yy0 >= y1 || xx1 <= x0 || yy1 <= y0)
            { 
                return false;
            }

            return true;
        }

        public BB Grow(double s)
        {
            return new BB(owner, x0 - s, y0 - s, x1 + s, y1 + s);
        }

        public bool Intersects(BB bb)
        {
            if (bb.x0 >= x1 || bb.y0 >= y1 || bb.x1 <= x0 || bb.y1 <= y0)
            {
                return false;
            }

            return true;
        }
    }
}
