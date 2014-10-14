using System;
using System.Collections.Generic;
using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Finder;
using UnityEngine;

namespace PathFinder2D.Core
{
    public class PathFinderService : IPathFinderService
    {
        #region Fields

        private readonly IFinder _finder;
        private readonly IDictionary<int, Map> _registeredMaps;

        #endregion

        #region Constructors

        public PathFinderService(IFinder finder)
        {
            if (finder == null)
            {
                throw new ArgumentException("Null object not supported as Finder");
            }

            _finder = finder;
            _registeredMaps = new Dictionary<int, Map>();
        }

        #endregion

        #region Finder Methods

        public Map InitMap(GameObject terrain, float cellWidth)
        {
            if (terrain == null)
            {
                throw new ArgumentException("Null object not supported as GameObject parameter");
            }

            if (cellWidth <= 0)
            {
                throw new ArgumentException("Cell width must be greater then 0");
            }

            var terrainKey = terrain.GetInstanceID();
            if (_registeredMaps.ContainsKey(terrainKey))
            {
                throw new ArgumentException("This GameObject already initialized as map");
            }

            _registeredMaps.Add(terrainKey, new Map(terrain, cellWidth));
            return _registeredMaps[terrainKey];
        }

        public FinderResult Find(int terrainId, Vector3 start, Vector3 end)
        {
            if (_registeredMaps == null || !_registeredMaps.ContainsKey(terrainId))
            {
                throw new ArgumentException(string.Format("Map with id = '{0}' not initialized", terrainId));
            }

            var map = _registeredMaps[terrainId];
            var result = _finder.Find(map, start, end);
            map.LastFinderResult = result;

            return result;
        }

        #endregion
    }
}