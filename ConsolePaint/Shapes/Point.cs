namespace ConsolePaint.Shapes
{
    public class Point : Shape
    {
        private int x, y;
        private char symbol;
        private ConsoleColor color;

        public Point(int x, int y, char symbol, ConsoleColor color)
            : base()
        {
            this.x = x;
            this.y = y;
            this.symbol = symbol;
            this.color = color;
            CalculatePixels();  
        }
        
        public override void CalculatePixels()
        {
          
            OuterPixels.Clear();
            InnerPixels.Clear(); 
            
            OuterPixels.Add(new Pixel(x, y, symbol, color));
        }
    }
}
