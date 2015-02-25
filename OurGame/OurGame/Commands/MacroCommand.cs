using System.Diagnostics;
using System.Linq;

namespace OurGame.Commands
{
    public class MacroCommand : ICommand
    {
        private readonly ICommand[] _commands;

        public MacroCommand(params ICommand[] com)
        {
            Debug.Assert(com != null, "Command list, com, can't be null!");

            _commands = new ICommand[com.Length];

            for (var i = 0; i < com.Length; i++)
            {
                _commands[i] = com[i];
            }
        }

        public override string ToString()
        {
            var tostr = "MacroCommand - has " + _commands.Length + " Commands in it whitch are:\n";
            return _commands.Aggregate(tostr, (current, command) => current + ("\t" + command.ToString() + "\n"));
        }

        public void Execute()
        {
            // I want to make sure these are called in order.
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < _commands.Length; i++)
            {
                _commands[i].Execute();
            }
        }

        public void Undo()
        {
            for (var i = _commands.Length - 1; i >= 0; i--)
            {
                _commands[i].Undo();
            }
        }
    }
}