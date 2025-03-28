using Catacomb.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Level.Tile
{
    public abstract class Tile : IBBOwner, IEditable
    {
        public void HandleCollision(Entity.Entity entity, double xa, double ya)
        {
            throw new NotImplementedException();
        }
    }
}
