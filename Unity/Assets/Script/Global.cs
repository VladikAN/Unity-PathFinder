using PathFinder2D.Core;
using PathFinder2D.Core.Finder;
using PathFinder2D.Core.Initializer;

namespace Assets.Script
{
    public static class Global
    {
        static Global()
        {
            PathFinderService = new PathFinderService(new JumpPointFinder(), new MapInitializer());
        }

        public static IPathFinderService PathFinderService { get; private set; }
    }
}