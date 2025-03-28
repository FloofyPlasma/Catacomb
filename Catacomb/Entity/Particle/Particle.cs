using Catacomb.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Entity.Particle
{
    public class Particle : Entity
    {
        public double xa, ya, za;
        public double z;
        public Entity owner;
        public int life;

        public Particle(double x, double y, double xa, double ya)
        {
            pos.Set(x, y);
            double pow = new Random().NextDouble() * 1 + 1; // TODO: network synced random
            this.xa = xa * pow;
            this.ya = ya * pow;
            za = new Random().NextDouble() * 2 + 1.0; // TODO: network synced random
            SetSize(2, 2);
            physicsSlide = false;
            isBlocking = false;
            life = new Random().Next(20) + 50; // TODO: network synced random
        }

        public override void Tick() // FIXME: Should this be override>
        {
            Move(xa, ya);
            z += za;

            if (z < 0)
            {
                z = 0;
                xa *= 0.8;
                ya *= 0.8;
            }
            else
            {
                xa *= 0.98;
                ya *= 0.98;
            }

            za -= 0.2;

            if (--life < 0)
            {
                Remove();
            }
        }

        public override void Render(IAbstractScreen screen)
        {
           // TODO: render to screen
        }
    }
}
