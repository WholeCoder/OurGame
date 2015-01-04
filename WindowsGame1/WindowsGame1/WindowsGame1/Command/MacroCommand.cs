
namespace Command
{
    public class MacroCommand : ICommand
    {
        private ICommand[] commands;

        public MacroCommand(params ICommand[] com)
        {
            this.commands = new ICommand[com.Length];

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
