namespace ThePathofKnight
{
    public class KnightPosition
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Distance { get; set; }
        public KnightPosition Parent { get; set; }

        public KnightPosition(int x, int y, int distance = 0, KnightPosition parent = null)
        {
            X = x;
            Y = y;
            Distance = distance;
            Parent = parent;
        }
    }
}
