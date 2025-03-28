using Catacomb.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Entity
{
    public class BulletMelee : Bullet
    {
        public uint color = 0xffcc9966;

        public BulletMelee(Mob.Mob e, double xa, double ya, float damage, int range) : base(e, xa, ya, damage)
        {
            duration = range;
        }

        public BulletMelee(Mob.Mob e, double xa, double ya, float damage, int range, uint color) : this(e, xa, ya, damage, range)
        {
            this.color = color;
        }

        public override void Tick()
        {
            if (--duration <= 0)
            {
                Remove();

                return;
            }

            base.Tick();
        }

        public override void Render(IAbstractScreen screen)
        {
            // TODO: render to screen
        }
    }
}
