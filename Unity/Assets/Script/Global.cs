using PathFinder2D.Core;
using PathFinder2D.Core.Finder;
using PathFinder2D.Core.Initializer;

namespace Assets.Script
{
    public static class Global
    {
        static Global()
        {
            PathService = new PathService(new WaveFinder(), new Map());
        }

        public static IPathService PathService { get; private set; }
    }
}