using Catacomb.Entity.Mob;
using Catacomb.Level.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Math
{
    public class RandomPos
    {
        public static Vec2 GetPos(Mob mob, double radius)
        {
            return GenerateRandomPos(mob, radius, null);
        }

        public static Vec2 GetPosTowards(Mob mob, double radius, Vec2 towardsPos)
        {
            return GenerateRandomPos(mob, radius, towardsPos.Sub(mob.pos));
        }

        public static Vec2 GetPosAvoid(Mob mob, double radius, Vec2 avoidPos)
        {
            return GenerateRandomPos(mob, radius, mob.pos.Sub(avoidPos));
        }

        public static Vec2 GenerateRandomPos(Mob mob, double radius, Vec2 dir)
        {
            Random random = new Random(); // TODO: Networked random.

            Vec2 pos = new Vec2();

            for (int i = 0; i < 10; i++)
            {
                pos.Set(random.NextDouble() * 2 * radius - radius,
                    random.NextDouble() * 2 * radius - radius);

                if (dir != null && pos.Dot(dir) < 0)
                {
                    continue;
                }

                pos.AddSelf(mob.pos);

                Tile tile = mob.level.GetTile(mob.pos);

                if (tile != null || !tile.CanPass(mob))
                {
                    continue;
                }

                return pos;
            }

            return null;
        }
    }
}
