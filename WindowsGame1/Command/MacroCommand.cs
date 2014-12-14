using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Command
{
    class MacroCommand : Command
    {
        private Command[] commands;

        public MacroCommand(params Command[] com)
        {
            this.commands = new Command[com.Length];

            for (int i = 0; i < com.Length; i++)
            {
                this.commands[i] = com[i];
            }
        }

        public void execute()
        {
            for (int i = 0; i < this.commands.Length; i++)
            {
                this.commands[i].execute();
            }
        }

        public void undo()
        {
            for (int i = this.commands.Length - 1; i >= 0 ; i--)
            {
                this.commands[i].undo();
            }
        }
    }
}
