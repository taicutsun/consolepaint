namespace ConsolePaint.Shapes
{
    public class Triangle : Shape
    {
        private int x1, y1, x2, y2, x3, y3;

        public Triangle() : base() { }
        
        public Triangle(int x1, int y1, int x2, int y2, int x3, int y3, char symbol, ConsoleColor color)
            : base(symbol, color)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            this.x3 = x3;
            this.y3 = y3;
            CalculatePixels();
        }

        protected override void CalculatePixels()
        {
            OuterPixels.Clear();
            InnerPixels.Clear();

            DrawLine(x1, y1, x2, y2);
            DrawLine(x2, y2, x3, y3);
            DrawLine(x3, y3, x1, y1);

            var minY = Math.Min(y1, Math.Min(y2, y3));
            var maxY = Math.Max(y1, Math.Max(y2, y3));

            for (var y = minY + 1; y < maxY; y++)
            {
                List<double> nodeX = [];

                ComputeIntersection(x1, y1, x2, y2, y, nodeX);
                ComputeIntersection(x2, y2, x3, y3, y, nodeX);
                ComputeIntersection(x3, y3, x1, y1, y, nodeX);

                if (nodeX.Count < 2) continue;
                nodeX.Sort();

                for (var i = 0; i < nodeX.Count; i += 2)
                {
                    if (i + 1 >= nodeX.Count)
                        break;
                    var startX = (int)Math.Ceiling(nodeX[i]);
                    var endX = (int)Math.Floor(nodeX[i + 1]);
                    for (var x = startX; x <= endX; x++)
                    {
                        if (!OuterPixels.Any(p => p.X == x && p.Y == y))
                        {
                            InnerPixels.Add(new Pixel(x, y, ' ', Color));
                        }
                    }
                }
            }
        }

        private void DrawLine(int x1, int y1, int x2, int y2)
        {
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = (x1 < x2) ? 1 : -1;
            int sy = (y1 < y2) ? 1 : -1;
            int err = dx - dy;

            int cx = x1, cy = y1;
            while (true)
            {
                OuterPixels.Add(new Pixel(cx, cy, Symbol, Color));

                if (cx == x2 && cy == y2)
                    break;

                var e2 = err * 2;
                if (e2 > -dy)
                {
                    err -= dy;
                    cx += sx;
                }

                if (e2 >= dx) continue;
                err += dx;
                cy += sy;
            }
        }

        private static void ComputeIntersection(int x1, int y1, int x2, int y2, int scanlineY, List<double> nodeX)
        {
            if ((y1 >= scanlineY || y2 < scanlineY) && (y2 >= scanlineY || y1 < scanlineY)) return;
            var xIntersection = x1 + (scanlineY - y1) * (x2 - x1) / (double)(y2 - y1);
            nodeX.Add(xIntersection);
        }
    }
}
