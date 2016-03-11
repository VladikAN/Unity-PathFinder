using System.Collections.Generic;
using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Domain.Terrain;
using PathFinder2D.Core.Finder;

namespace PathFinder2D.Core
{
    public interface IPathService
    {
        IDictionary<int, MapDefinition> GetMaps();
        MapDefinition InitMap(IFloor terrain, float cellSize);
        PathResult FindPath(int terrainId, WorldPosition start, WorldPosition end, SearchOptions options = SearchOptions.None);
    }
}