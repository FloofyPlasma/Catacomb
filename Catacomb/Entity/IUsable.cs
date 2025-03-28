using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Catacomb.Entity
{
    public interface IUsable
    {
        public void Use(Entity user);
        public bool Upgrade(Player player);
        public void SetHighlighted(bool hl);
        public bool IsHighlightable();
        public bool IsAllowedToCancel();
    }
}
