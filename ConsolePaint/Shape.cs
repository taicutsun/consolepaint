namespace ConsolePaint
{
    public abstract class Shape
    {
        private static int idCounter = 0;
        public int Id { get; set; }

        public char Symbol { get; set; }
        public ConsoleColor Color { get; set; }

        public List<Pixel> OuterPixels { get; set; }
        public List<Pixel> InnerPixels { get; set; }

        public Shape()
        {
            Id = idCounter++;
            OuterPixels = [];
            InnerPixels = [];
            Symbol = ' ';              
            Color = ConsoleColor.White;
        }

        protected Shape(char symbol, ConsoleColor color)
        {
            Id = idCounter++;
            OuterPixels = [];
            InnerPixels = [];
            Symbol = symbol;
            Color = color;
        }
        
        protected abstract void CalculatePixels();
        
        public virtual void Move(int dx, int dy)
        {
            for (var i = 0; i < OuterPixels.Count; i++)
            {
                OuterPixels[i].X += dx;
                OuterPixels[i].Y += dy;
            }
            for (var i = 0; i < InnerPixels.Count; i++)
            {
                InnerPixels[i].X += dx;
                InnerPixels[i].Y += dy;
            }
        }
       
        public virtual bool ContainsPoint(int x, int y)
        {
            foreach (var p in OuterPixels)
            {
                if (p.X == x && p.Y == y)
                    return true;
            }
            foreach (var p in InnerPixels)
            {
                if (p.X == x && p.Y == y)
                    return true;
            }
            return false;
        }
    }
}
