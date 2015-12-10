using System.Collections.Generic;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Domain.Terrain;
using UnityEngine;

namespace PathFinder2D.Core
{
    public interface IPathFinderService
    {
        IDictionary<int, MapDefinition> GetMaps();
        MapDefinition InitMap(ITerrain terrain, float cellSize);
        FinderResult FindPath(int terrainId, Vector3 start, Vector3 end);
    }
}