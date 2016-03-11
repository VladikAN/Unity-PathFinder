using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Finder;

namespace PathFinder2D.UnitTests.Stubs
{
    public class FakeFinder : Finder<FakePoint>
    {
        protected override FakePoint[] Find(WorldPosition start, WorldPosition end, SearchOptions options = SearchOptions.None)
        {
            return null;
        }
    }
}