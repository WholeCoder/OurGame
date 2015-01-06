using System.Diagnostics;
namespace OurGame.Commands
{
    public class MacroCommand : ICommand
    {
        private ICommand[] _commands;

        public MacroCommand(params ICommand[] com)
        {
            Debug.Assert(com != null);

            this._commands = new ICommand[com.Length];

            for (int i = 0; i < com.Length; i++)
            {
                this._commands[i] = com[i];
            }
        }

        public void Execute()
        {
            for (int i = 0; i < this._commands.Length; i++)
            {
                this._commands[i].Execute();
            }
        }

        public void Undo()
        {
            for (int i = this._commands.Length - 1; i >= 0 ; i--)
            {
                this._commands[i].Undo();
            }
        }
    }
}
