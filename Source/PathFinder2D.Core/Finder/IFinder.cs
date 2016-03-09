using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;

namespace PathFinder2D.Core.Finder
{
    public interface IFinder
    {
        FinderResult Find(MapDefinition mapDefinition, WorldPosition start, WorldPosition end, SearchOptions options = SearchOptions.None);
    }
}