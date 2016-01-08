namespace PathFinder2D.Core.Domain
{
    public class WorldPosition
    {
        public WorldPosition(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }
    }
}