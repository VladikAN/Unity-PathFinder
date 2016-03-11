namespace PathFinder2D.Core.Domain.Finder
{
    public class PathResult
    {
        public WorldPosition[] Path;

        public PathResult(WorldPosition[] path)
        {
            Path = path;
        }
    }
}