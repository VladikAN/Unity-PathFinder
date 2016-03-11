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
        /// <summary>Returns all registered maps</summary>
        IDictionary<int, MapDefinition> GetMaps();

        /// <summary>Register new map into service</summary>
        /// <param name="floor">Floor to register</param>
        /// <param name="cellSize">Floor cell size. Floor will be divided by this value</param>
        MapDefinition InitMap(IFloor floor, float cellSize);

        /// <summary>Find path from point to point</summary>
        /// <param name="floorId">target floor id</param>
        /// <param name="start">Start position</param>
        /// <param name="end">Target position</param>
        /// <param name="options">Search options</param>
        PathResult FindPath(int floorId, WorldPosition start, WorldPosition end, SearchOptions options = SearchOptions.None);
    }
}