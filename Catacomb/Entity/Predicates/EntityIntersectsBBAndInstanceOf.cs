using Catacomb.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Entity.Predicates
{
    public class EntityIntersectsBBAndInstanceOf : IBBPredicate<Entity>
    {
        private static HashSet<Type> entityClasses;
        public EntityIntersectsBBAndInstanceOf(Type class1)
        {
            entityClasses = new HashSet<Type> { class1 };
        }

        public EntityIntersectsBBAndInstanceOf(Type class1, Type class2) : this(class1)
        {
            entityClasses.Add(class2);
        }

        public bool AppliesTo(Entity item, double x0, double y0, double x1, double y1)
        {
            foreach (var entityClass in entityClasses)
            {
                if (entityClass.IsInstanceOfType(item))
                {
                    return EntityIntersectsBB.Instance.AppliesTo(item, x0, y0, x1, y1);
                }
            }
            return false;
        }

        public override string ToString()
        {
            return $"EntityIntersectsOneOfBBPredicate{{ entityClasses={string.Join(", ", entityClasses)} }}";
        }
    }
}
