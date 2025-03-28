using Catacomb.GUI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.GUI
{
    public abstract class GuiMenu : GuiComponent, IButtonListener
    {
        public void ButtonPressed(ClickableComponent button)
        {
            throw new NotImplementedException();
        }
    }
}
