namespace OurGame.Commands
{
    public class DoNothingCommand : ICommand
    {
        public override string ToString()
        {
            return "DoNothingCommand";
        }

        public void Execute()
        {
        }

        public void Undo()
        {
        }
    }
}