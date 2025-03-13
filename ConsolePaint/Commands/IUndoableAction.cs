namespace ConsolePaint.Commands
{
    public interface IUndoableAction
    {
      
        void Execute();
        
        void Undo();
    }
}