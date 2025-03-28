using Catacomb.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Entity.Particle
{
    public class Sparkle : Particle
    {
        public int duration;

        public Sparkle(double x, double y, double xa, double ya) : base(x, y, xa, ya)
        {
            duration = (life = new Random().Next(10) + 20) + 1; // TODO: network synced random
        }

        public override void Render(IAbstractScreen screen)
        {
            // TODO: render to screen
        }
    }
}
