using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO: This may be replaceable with the XNA Vector2.

namespace Catacomb.Math
{
    public class Vec2 : ICloneable
    {
        public double x, y;

        public Vec2()
        {
            x = 0;
            y = 0;
        }

        public Vec2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Vec2 Floor()
        {
            return new Vec2(System.Math.Floor(x), System.Math.Floor(y));
        }

        public override bool Equals(object obj)
        {
            if (obj is Vec2)
            {
                Vec2 p = (Vec2)obj;

                return p.x == x && p.y == y;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int res = 17;
            res = res * 17 + ((double)x).GetHashCode();
            res = res * 17 + ((double)y).GetHashCode();

            return res;
        }

        public double DistSqr(Vec2 to)
        {
            double xd = x - to.x;
            double yd = y - to.y;

            return xd * xd + yd * yd;
        }

        public double Dist(Vec2 pos)
        {
            return System.Math.Sqrt(DistSqr(pos));
        }

        public Vec2 Clone()
        {
            return new Vec2(x, y);
        }

        public Vec2 Add(Vec2 p)
        {
            return new Vec2(x + p.x, y + p.y);
        }

        public void Set(double x, double y)
        {
            this.x = x;
            this.y = y;
            Validate();
        }

        public Vec2 Sub(Vec2 p)
        {
            return new Vec2(x - p.x, y - p.y);
        }

        public override string ToString()
        {
            return "[" + x + ", " + y + "]";
        }

        public double Dot(Vec2 v)
        {
            return x * v.x + y * v.y;
        }

        public void AddSelf(Vec2 p)
        {
            x += p.x;
            y += p.y;
            Validate();
        }

        public void AddSelf(double x, double y)
        {
            this.x += x;
            this.y += y;
            Validate();
        }

        public void Copy(Vec2 pos)
        {
            this.x = pos.x;
            this.y = pos.y;
            Validate();
        }

        public double LengthSqr()
        {
            return x * x + y * y;
        }

        public double Length()
        {
            return System.Math.Sqrt(LengthSqr());
        }

        public Vec2 NormalizeSelf()
        {
            double nf = 1 / Length();
            x *= nf;
            y *= nf;
            Validate();

            return this;
        }

        public Vec2 RescaleSelf(double newLen)
        {
            double nf = newLen / Length();
            x *= nf;
            y *= nf;
            Validate();

            return this;
        }

        public Vec2 Scale(double s)
        {
            return new Vec2(x * s, y * s);
        }

        public void ScaleSelf(double s)
        {
            x *= s;
            y *= s;
            Validate();
        }

        public Vec2 Mul(Vec2 v)
        {
            return new Vec2(x * v.x, y * v.y);
        }

        public void Validate()
        {
            if (double.IsInfinity(x) || double.IsInfinity(y) || double.IsNaN(x) || double.IsNaN(y))
            {
                System.Console.WriteLine("Vec2 validation failed: " + ToString());
            }
        }

        public Vec2 Normal()
        {
            Vec2 r = new Vec2(x, y);
            r.NormalizeSelf();
            return r;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
