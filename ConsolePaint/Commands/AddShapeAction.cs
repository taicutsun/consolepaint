﻿
namespace ConsolePaint.Commands
{
    public class AddShapeAction(Canvas canvas, Shape shape) : IUndoableAction
    {
        public void Execute()
        {
            canvas.AddShape(shape);
            canvas.RedrawAllShapes();
        }

        public void Undo()
        {
            canvas.RemoveShape(shape);
            canvas.RedrawAllShapes();
        }
    }
}
