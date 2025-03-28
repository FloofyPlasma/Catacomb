using Catacomb.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Entity
{
    public class BulletFlame : Bullet
    {
        private bool shrink = false;

        public BulletFlame(Mob.Mob e, double xa, double ya, float damage) : base(e, xa, ya, damage)
        {
            owner = e;
            pos.Set(e.pos.x + xa * 4, e.pos.y + ya * 4);
            this.xa = xa * ((4 * new Random().NextDouble()) + 1); // TODO: Networked random
            this.ya = ya * ((4 * new Random().NextDouble()) + 1); // TODO: Networked random
            SetSize(4, 4);
            physicsSlide = false;
            duration = 27;
            double angle = (System.Math.Atan2(ya, xa) + System.Math.PI * 1.625);
            facing = (8 + (int)(angle / System.Math.PI * 4)) & 7;
            this.damage = damage;

            if (new Random().Next(4) == 0) // TODO: Networked random
            {
                shrink = true;
            }

            freezeTime = 0;
        }

        public override void Render(IAbstractScreen screen)
        {
            // TODO: render to screen
        }
    }
}
