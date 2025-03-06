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

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    return;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (selectedShape == null)
                    {
                        selectedShape = GetShapeAtCursor();
                        PrintMessage(selectedShape is not null
                            ? "You selected a figure. Press arrow to move it .X - delete. F - to fill.  Enter - unselect"
                            : "No figure found");
                    }
                    else
                    {
                        selectedShape = null;
                        PrintMessage("Unselected. Press arrow to move cursor");
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Z)
                {
                    undoManager.Undo();
                }
                else if (keyInfo.Key == ConsoleKey.Y)
                {
                    undoManager.Redo();
                }
                else if (keyInfo.Key == ConsoleKey.X)
                {
                    if (selectedShape != null)
                    {
                        var addAction = new RemoveShapeAction(canvas, selectedShape);
                        undoManager.ExecuteAction(addAction);
                        selectedShape = null;
                        PrintMessage("Selected figure deleted.");
                    }
                    else
                    {
                        PrintMessage("No selected figure for deletion.");
                    }
                }
                else if (keyInfo.Key == ConsoleKey.D)
                {
                    ShowAddShapeMenu();
                }
                else if (keyInfo.Key == ConsoleKey.F)
                {
                    if (selectedShape != null)
                    {
                        PrintMessage("Filling symbol (Enter = +):");
                        string fillSym = ReadLineAt(canvasHeight + 5);
                        char fillSymbol = string.IsNullOrEmpty(fillSym) ? '+' : fillSym[0];

                        PrintMessage("Filling color (exmpl: Blue, Enter = White):");
                        string fillCol = ReadLineAt(canvasHeight + 5);
                        ConsoleColor fillColor = Enum.TryParse(fillCol, true, out fillColor) ? fillColor : ConsoleColor.White;

                        var fillAction = new FillShapeAction(canvas, selectedShape, fillSymbol, fillColor);
                        undoManager.ExecuteAction(fillAction);

                        PrintMessage("Filled. Press 'Enter' 2 times.");
                        ReadLineAt(canvasHeight + 5);
                    }
                    else
                    {
                        PrintMessage("No selected figure for filling.");
                        ReadLineAt(canvasHeight + 5);
                    }
                }
                else if (keyInfo.Key == ConsoleKey.S)
                {
                    SaveCanvas();
                }
                else if (keyInfo.Key == ConsoleKey.L)
                {
                    LoadCanvas();
                }
                else if (IsArrowKey(keyInfo.Key))
                {
                    int dx = 0, dy = 0;
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow: dy = -1; break;
                        case ConsoleKey.DownArrow: dy = 1; break;
                        case ConsoleKey.LeftArrow: dx = -1; break;
                        case ConsoleKey.RightArrow: dx = 1; break;
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
            }
        }
      
        private void ShowAddShapeMenu()
        {
            // Снимаем выбор, чтобы стрелки не двигали выбранную фигуру
            selectedShape = null;

            ClearMenuArea();
            PrintMessage("Add figure: [1] Line, [2] Dot, [3] Rectangle, [4] Triangle");
            var choice = ReadLineAt(canvasHeight + 5);
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
                    if (PromptTriangleInput(out int tx1, out int ty1, out int tx2, out int ty2, out int tx3, out int ty3, out char tSym, out ConsoleColor tColor))
                    {
                        s = ShapeFactory.CreateTriangle(tx1, ty1, tx2, ty2, tx3, ty3, tSym, tColor);
                    }
                    break;
                default:
                    PrintMessage("Wrong answer. Press 'Enter' 2 times.");
                    ReadLineAt(canvasHeight + 5);
                    break;
            }

                var addAction = new AddShapeAction(canvas, s);
                undoManager.ExecuteAction(addAction);

            canvas.RedrawAllShapes();
            DrawMenu();
            DrawCursor();
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

        private bool TryReadInt(out int result)
        {
            FlushInput();
            var row = canvasHeight + 5;
            var input = ReadLineAt(row);
            if (int.TryParse(input, out result)) return true;
            PrintMessage("Error, must be an integer");
            ReadLineAt(row);
            return false;
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
            var drawX = cursorX + 1;
            var drawY = cursorY + 1;

            var prevLeft = Console.CursorLeft;
            var prevTop = Console.CursorTop;

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
        
        private void SaveCanvas()
        {
            PrintMessage("Enter file name to save it (exmpl: canvas):");
            var filename = ReadLineAt(canvasHeight + 5);
            if (!filename.EndsWith(".json"))
            {
                filename += ".json";
            } 
            FileManager.SaveShapesToFile(canvas.Shapes, filename);
            PrintMessage("Saved to " + filename + ". Press Enter.");
            ReadLineAt(canvasHeight + 5);
        }
        private void LoadCanvas()
        {
            PrintMessage("Enter file name to load it:");
            var filename = ReadLineAt(canvasHeight + 5);
            LoadCanvas(filename);
        }

        private void LoadCanvas(string filename) 
        {
            if (!filename.EndsWith(".json")) 
            {
                filename += ".json";
            }
            var loadedShapes = FileManager.LoadShapesFromFile(filename);
            if (loadedShapes.Count > 0)
            {
                foreach (var s in loadedShapes)
                {
                    canvas.AddShape(s);
                }
                canvas.RedrawAllShapes();
                PrintMessage("Data loaded from " + filename + ". Press Enter.");
                ReadLineAt(canvasHeight + 5);
            }
            else
            {
                PrintMessage("Error loading data from" + filename + ". Press Enter.");
                ReadLineAt(canvasHeight + 5);
            }
        }
    }
}
