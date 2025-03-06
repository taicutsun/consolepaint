using ConsolePaint.Services;
using System.Globalization;

namespace ConsolePaint.Terminal
{
    public partial class Terminal
    {
        private void DrawMenu()
        {
            var row = canvasHeight + 2;
            ClearLine(row);
            Console.SetCursorPosition(0, row);
            Console.WriteLine("Menu: D - add figure, S - save, L - load, Enter - select/unselect");
        }
        
        private void PrintMessage(string msg)
        {
            var row = canvasHeight + 4;
            ClearLine(row);
            Console.SetCursorPosition(0, row);
            Console.WriteLine(msg);
        }

        private void ClearMenuArea()
        {
            var startRow = canvasHeight + 2;
            for (var i = 0; i < MenuLines; i++)
            {
                ClearLine(startRow + i);
            }
        }

        private static void ClearLine(int row)
        {
            Console.SetCursorPosition(0, row);
            Console.Write(new string(' ', 120));   
            Console.SetCursorPosition(0, row);
        }
        private bool PromptTriangleInput(out int x1, out int y1, out int x2, out int y2, out int x3, out int y3, out char symbol, out ConsoleColor color)
        {
            x1 = y1 = x2 = y2 = x3 = y3 = 0;
            symbol = '*';
            color = ConsoleColor.White;

            PrintMessage("Enter cord of first top (X1 Y1):");
            if (!TryReadInt(out x1)) return false;
            if (!TryReadInt(out y1)) return false;
            TempPointDraw(x1, y1, out Shape tempPoint1);

            PrintMessage("Enter cord of second top (X2 Y2):");
            if (!TryReadInt(out x2)) return false;
            if (!TryReadInt(out y2)) return false;
            TempPointDraw(x2, y2, out Shape tempPoint2);

            PrintMessage("Enter cord of third top (X3 Y3):");
            if (!TryReadInt(out x3)) return false;
            if (!TryReadInt(out y3)) return false;
            TempPointDraw(x3, y3, out Shape tempPoint3);

            PrintMessage("Chose symbol (Enter=*)");
            var symInput = ReadLineAt(canvasHeight + 4);
            if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

            PrintMessage("Color (Enter=White)");
            var colInput = ReadLineAt(canvasHeight + 4);
            if (!string.IsNullOrEmpty(colInput))
            {
                if (!Enum.TryParse(colInput, true, out color))
                    color = ConsoleColor.White;
            }
            TempPointsRemove([tempPoint1, tempPoint2, tempPoint3]);

            return true;
        }

        private bool PromptLineInput(out int x1, out int y1, out int x2, out int y2,
                                     out char symbol, out ConsoleColor color)
        {
            x1 = y1 = x2 = y2 = 0;
            symbol = '*';
            color = ConsoleColor.White;

            PrintMessage("enter X1:");
            if (!TryReadInt(out x1)) return false;

            PrintMessage("enter Y1:");
            if (!TryReadInt(out y1)) return false;

            TempPointDraw(x1, y1, out Shape tempPoint1);

            PrintMessage("enter X2:");
            if (!TryReadInt(out x2)) return false;

            PrintMessage("enter Y2:");
            if (!TryReadInt(out y2)) return false;

            TempPointDraw(x2, y2, out Shape tempPoint2);

            PrintMessage("СSymbol for line (Enter=*)");
            var symInput = ReadLineAt(canvasHeight + 5);
            if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

            PrintMessage("Color (Enter=White)");
            var colInput = ReadLineAt(canvasHeight + 5);
            if (!string.IsNullOrEmpty(colInput))
            {
                if (!Enum.TryParse(colInput, true, out color))
                    color = ConsoleColor.White;
            }

            TempPointsRemove([tempPoint1, tempPoint2]);

            return true;
        }

        private bool PromptPointInput(out int x, out int y, out char symbol, out ConsoleColor color)
        {
            x = y = 0;
            symbol = '*';
            color = ConsoleColor.White;

            PrintMessage("Enter X:");
            if (!TryReadInt(out x)) return false;

            PrintMessage("Enter Y:");
            if (!TryReadInt(out y)) return false;

            PrintMessage("Dot symbol (Enter=*)");
            var symInput = ReadLineAt(canvasHeight + 5);
            if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

            PrintMessage("Цвет (Enter=White)");
            var colInput = ReadLineAt(canvasHeight + 5);
            if (string.IsNullOrEmpty(colInput)) return true;
            
            if (!Enum.TryParse(colInput, true, out color))
                color = ConsoleColor.White;
            return true;
        }

        private bool PromptRectangleInput(out int x1, out int y1, out int x2, out int y2,
                                          out char symbol, out ConsoleColor color)
        {
            x1 = y1 = x2 = y2 = 0;
            symbol = '#';
            color = ConsoleColor.White;

            PrintMessage("Enter X1 (left top):");
            if (!TryReadInt(out x1)) return false;

            PrintMessage("Enter Y1 (left top):");
            if (!TryReadInt(out y1)) return false;

            TempPointDraw(x1, y1, out Shape tempPoint1);

            PrintMessage("Enter X2 (right bottom):");
            if (!TryReadInt(out x2)) return false;

            PrintMessage("Enter Y2 (right bottom):");
            if (!TryReadInt(out y2)) return false;

            TempPointDraw(x2, y2, out Shape tempPoint2);

            PrintMessage("Symbol of triangle (Enter=#)");
            var symInput = ReadLineAt(canvasHeight + 5);  //было 4
            if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

            PrintMessage("Color (Enter=White)");
            var colInput = ReadLineAt(canvasHeight + 5);
            if (!string.IsNullOrEmpty(colInput))
            {
                if (!Enum.TryParse(colInput, true, out color))
                    color = ConsoleColor.White;
            }

            TempPointsRemove([tempPoint1, tempPoint2]);

            return true;
        }
        private void TempPointDraw(int x, int y, out Shape tempPoint)
        {
            tempPoint = ShapeFactory.CreatePoint(x, y, '*', ConsoleColor.Red);
            canvas.AddShape(tempPoint);
        }

        private void TempPointsRemove(Shape[] shapes)
        {
            foreach (var s in shapes)
            {
                canvas.RemoveShape(s);
            }
        }
        
    }
}