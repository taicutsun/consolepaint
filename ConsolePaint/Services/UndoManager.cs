using System.Collections.Generic;
using ConsolePaint.Commands;

namespace ConsolePaint.Services
{
    public class UndoManager
    {
        private readonly Stack<IUndoableAction> undoStack = new();
        private readonly Stack<IUndoableAction> redoStack = new();

        public void ExecuteAction(IUndoableAction action)
        {
            action.Execute();
            undoStack.Push(action);
            redoStack.Clear();
        }
        
        public void Undo()
        {
            if (undoStack.Count <= 0) return;
            
            var action = undoStack.Pop();
            action.Undo();
            redoStack.Push(action);
        }

        public void Redo()
        {
            if (redoStack.Count <= 0) return;
           
            var action = redoStack.Pop();
            action.Execute();
            undoStack.Push(action);
        }
    }
}
