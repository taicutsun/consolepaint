using ConsolePaint.Commands;
using ConsolePaint.Services;

namespace ConsolePaint.Terminal
{
    public partial class Terminal
    {
        private readonly Canvas canvas;
        private readonly UndoManager undoManager;
        private readonly int canvasWidth;
        private readonly int canvasHeight;
        private int cursorX;
        private int cursorY;
        private Shape? selectedShape;
        private const int MenuLines = 8;

        public Terminal()
        {
            canvasWidth = Console.WindowWidth - 10;
            canvasHeight = Console.WindowHeight - 10;
            canvas = new Canvas(canvasWidth, canvasHeight);
            undoManager = new UndoManager();
            cursorX = 0;
            cursorY = 0;
        }

        public Terminal(string fileName) 
            : this()
        {
            LoadCanvas(fileName);
        }

        public void Run()
        {
            Console.Clear();
            canvas.DrawFrame();
            canvas.RedrawAllShapes();
            DrawMenu();
            DrawCursor();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Escape:
                        return;

                    case ConsoleKey.Enter:
                        if (selectedShape == null)
                        {
                            selectedShape = GetShapeAtCursor();
                            PrintMessage(selectedShape is not null
                                ? "Shape selected. Use arrow keys to move it. [X] - delete. [F] - fill. Press Enter to cancel selection."
                                : "No shape found under the cursor.");
                        }
                        else
                        {
                            selectedShape = null;
                            PrintMessage("Selection cleared. Arrow keys move the cursor.");
                        }
                        break;

                    case ConsoleKey.Z:
                        undoManager.Undo();
                        break;

                    case ConsoleKey.Y:
                        undoManager.Redo();
                        break;

                    case ConsoleKey.X:
                        if (selectedShape != null)
                        {
                            var removeAction = new RemoveShapeAction(canvas, selectedShape);
                            undoManager.ExecuteAction(removeAction);
                            selectedShape = null;
                            PrintMessage("Selected shape deleted.");
                        }
                        else
                        {
                            PrintMessage("No shape selected for deletion.");
                        }
                        break;

                    case ConsoleKey.D:
                        ShowAddShapeMenu();
                        break;

                    case ConsoleKey.F:
                        if (selectedShape != null)
                        {
                            PrintMessage("Enter fill symbol (Enter = +):");
                            string fillSym = ReadLineAt(canvasHeight + 5);
                            char fillSymbol = string.IsNullOrEmpty(fillSym) ? '+' : fillSym[0];

                            PrintMessage("Enter fill color (e.g., Blue, Enter = White):");
                            string fillCol = ReadLineAt(canvasHeight + 5);
                            ConsoleColor fillColor = Enum.TryParse(fillCol, true, out fillColor) ? fillColor : ConsoleColor.White;

                            var fillAction = new FillShapeAction(canvas, selectedShape, fillSymbol, fillColor);
                            undoManager.ExecuteAction(fillAction);

                            PrintMessage("Fill applied. Press Enter twice.");
                            ReadLineAt(canvasHeight + 5);
                        }
                        else
                        {
                            PrintMessage("No shape selected for filling.");
                            ReadLineAt(canvasHeight + 5);
                        }
                        break;

                    case ConsoleKey.S:
                        SaveCanvas();
                        break;

                    case ConsoleKey.L:
                        LoadCanvas();
                        break;

                    default:
                        if (IsArrowKey(keyInfo.Key))
                        {
                            int dx = 0, dy = 0;
                            switch (keyInfo.Key)
                            {
                                case ConsoleKey.UpArrow:
                                    dy = -1;
                                    break;
                                case ConsoleKey.DownArrow:
                                    dy = 1;
                                    break;
                                case ConsoleKey.LeftArrow:
                                    dx = -1;
                                    break;
                                case ConsoleKey.RightArrow:
                                    dx = 1;
                                    break;
                            }

                            if (selectedShape != null)
                            {
                                var moveAction = new MoveShapeAction(canvas, selectedShape, dx, dy);
                                undoManager.ExecuteAction(moveAction);
                            }
                            else
                            {
                                MoveCursor(dx, dy);
                            }
                        }
                        break;
                }
            }
        }

        private void ShowAddShapeMenu()
        {
            selectedShape = null;
            ClearMenuArea();
            PrintMessage("Add shape: [1] Line, [2] Point, [3] Rectangle, [4] Ellipse, [5] Triangle");
            string choice = ReadLineAt(canvasHeight + 5);
            Shape s = null!;
            switch (choice)
            {
                case "1":
                    if (PromptLineInput(out int x1, out int y1, out int x2, out int y2, out char lineSym, out ConsoleColor lineColor))
                    {
                        s = ShapeFactory.CreateLine(x1, y1, x2, y2, lineSym, lineColor);
                    }
                    break;
                case "2":
                    if (PromptPointInput(out int px, out int py, out char pSym, out ConsoleColor pColor))
                    {
                        s = ShapeFactory.CreatePoint(px, py, pSym, pColor);
                    }
                    break;
                case "3":
                    if (PromptRectangleInput(out int rx1, out int ry1, out int rx2, out int ry2, out char rSym, out ConsoleColor rColor))
                    {
                        s = ShapeFactory.CreateRectangle(rx1, ry1, rx2, ry2, rSym, rColor);
                    }
                    break;
                case "4":
                    if (PromptEllipseInput(out int ex, out int ey, out int exRadius, out int eyRadius, out char eSym, out ConsoleColor eColor))
                    {
                        s = ShapeFactory.CreateEllipse(ex, ey, exRadius, eyRadius, eSym, eColor);
                    }
                    break;
                case "5":
                    if (PromptTriangleInput(out int tx1, out int ty1, out int tx2, out int ty2, out int tx3, out int ty3, out char tSym, out ConsoleColor tColor))
                    {
                        s = ShapeFactory.CreateTriangle(tx1, ty1, tx2, ty2, tx3, ty3, tSym, tColor);
                    }
                    break;
                default:
                    PrintMessage("Invalid choice. Press Enter twice.");
                    ReadLineAt(canvasHeight + 5);
                    break;
            }

            if (s != null)
            {
                var addAction = new AddShapeAction(canvas, s);
                undoManager.ExecuteAction(addAction);
            }

            canvas.RedrawAllShapes();
            DrawMenu();
            DrawCursor();
        }

        private void DrawMenu()
        {
            int row = canvasHeight + 2;
            ClearLine(row);
            Console.SetCursorPosition(0, row);
            Console.WriteLine("Menu: [D] - add shape, [S] - save, [L] - load, [Enter] - select/deselect, [Z]/[Y] - undo/redo, [Esc] - exit");
        }

        private Shape? GetShapeAtCursor()
        {
            var allShapes = canvas.Shapes;
            foreach (var s in allShapes)
            {
                if (s.ContainsPoint(cursorX, cursorY))
                    return s;
            }
            return null;
        }

        private static bool IsArrowKey(ConsoleKey key)
        {
            return (key == ConsoleKey.UpArrow ||
                    key == ConsoleKey.DownArrow ||
                    key == ConsoleKey.LeftArrow ||
                    key == ConsoleKey.RightArrow);
        }

        private void PrintMessage(string msg)
        {
            int row = canvasHeight + 4;
            ClearLine(row);
            Console.SetCursorPosition(0, row);
            Console.WriteLine(msg);
        }

        private void ClearMenuArea()
        {
            int startRow = canvasHeight + 2;
            for (int i = 0; i < MenuLines; i++)
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

        private static string ReadLineAt(int row)
        {
            Console.SetCursorPosition(0, row);
            string input = Console.ReadLine();
            ClearLine(row);
            return input;
        }

        private static void FlushInput()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        private void DrawCursor()
        {
            int drawX = cursorX + 1;
            int drawY = cursorY + 1;
            int prevLeft = Console.CursorLeft;
            int prevTop = Console.CursorTop;
            Console.SetCursorPosition(drawX, drawY);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("_");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(prevLeft, prevTop);
        }

        private void MoveCursor(int dx, int dy)
        {
            EraseCursor();
            cursorX = Math.Max(0, Math.Min(cursorX + dx, canvasWidth - 1));
            cursorY = Math.Max(0, Math.Min(cursorY + dy, canvasHeight - 1));
            DrawCursor();
        }

        private void EraseCursor()
        {
            Pixel oldPixel = canvas.GetPixel(cursorX, cursorY);
            Console.SetCursorPosition(cursorX + 1, cursorY + 1);
            Console.ForegroundColor = oldPixel.Color;
            Console.Write(oldPixel.Symbol);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
