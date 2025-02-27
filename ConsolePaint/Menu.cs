using ConsolePaint.Shapes;

namespace ConsolePaint
{
    public class Menu
    {
        private Canvas canvas;
        private List<Shape> shapes;  

        public Menu(Canvas canvas)
        {
            this.canvas = canvas;
            this.shapes = new List<Shape>();
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                DrawMenu();  

                canvas.DrawFrame();
                canvas.DrawShapes(shapes);

                string choice = Console.ReadLine().ToLower();

                switch (choice)
                {
                    case "1":
                        HandleDrawCommand();
                        break;
                    case "2":
                        canvas.Clear();
                        shapes.Clear();
                        break;
                    case "3":
                        return;  
                    default:
                        Console.WriteLine("error.try again.");
                        break;
                }
            }
        }

        private void DrawMenu()
        {
            Console.WriteLine("choose command:");
            Console.WriteLine("1.Paint an figure (draw)");
            Console.WriteLine("2. Clear holst (clear)");
            Console.WriteLine("3. Exit (exit)");
            Console.WriteLine(); 
        }

        private void HandleDrawCommand()
        {
            Console.Clear();
            DrawMenu(); 

            Console.WriteLine("Choose a shape:");
            Console.WriteLine("1. Line");
            Console.WriteLine("2. Dot");
            Console.WriteLine("3. Rectangle");

            string shapeChoice = Console.ReadLine().ToLower();

            Shape shape = null;
            switch (shapeChoice)
            {
                case "1":
                    shape = CreateLine(); 
                    break;
                case "2":
                    shape = CreatePoint();  
                    break;
                case "3":
                    shape = CreateRectangle(); 
                    break;
                default:
                    Console.WriteLine("error. Try again.");
                    return;
            }

            shapes.Add(shape);  
            canvas.Draw(shape); 
        }

        // Методы для создания фигур (аналогично предыдущим)
        private Shape CreateLine()
        {
            Console.WriteLine("Enter cord of line's start (x1, y1):");
            int x1 = int.Parse(Console.ReadLine());
            int y1 = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter cord of line's end (x2, y2):");
            int x2 = int.Parse(Console.ReadLine());
            int y2 = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter line symbol ('*'):");
            char symbol = Console.ReadLine().Length > 0 ? Console.ReadLine()[0] : '*';

            Console.WriteLine("enter color for line (White):");
            string colorInput = Console.ReadLine();
            ConsoleColor color = Enum.TryParse(colorInput, true, out color) ? color : ConsoleColor.White;

            return ShapeFactory.CreateLine(x1, y1, x2, y2, symbol, color);
        }

        private Shape CreatePoint()
        {
            Console.WriteLine("Enter cod dot (x, y):");
            int x = int.Parse(Console.ReadLine());
            int y = int.Parse(Console.ReadLine());

            Console.WriteLine("enter symbol for dot (*):");
            char symbol = Console.ReadLine().Length > 0 ? Console.ReadLine()[0] : '*';

            Console.WriteLine("enter color for dot (White):");
            string colorInput = Console.ReadLine();
            ConsoleColor color = Enum.TryParse(colorInput, true, out color) ? color : ConsoleColor.White;

            return ShapeFactory.CreatePoint(x, y, symbol, color);
        }

        private Shape CreateRectangle()
        {
            Console.WriteLine("enter cord for  top left angle (x1, y1):");
            int x1 = int.Parse(Console.ReadLine());
            int y1 = int.Parse(Console.ReadLine());

            Console.WriteLine("enter cord for  bottom left angle  (x2, y2):");
            int x2 = int.Parse(Console.ReadLine());
            int y2 = int.Parse(Console.ReadLine());

            Console.WriteLine("enter symbol for rectangle ('#'):");
            char symbol = Console.ReadLine().Length > 0 ? Console.ReadLine()[0] : '#';

            Console.WriteLine("enter rectangle for rectangle (White):");
            string colorInput = Console.ReadLine();
            ConsoleColor color = Enum.TryParse(colorInput, true, out color) ? color : ConsoleColor.White;

            // Используем фабрику для создания прямоугольника
            return ShapeFactory.CreateRectangle(x1, y1, x2, y2, symbol, color);
        }
    }
}
