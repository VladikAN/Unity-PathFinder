using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Domain.Terrain;
using PathFinder2D.Core.Finder;
using PathFinder2D.Core.Initializer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinder2D.Core
{
    public class PathFinderService : IPathFinderService
    {
        #region Fields

        private readonly IDictionary<int, MapDefinition> _maps;
        private readonly IFinder _finder;
        private readonly IMapInitializer _initializer;

        #endregion

        #region Constructors

        public PathFinderService(IFinder finder, IMapInitializer initializer)
        {
            if (finder == null) throw new ArgumentException("Null object not supported as Finder");
            if (initializer == null) throw new ArgumentException("Null object not supported as MapInitializer");

            _finder = finder;
            _initializer = initializer;
            _maps = new Dictionary<int, MapDefinition>();
        }

        #endregion

        #region Methods

        public IDictionary<int, MapDefinition> GetMaps()
        {
            return _maps;
        }

        public MapDefinition InitMap(ITerrain terrain, float cellSize)
        {
            if (terrain == null) throw new ArgumentException("Null object not supported as terrain parameter");
            if (cellSize <= 0) throw new ArgumentException("Cell width must be greater then 0");

            var terrainKey = terrain.Id();
            if (_maps.ContainsKey(terrainKey))
            {
                throw new ArgumentException("This GameObject already initialized as map");
            }

            var field = _initializer.ParseMapCells(terrain, cellSize);
            var mapDefinition = new MapDefinition(terrain, field, cellSize);
            _maps.Add(terrainKey, mapDefinition);
            
            return mapDefinition;
        }

        public FinderResult FindPath(int terrainId, Vector3 start, Vector3 end)
        {
            if (_maps == null || !_maps.ContainsKey(terrainId))
                throw new ArgumentException(string.Format("Map with id = '{0}' not initialized", terrainId));

            var mapDefinition = _maps[terrainId];
            var result = _finder.Find(mapDefinition, start, end);
            mapDefinition.LastFinderResult = result;

            return result;
        }

        #endregion
    }
}