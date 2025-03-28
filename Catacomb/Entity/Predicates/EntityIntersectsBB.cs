using Catacomb.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Entity.Predicates
{
    public sealed class EntityIntersectsBB : IBBPredicate<Entity>
    {
        public static readonly EntityIntersectsBB Instance = new EntityIntersectsBB();

        private EntityIntersectsBB() { }
        public bool AppliesTo(Entity item, double x0, double y0, double x1, double y1)
        {
            return !item.removed && item.Intersects(x0, y0, x1, y1);
        }
        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
