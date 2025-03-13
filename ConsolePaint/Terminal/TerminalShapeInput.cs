using ConsolePaint.Services;
using System.Globalization;

namespace ConsolePaint.Terminal
{
    public partial class Terminal
    {
        private bool PromptEllipseInput(out int x, out int y, out int radiusX, out int radiusY, out char symbol, out ConsoleColor color)
        {
            x = y = radiusX = radiusY = 0;
            symbol = '*';
            color = ConsoleColor.White;
            Shape tempPoint = null!;
            try
            {
                PrintMessage("Enter center coordinate (X):");
                if (!TryReadIntInRange(out x, 0, canvasWidth - 1)) return false;

                PrintMessage("Enter center coordinate (Y):");
                if (!TryReadIntInRange(out y, 0, canvasHeight - 1)) return false;
                TempPointDraw(x, y, out tempPoint);

                PrintMessage("Enter radius for X:");
                if (!TryReadIntInRange(out radiusX, 1, canvasWidth / 2 - 1)) return false;
                int maxRadiusX = Math.Min(x, canvasWidth - 1 - x);
                if (radiusX < 1 || radiusX > maxRadiusX)
                {
                    PrintMessage($"X radius must be between 1 and {maxRadiusX}.");
                    return false;
                }

                PrintMessage("Enter radius for Y:");
                if (!TryReadIntInRange(out radiusY, 1, canvasHeight / 2 - 1)) return false;
                int maxRadiusY = Math.Min(y, canvasHeight - 1 - y);
                if (radiusY < 1 || radiusY > maxRadiusY)
                {
                    PrintMessage($"Y radius must be between 1 and {maxRadiusY}.");
                    return false;
                }

                PrintMessage("Symbol for ellipse (Enter = *):");
                string symInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

                PrintMessage("Color (Enter = White):");
                string colInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(colInput))
                {
                    if (!Enum.TryParse(colInput, true, out color))
                        color = ConsoleColor.White;
                }

                return true;
            }
            finally
            {
                TempPointsRemove(new Shape[] { tempPoint });
            }
        }

        private bool PromptTriangleInput(out int x1, out int y1, out int x2, out int y2, out int x3, out int y3, out char symbol, out ConsoleColor color)
        {
            x1 = y1 = x2 = y2 = x3 = y3 = 0;
            symbol = '*';
            color = ConsoleColor.White;
            Shape tempPoint1, tempPoint2, tempPoint3;
            tempPoint1 = tempPoint2 = tempPoint3 = null!;
            try
            {
                PrintMessage("Enter first vertex coordinate (X1):");
                if (!TryReadIntInRange(out x1, 0, canvasWidth - 1)) return false;
                PrintMessage("Enter first vertex coordinate (Y1):");
                if (!TryReadIntInRange(out y1, 0, canvasHeight - 1)) return false;
                TempPointDraw(x1, y1, out tempPoint1);

                PrintMessage("Enter second vertex coordinate (X2):");
                if (!TryReadIntInRange(out x2, 0, canvasWidth - 1)) return false;
                PrintMessage("Enter second vertex coordinate (Y2):");
                if (!TryReadIntInRange(out y2, 0, canvasHeight - 1)) return false;
                TempPointDraw(x2, y2, out tempPoint2);

                PrintMessage("Enter third vertex coordinate (X3):");
                if (!TryReadIntInRange(out x3, 0, canvasWidth - 1)) return false;
                PrintMessage("Enter third vertex coordinate (Y3):");
                if (!TryReadIntInRange(out y3, 0, canvasHeight - 1)) return false;
                TempPointDraw(x3, y3, out tempPoint3);

                PrintMessage("Symbol for triangle (Enter = *):");
                string symInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

                PrintMessage("Color (Enter = White):");
                string colInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(colInput))
                {
                    if (!Enum.TryParse(colInput, true, out color))
                        color = ConsoleColor.White;
                }
                return true;
            }
            finally
            {
                TempPointsRemove(new Shape[] { tempPoint1, tempPoint2, tempPoint3 });
            }
        }

        private bool PromptLineInput(out int x1, out int y1, out int x2, out int y2, out char symbol, out ConsoleColor color)
        {
            Shape tempPoint1, tempPoint2;
            tempPoint1 = tempPoint2 = null!;
            x1 = y1 = x2 = y2 = 0;
            symbol = '*';
            color = ConsoleColor.White;
            try
            {
                PrintMessage("Enter X1:");
                if (!TryReadIntInRange(out x1, 0, canvasWidth - 1)) return false;

                PrintMessage("Enter Y1:");
                if (!TryReadIntInRange(out y1, 0, canvasHeight - 1)) return false;
                TempPointDraw(x1, y1, out tempPoint1);

                PrintMessage("Enter X2:");
                if (!TryReadIntInRange(out x2, 0, canvasWidth - 1)) return false;

                PrintMessage("Enter Y2:");
                if (!TryReadIntInRange(out y2, 0, canvasHeight - 1)) return false;
                TempPointDraw(x2, y2, out tempPoint2);

                PrintMessage("Symbol for line (Enter = *):");
                string symInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

                PrintMessage("Color (Enter = White):");
                string colInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(colInput))
                {
                    if (!Enum.TryParse(colInput, true, out color))
                        color = ConsoleColor.White;
                }
                return true;
            }
            finally
            {
                TempPointsRemove(new Shape[] { tempPoint1, tempPoint2 });
            }
        }

        private bool PromptPointInput(out int x, out int y, out char symbol, out ConsoleColor color)
        {
            x = y = 0;
            symbol = '*';
            color = ConsoleColor.White;

            PrintMessage("Enter X:");
            if (!TryReadIntInRange(out x, 0, canvasWidth - 1)) return false;

            PrintMessage("Enter Y:");
            if (!TryReadIntInRange(out y, 0, canvasHeight - 1)) return false;

            PrintMessage("Symbol for point (Enter = *):");
            string symInput = ReadLineAt(canvasHeight + 5);
            if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

            PrintMessage("Color (Enter = White):");
            string colInput = ReadLineAt(canvasHeight + 5);
            if (!string.IsNullOrEmpty(colInput))
            {
                if (!Enum.TryParse(colInput, true, out color))
                    color = ConsoleColor.White;
            }
            return true;
        }

        private bool PromptRectangleInput(out int x1, out int y1, out int x2, out int y2, out char symbol, out ConsoleColor color)
        {
            x1 = y1 = x2 = y2 = default;
            symbol = '#';
            color = ConsoleColor.White;
            Shape tempPoint1, tempPoint2;
            tempPoint1 = tempPoint2 = null!;
            try
            {
                PrintMessage("Enter X of first vertex:");
                if (!TryReadIntInRange(out x1, 0, canvasWidth - 1)) return false;

                PrintMessage("Enter Y of first vertex:");
                if (!TryReadIntInRange(out y1, 0, canvasHeight - 1)) return false;
                TempPointDraw(x1, y1, out tempPoint1);

                PrintMessage("Enter X of second vertex:");
                if (!TryReadIntInRange(out x2, 0, canvasWidth - 1)) return false;

                PrintMessage("Enter Y of second vertex:");
                if (!TryReadIntInRange(out y2, 0, canvasHeight - 1)) return false;
                TempPointDraw(x2, y2, out tempPoint2);

                PrintMessage("Symbol for rectangle (Enter = #):");
                string symInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

                PrintMessage("Color (Enter = White):");
                string colInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(colInput))
                {
                    if (!Enum.TryParse(colInput, true, out color))
                        color = ConsoleColor.White;
                }
                return true;
            }
            finally
            {
                TempPointsRemove(new Shape[] { tempPoint1, tempPoint2 });
            }
        }

        private bool TryReadIntInRange(out int result, int min, int max)
        {
            if (!TryReadInt(out result))
                return false;
            if (result < min || result > max)
            {
                PrintMessage($"Value must be between {min} and {max}.");
                return false;
            }
            return true;

            bool TryReadInt(out int result)
            {
                FlushInput();
                int row = canvasHeight + 5;
                string input = ReadLineAt(row);
                if (!int.TryParse(input, out result))
                {
                    PrintMessage("Input error (not an integer)!");
                    ReadLineAt(row);
                    return false;
                }
                return true;
            }
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

        private void SaveCanvas()
        {
            PrintMessage("Enter file name for saving (e.g., canvas):");
            string filename = ReadLineAt(canvasHeight + 5);
            if (!filename.EndsWith(".json"))
            {
                filename += ".json";
            }
            FileManager.SaveShapesToFile(canvas.Shapes, filename);
            PrintMessage("Canvas saved to " + filename + ". Press Enter.");
            ReadLineAt(canvasHeight + 5);
        }

        private void LoadCanvas()
        {
            PrintMessage("Enter file name for loading (e.g., canvas):");
            string filename = ReadLineAt(canvasHeight + 5);
            LoadCanvas(filename);
        }

        private void LoadCanvas(string filename)
        {
            if (!filename.EndsWith(".json"))
            {
                filename += ".json";
            }
            List<Shape> loadedShapes = FileManager.LoadShapesFromFile(filename);
            if (loadedShapes != null && loadedShapes.Count > 0)
            {
                foreach (Shape s in loadedShapes)
                {
                    canvas.AddShape(s);
                }
                canvas.RedrawAllShapes();
                PrintMessage("Canvas loaded from " + filename + ". Press Enter.");
                ReadLineAt(canvasHeight + 5);
            }
            else
            {
                PrintMessage("Failed to load canvas from " + filename + ". Press Enter.");
                ReadLineAt(canvasHeight + 5);
            }
        }
    }
}
