using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My.MenuComponents
{
    class DidNotAddAnyMenuItemsToMenucomponent : Exception
    {
        public DidNotAddAnyMenuItemsToMenucomponent()
        {
        }

        public DidNotAddAnyMenuItemsToMenucomponent(string message)
            : base(message)
        {
        }

        public DidNotAddAnyMenuItemsToMenucomponent(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
