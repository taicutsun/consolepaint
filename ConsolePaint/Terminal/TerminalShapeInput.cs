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

                PrintMessage("Enter radius along X:");
                if (!TryReadIntInRange(out radiusX, 1, canvasWidth / 2 - 1)) return false;
                var maxRadiusX = Math.Min(x, canvasWidth - 1 - x);
                if (radiusX < 1 || radiusX > maxRadiusX)
                {
                    PrintMessage($"Radius along X must be between 1 and {maxRadiusX}.");
                    return false;
                }

                PrintMessage("Enter radius along Y:");
                if (!TryReadIntInRange(out radiusY, 1, canvasHeight / 2 - 1)) return false;
                var maxRadiusY = Math.Min(y, canvasHeight - 1 - y);
                if (radiusY < 1 || radiusY > maxRadiusY)
                {
                    PrintMessage($"Radius along Y must be between 1 and {maxRadiusY}.");
                    return false;
                }

                PrintMessage("Symbol for ellipse (Enter = *):");
                var symInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

                PrintMessage("Color (Enter = White):");
                var colInput = ReadLineAt(canvasHeight + 5);
                
                if (string.IsNullOrEmpty(colInput)) return true;
                if (!Enum.TryParse(colInput, true, out color))
                    color = ConsoleColor.White;

                return true;
            }
            finally
            {
                TempPointsRemove(new[] { tempPoint });
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
                PrintMessage("Enter coordinates for the first vertex (X1):");
                if (!TryReadIntInRange(out x1, 0, canvasWidth - 1)) return false;
                PrintMessage("Enter coordinates for the first vertex (Y1):");
                if (!TryReadIntInRange(out y1, 0, canvasHeight - 1)) return false;
                TempPointDraw(x1, y1, out tempPoint1);

                PrintMessage("Enter coordinates for the second vertex (X2):");
                if (!TryReadIntInRange(out x2, 0, canvasWidth - 1)) return false;
                PrintMessage("Enter coordinates for the second vertex (Y2):");
                if (!TryReadIntInRange(out y2, 0, canvasHeight - 1)) return false;
                TempPointDraw(x2, y2, out tempPoint2);

                PrintMessage("Enter coordinates for the third vertex (X3):");
                if (!TryReadIntInRange(out x3, 0, canvasWidth - 1)) return false;
                PrintMessage("Enter coordinates for the third vertex (Y3):");
                if (!TryReadIntInRange(out y3, 0, canvasHeight - 1)) return false;
                TempPointDraw(x3, y3, out tempPoint3);

                PrintMessage("Symbol for triangle (Enter = *):");
                var symInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

                PrintMessage("Color (Enter = White):");
                var colInput = ReadLineAt(canvasHeight + 5);
                
                if (string.IsNullOrEmpty(colInput)) return true;
                if (!Enum.TryParse(colInput, true, out color))
                    color = ConsoleColor.White;
                return true;
            }
            finally
            {
                TempPointsRemove(new[] { tempPoint1, tempPoint2, tempPoint3 });
            }
        }

        private bool PromptLineInput(out int x1, out int y1, out int x2, out int y2,
                               out char symbol, out ConsoleColor color)
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
                var colInput = ReadLineAt(canvasHeight + 5);
                
                if (string.IsNullOrEmpty(colInput)) return true;
                if (!Enum.TryParse(colInput, true, out color))
                    color = ConsoleColor.White;
                return true;
            }
            finally
            {
                TempPointsRemove(new[] { tempPoint1, tempPoint2 });
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
            var symInput = ReadLineAt(canvasHeight + 5);
            if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

            PrintMessage("Color (Enter = White):");
            var colInput = ReadLineAt(canvasHeight + 5);
            
            if (string.IsNullOrEmpty(colInput)) return true;
            if (!Enum.TryParse(colInput, true, out color))
                color = ConsoleColor.White;
            return true;
        }

        private bool PromptRectangleInput(out int x1, out int y1, out int x2, out int y2,
                                    out char symbol, out ConsoleColor color)
        {
            x1 = y1 = x2 = y2 = default;
            symbol = '#';
            color = ConsoleColor.White;
            Shape tempPoint1, tempPoint2;
            tempPoint1 = tempPoint2 = null!;
            try
            {
                PrintMessage("Enter X of the first vertex:");
                if (!TryReadIntInRange(out x1, 0, canvasWidth - 1)) return false;

                PrintMessage("Enter Y of the first vertex:");
                if (!TryReadIntInRange(out y1, 0, canvasHeight - 1)) return false;
                TempPointDraw(x1, y1, out tempPoint1);

                PrintMessage("Enter X of the second vertex:");
                if (!TryReadIntInRange(out x2, 0, canvasWidth - 1)) return false;

                PrintMessage("Enter Y of the second vertex:");
                if (!TryReadIntInRange(out y2, 0, canvasHeight - 1)) return false;
                TempPointDraw(x2, y2, out tempPoint2);

                PrintMessage("Symbol for rectangle (Enter = #):");
                var symInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

                PrintMessage("Color (Enter = White):");
                var colInput = ReadLineAt(canvasHeight + 5);
                
                if (string.IsNullOrEmpty(colInput)) return true;
                if (!Enum.TryParse(colInput, true, out color))
                    color = ConsoleColor.White;
                return true;
            }
            finally
            {
                TempPointsRemove(new[] { tempPoint1, tempPoint2 });
            }
        }

        private bool TryReadIntInRange(out int result, int min, int max)
        {
            if (!TryReadInt(out result))
                return false;
            if (result >= min && result <= max) return true;
            
            PrintMessage($"Value must be between {min} and {max}.");
            return false;

            bool TryReadInt(out int result)
            {
                FlushInput();
                var row = canvasHeight + 5;
                var input = ReadLineAt(row);
                
                if (int.TryParse(input, out result)) return true;
                PrintMessage("Input error (not an integer)!");
                ReadLineAt(row);  // Wait for Enter
                return false;
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
            var filename = ReadLineAt(canvasHeight + 5);
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
