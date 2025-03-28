using Catacomb.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Entity
{
    public class BulletBuckshot : Bullet
    {
        public BulletBuckshot(Mob.Mob owner, double xa, double ya, float damage) : base(owner, xa, ya, damage)
        {
            this.owner = owner;
            pos.Set(owner.pos.x + xa * 4, owner.pos.y + ya * 4);
            this.xa = xa * 18;
            this.ya = ya * 18;
            SetSize(2, 2);
            physicsSlide = false;
            duration = 20;
            this.damage = damage;
        }

        override public void Tick()
        {
            if (--duration <= 0)
            {
                Remove();

                return;
            }

            if (!Move(xa, ya))
            {
                hit = true;
            }

            if (hit && !removed)
            {
                Remove();
            }

            if (damage > 0.5f)
            {
                damage -= 0.5f;
            }
        }

        public override void Render(IAbstractScreen screen)
        {
            // TODO: render to screen
        }
    }
}
