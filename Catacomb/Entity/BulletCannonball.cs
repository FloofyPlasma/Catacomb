using Catacomb.Entity.Animation;
using Catacomb.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Entity
{
    public class BulletCannonball : Bullet
    {
        public BulletCannonball(Mob.Mob e, double xa, double ya, float damage) : base(e, xa, ya, damage)
        {
        }

        override public void Tick()
        {
            if (--duration <= -20)
            {
                Remove();

                return;
            }

            if (!Move(xa, ya))
            {
                if (Move(-xa, ya))
                {
                    xa = -xa;
                }

                if (Move(xa, -ya))
                {
                    ya = -ya;
                }
            }

            xa *= 0.95;
            ya *= 0.95;
        }

        override public void Remove()
        {
            level.AddEntity(new LargeBombExplodeAnimation(pos.x, pos.y));
            // TODO: play sound
            float BOMB_DISTANCE = 100;
            List<Entity> entities = level.GetEntities(pos.x - BOMB_DISTANCE, pos.y - BOMB_DISTANCE, pos.x + BOMB_DISTANCE, pos.y + BOMB_DISTANCE, typeof(Mob.Mob));

            foreach (Entity e in entities)
            {
                double distSqr = pos.DistSqr(e.pos);

                if (distSqr < (BOMB_DISTANCE * BOMB_DISTANCE))
                {
                    ((Mob.Mob)e).Hurt(this, (float)(damage * damage / distSqr));
                }
            }

            base.Remove();
        }

        public override void Render(IAbstractScreen screen)
        {
            // TODO: draw on screen
        }
    }
}
