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

        private readonly IDictionary<int, MapDefinition> _registeredMaps;
        private readonly IFinder _finder;
        private readonly IMapInitializer _mapInitializer;

        #endregion

        #region Constructors

        public PathFinderService(IFinder finder, IMapInitializer mapInitializer)
        {
            if (finder == null)
            {
                throw new ArgumentException("Null object not supported as Finder");
            }

            if (mapInitializer == null)
            {
                throw new ArgumentException("Null object not supported as MapInitializer");
            }

            _finder = finder;
            _mapInitializer = mapInitializer;
            _registeredMaps = new Dictionary<int, MapDefinition>();
        }

        #endregion

        #region Methods

        public IDictionary<int, MapDefinition> RegisteredMaps()
        {
            return _registeredMaps;
        }

        public MapDefinition InitMap(ITerrain terrain, float cellSize)
        {
            if (terrain == null)
            {
                throw new ArgumentException("Null object not supported as GameObject parameter");
            }

            if (cellSize <= 0)
            {
                throw new ArgumentException("Cell width must be greater then 0");
            }

            var terrainKey = terrain.Id();
            if (_registeredMaps.ContainsKey(terrainKey))
            {
                throw new ArgumentException("This GameObject already initialized as map");
            }

            var field = _mapInitializer.ParceMapCells(terrain, cellSize);
            var mapDefinition = new MapDefinition(terrain, field, cellSize);
            _registeredMaps.Add(terrainKey, mapDefinition);
            
            return mapDefinition;
        }

        public FinderResult Find(int terrainId, Vector3 start, Vector3 end)
        {
            if (_registeredMaps == null || !_registeredMaps.ContainsKey(terrainId))
            {
                throw new ArgumentException(string.Format("Map with id = '{0}' not initialized", terrainId));
            }

            var mapDefinition = _registeredMaps[terrainId];
            var result = _finder.Find(mapDefinition, start, end);
            mapDefinition.LastFinderResult = result;

            return result;
        }

        #endregion
    }
}