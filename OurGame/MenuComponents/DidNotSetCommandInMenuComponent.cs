using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OurGame.MenuComponents
{
    // Thrown if we didn't call the MenuComponent.SetCommand(ICommand command) method.
    class DidNotSetCommandInMenuComponent: Exception
    {
        public DidNotSetCommandInMenuComponent()
        {
        }

        public DidNotSetCommandInMenuComponent(string message)
            : base(message)
        {
        }

        public DidNotSetCommandInMenuComponent(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
