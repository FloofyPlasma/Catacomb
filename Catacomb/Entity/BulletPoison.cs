using Catacomb.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Entity
{
    public class BulletPoison : Bullet
    {
        public BulletPoison(Mob.Mob e, double xa, double ya, float damage) : base(e, xa, ya, damage)
        {
            duration = 80;
            this.xa = xa * 3;
            this.ya = ya * 3;
        }

        public override void Collide(Entity entity, double xa, double ya)
        {
            if (entity is Mob.Mob)
            {
                Mob.Mob mobEnt = (Mob.Mob)entity;

                if (mobEnt.IsNotFriendOf(owner) && !(entity is Building.Building))
                {
                    mobEnt.Hurt(this, damage);
                    hit = true;
                }
            }

            if (hit)
            {
                // TODO: play sound
            }
        }
        public override void Render(IAbstractScreen screen)
        {
            // TODO: render to screen
        }
    }
}
