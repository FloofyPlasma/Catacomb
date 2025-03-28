using Catacomb.Entity.Building;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Entity
{
    public interface ICarrySwap
    {
        public bool IsCarrying();
        public bool CanCarry(Building.Building b);
        public bool CanPickup(Building.Building b);
        public Building.Building GetCarrying();
        public void Pickup(Building.Building b);
        public Building.Building TryToSwap(Building.Building b);
    }
}
