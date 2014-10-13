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

        private readonly IDictionary<int, Map> _registeredMaps;
        private readonly IDictionary<Type, IFinder> _registeredFinders;

        #endregion

        #region Constructors

        public PathFinderService()
        {
            _registeredMaps = new Dictionary<int, Map>();
            _registeredFinders = new Dictionary<Type, IFinder>();
        }

        #endregion

        #region Service Locator Methods

        public void RegisterFinder<TFinder>(TFinder instance) where TFinder : class, IFinder
        {
            if (instance == null)
            {
                throw new NullReferenceException();
            }

            if (!_registeredFinders.ContainsKey(typeof(TFinder)))
            {
                _registeredFinders.Add(typeof(TFinder), instance);
            }
        }

        public TFinder ResolveFinder<TFinder>() where TFinder : class, IFinder
        {
            if (_registeredFinders == null || !_registeredFinders.ContainsKey(typeof (TFinder)))
            {
                return null;
            }

            return _registeredFinders[typeof(TFinder)] as TFinder;
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

        public FinderResult Find<TFinder>(int terrainId, Vector3 start, Vector3 end) where TFinder : class, IFinder
        {
            if (_registeredMaps == null || !_registeredMaps.ContainsKey(terrainId))
            {
                throw new ArgumentException(string.Format("Map with id = '{0}' not initialized", terrainId));
            }

            var finder = ResolveFinder<TFinder>();
            if (finder == null)
            {
                throw new ArgumentException(string.Format("{0} finder type not registered", typeof(TFinder).Name));
            }

            var map = _registeredMaps[terrainId];
            var result = finder.Find(map, start, end);
            map.LastFinderResult = result;

            return result;
        }

        #endregion
    }
}