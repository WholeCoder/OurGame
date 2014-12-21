using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Command
{
    public interface Command
    {
        void execute();
        void undo();
    }
}
