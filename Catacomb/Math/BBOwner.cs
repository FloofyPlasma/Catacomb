using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Math
{
    public interface IBBOwner
    {
        void HandleCollision(Entity.Entity entity, double xa, double ya);
    }
}
