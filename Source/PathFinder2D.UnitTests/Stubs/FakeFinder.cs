using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Finder;

namespace PathFinder2D.UnitTests.Stubs
{
    public class FakeFinder : BaseFinder<FakeFinderPoint>
    {
        protected override FakeFinderPoint[] Find(WorldPosition start, WorldPosition end, SearchOptions options = SearchOptions.None)
        {
            return null;
        }
    }
}