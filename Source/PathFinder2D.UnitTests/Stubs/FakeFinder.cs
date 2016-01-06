using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Finder;

namespace PathFinder2D.UnitTests.Stubs
{
    public class FakeFinder : BaseFinder
    {
        protected override FinderResult Find(WorldPosition start, WorldPosition end)
        {
            return new FinderResult(null);
        }
    }
}