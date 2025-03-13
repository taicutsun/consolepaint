namespace ConsolePaint
{
    public class Canvas : ICanvas
    {
        private int width;
        private int height;
        private Pixel[,] pixels;
        private List<Shape> shapes;  

        public int Height => height;
        public int Width => width;
        
        public List<Shape> Shapes => shapes;

        public Canvas(int width, int height)
        {
            this.width = width;
            this.height = height;
            pixels = new Pixel[width, height];
            shapes = new List<Shape>();

        }
        
        public void DrawFrame()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(0);
            for (var i = 0; i < width; i++)
            {
                Console.Write("+");
            }
            Console.Write(width);
            Console.WriteLine();

            for (var y = 0; y < height; y++)
            {
                Console.SetCursorPosition(0, y + 1);
                Console.Write("+");
                for (var x = 0; x < width; x++)
                {
                    Console.Write(" ");
                }
                Console.Write("+");
            }

            Console.SetCursorPosition(0, height + 1);
            Console.Write(height);
            for (var i = 0; i < width; i++)
            {
                Console.Write("+");
            }
            Console.WriteLine();
        }

        public void Draw(Shape shape)
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

        public void Clear()
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
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
            shapes.Add(shape);
            Draw(shape);
        }

        public void RemoveShape(Shape shape)
        {
            shapes.Remove(shape);
            RedrawAllShapes();
        }

        public void RedrawAllShapes()
        {
            Clear();
            foreach (var shape in shapes)
            {
                Draw(shape);

                if (shape.InnerPixels.Any(p => p.Symbol != ' '))
                {
                    foreach (var p in shape.InnerPixels)
                    {
                        SetPixel(p.X, p.Y, p.Symbol, p.Color);
                    }
                }
            }
        }


        public void SetPixel(int x, int y, char symbol, ConsoleColor color)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return;
            
            pixels[x, y] = new Pixel(x, y, symbol, color);
            Console.SetCursorPosition(x + 1, y + 1);
            Console.ForegroundColor = color;
            Console.Write(symbol);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public Pixel GetPixel(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                return pixels[x, y] ?? new Pixel(x, y, ' ', ConsoleColor.Black);
            }
            return new Pixel(x, y, ' ', ConsoleColor.Black);
        }
    }
}
