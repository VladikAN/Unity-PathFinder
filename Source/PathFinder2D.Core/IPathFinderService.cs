﻿using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Domain.Terrain;
using UnityEngine;

namespace PathFinder2D.Core
{
    public interface IPathFinderService
    {
        MapDefinition InitMap(ITerrain terrain, float cellSize);
        FinderResult Find(int terrainId, Vector3 start, Vector3 end);
    }
}