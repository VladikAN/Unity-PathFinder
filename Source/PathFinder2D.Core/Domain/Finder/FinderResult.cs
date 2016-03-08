namespace PathFinder2D.Core.Domain.Finder
{
    public class FinderResult
    {
        public WorldPosition[] Path;

        public FinderResult(WorldPosition[] path)
        {
            Path = path;
        }
    }
}