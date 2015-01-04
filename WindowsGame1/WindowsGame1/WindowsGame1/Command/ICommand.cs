
namespace Command
{
    public interface ICommand
    {
        void execute();
        void undo();
    }
}
