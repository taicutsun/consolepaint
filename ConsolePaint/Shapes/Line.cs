namespace ConsolePaint.Shapes
{
    class Line(int x1, int y1, int x2, int y2, char symbol) : Shape
    {
        public override void Draw(Canvas canvas)
        {
            if (x1 == x2)
            {
                for (int y = y1; y <= y2; y++)
                {
                    canvas.SetPixel(x1, y, symbol);
                }
            }
            else if (y1 == y2) 
            {
                for (int x = x1; x <= x2; x++)
                {
                    canvas.SetPixel(x, y1, symbol);
                }
            }
        }
    }
}
