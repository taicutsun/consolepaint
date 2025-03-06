namespace ConsolePaint
{
    public class Canvas
    {
        private Pixel[,] pixels;

        private int Height { get; }

        private int Width { get; }

        public List<Shape> Shapes { get; }

        public Canvas(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            pixels = new Pixel[width, height];
            Shapes = new List<Shape>();

        }

        public void DrawFrame()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(0);
            for (var i = 0; i < Width; i++)
            {
                Console.Write("+");
            }
            Console.Write(Width);
            Console.WriteLine();

            for (var y = 0; y < Height; y++)
            {
                Console.SetCursorPosition(0, y + 1);
                Console.Write("+");
                for (var x = 0; x < Width; x++)
                {
                    Console.Write(" ");
                }
                Console.Write("+");
            }

            Console.SetCursorPosition(0, Height + 1);
            Console.Write(Height);
            for (var i = 0; i < Width; i++)
            {
                Console.Write("+d");
            }
            Console.WriteLine();
        }

        private void Draw(Shape shape)
        {
            foreach (var p in shape.OuterPixels)
            {
                SetPixel(p.X, p.Y, p.Symbol, p.Color);
            }
        }

        public void Fill(Shape shape)
        {
            foreach (var p in shape.InnerPixels)
            {
                SetPixel(p.X, p.Y, p.Symbol, p.Color);
            }
        }

        private void Clear()
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    pixels[x, y] = new Pixel(x, y, ' ', ConsoleColor.Black);
                    Console.SetCursorPosition(x + 1, y + 1);
                    Console.Write(' ');
                }
            }
        }

        public void EraseShape(Shape shape)
        {
            foreach (var p in shape.OuterPixels)
            {
                SetPixel(p.X, p.Y, ' ', ConsoleColor.Black);
            }
            foreach (var p in shape.InnerPixels)
            {
                SetPixel(p.X, p.Y, ' ', ConsoleColor.Black);
            }
        }

        public void AddShape(Shape shape)
        {
            Shapes.Add(shape);
            Draw(shape);
        }

        public void RemoveShape(Shape shape)
        {
            Shapes.Remove(shape);
            RedrawAllShapes();
        }

        public void RedrawAllShapes()
        {
            Clear();
            foreach (var shape in Shapes)
            {
                Draw(shape);

                if (!shape.InnerPixels.Any(p => p.Symbol != ' ')) continue;
                {
                    foreach (var p in shape.InnerPixels)
                    {
                        SetPixel(p.X, p.Y, p.Symbol, p.Color);
                    }
                }
            }
        }

        private void SetPixel(int x, int y, char symbol, ConsoleColor color)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return;
            
            pixels[x, y] = new Pixel(x, y, symbol, color);
            Console.SetCursorPosition(x + 1, y + 1);
            Console.ForegroundColor = color;
            Console.Write(symbol);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public Pixel GetPixel(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return pixels[x, y] ?? new Pixel(x, y, ' ', ConsoleColor.Black);
            }
            return new Pixel(x, y, ' ', ConsoleColor.Black);
        }
    }
}
