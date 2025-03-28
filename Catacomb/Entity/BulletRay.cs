using Catacomb.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Entity
{
    public class BulletRay : Bullet
    {
        private int maxBounceNumber;
        private double previousPositionX, previousPositionY;
        private int frame;

        public BulletRay(Mob.Mob e, double xa, double ya, float damage) : base(e, xa, ya, damage)
        {
            owner = e;
            pos.Set(e.pos.x + xa * 4, ya + e.pos.y + ya * 4);
            this.xa = xa * 5;
            this.ya = ya * 5;
            SetSize(4, 4);
            physicsSlide = false;
            duration = 50;
            maxBounceNumber = 5;
            frame = 0;
            this.damage = damage;
        }

        public override void Tick()
        {
            previousPositionX = pos.x;
            previousPositionY = pos.y;

            if (--duration <= 0)
            {
                Remove();

                return;
            }

            if (!Move(xa, ya))
            {
                if (maxBounceNumber > 0)
                {
                    if (previousPositionX != pos.x)
                    {
                        ya = -ya;
                    }

                    if (previousPositionY != pos.y)
                    {
                        ya = -ya;
                    }

                    if (previousPositionY == pos.y && previousPositionX == pos.x)
                    {
                        xa = -xa;
                        ya = -ya;
                    }

                    xa *= 1.2;
                    ya *= 1.2;
                    duration += 5;
                    damage *= 1.5f;

                    maxBounceNumber--;
                }
                else
                {
                    hit = true;
                }
            }

            if (hit && !removed)
            {
                Remove();
            }

            frame = new Random().Next(8); // TODO: network synced random
        }

        public override void Render(IAbstractScreen screen)
        {
            // TODO: draw to screen
        }
    }
}
