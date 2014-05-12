namespace Assets.Script.PathFinder.Finder2D.Finder
{
    public abstract class BasePoint
    {
        public int X;
        public int Y;

        protected BasePoint()
        {
        }

        protected BasePoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}