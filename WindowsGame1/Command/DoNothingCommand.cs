using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Command
{
    public class DoNothingCommand : Command
    {
        public void execute()
        {
        }

        public void undo()
        {
        }
    }
}
