namespace OurGame.Commands
{
    public class DoNothingCommand : ICommand
    {
        public void Execute()
        {
        }

        public void Undo()
        {
        }

        public override string ToString()
        {
            return "DoNothingCommand";
        }
    }
}