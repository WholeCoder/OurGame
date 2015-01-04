
namespace OurGame.Commands
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

        public void Execute()
        {
            for (int i = 0; i < this.commands.Length; i++)
            {
                this.commands[i].Execute();
            }
        }

        public void Undo()
        {
            for (int i = this.commands.Length - 1; i >= 0 ; i--)
            {
                this.commands[i].Undo();
            }
        }
    }
}
