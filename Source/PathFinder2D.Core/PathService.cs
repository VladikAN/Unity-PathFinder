using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Domain.Terrain;
using PathFinder2D.Core.Finder;
using PathFinder2D.Core.Initializer;
using System;
using System.Collections.Generic;

namespace PathFinder2D.Core
{
    public class PathService : IPathService
    {
        #region Fields

        private readonly IDictionary<int, MapDefinition> _maps;
        private readonly IFinder _finder;
        private readonly IMap _initializer;

        #endregion

        #region Constructors

        public PathService(IFinder finder, IMap initializer)
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

        public MapDefinition InitMap(IFloor terrain, float cellSize)
        {
            if (terrain == null) throw new ArgumentException("Null object not supported as terrain parameter");
            if (cellSize <= 0) throw new ArgumentException("Cell width must be greater then 0");
            if (_maps.ContainsKey(terrain.Id())) throw new ArgumentException("This GameObject already initialized as map");

            var field = _initializer.ParseMapCells(terrain, cellSize);
            var mapDefinition = new MapDefinition(terrain, field, cellSize);
            _maps.Add(terrain.Id(), mapDefinition);
            
            return mapDefinition;
        }

        public PathResult FindPath(int terrainId, WorldPosition start, WorldPosition end, SearchOptions options = SearchOptions.None)
        {
            if (_maps == null || !_maps.ContainsKey(terrainId))
                throw new ArgumentException(string.Format("Map with id = '{0}' not initialized", terrainId));

            var mapDefinition = _maps[terrainId];
            var result = _finder.Find(mapDefinition, start, end, options);
            mapDefinition.LastFinderResult = result;

            return result;
        }

        #endregion
    }
}