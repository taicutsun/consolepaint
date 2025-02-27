using ConsolePaint.Shapes;

namespace ConsolePaint
{
    public class Program
    {
        static void Main(string[] args)
        {
            Canvas canvas = new Canvas(50, 20);

            Menu menu = new Menu(canvas);
            menu.ShowMenu();
        }
    }
}
