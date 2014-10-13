namespace PathFinder2D.Core.Domain
{
    public abstract class FinderPoint
    {
        public int X;
        public int Y;

        protected FinderPoint()
        {
        }

        protected FinderPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}