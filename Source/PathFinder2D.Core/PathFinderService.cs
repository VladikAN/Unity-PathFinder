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
        private readonly IDictionary<Type, IGizmo> _registeredGizmos;

        #endregion

        #region Constructors

        public PathFinderService()
        {
            _registeredMaps = new Dictionary<int, Map>();
            _registeredFinders = new Dictionary<Type, IFinder>();
            _registeredGizmos = new Dictionary<Type, IGizmo>();
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

        public void RegisterGizmo<TGizmo>(TGizmo instance) where TGizmo : class, IGizmo
        {
            if (instance == null)
            {
                throw new NullReferenceException();
            }

            if (!_registeredGizmos.ContainsKey(typeof(TGizmo)))
            {
                _registeredGizmos.Add(typeof(TGizmo), instance);
            }
        }

        public TGizmo ResolveGizmo<TGizmo>() where TGizmo : class, IGizmo
        {
            if (_registeredGizmos == null || !_registeredGizmos.ContainsKey(typeof(TGizmo)))
            {
                return null;
            }

            return _registeredGizmos[typeof(TGizmo)] as TGizmo;
        }

        #endregion

        #region Finder Methods

        public int InitMap(GameObject terrain, uint cellWidth)
        {
            if (terrain == null)
            {
                throw new ArgumentException("Null object not supported as GameObject parameter");
            }

            if (cellWidth == 0)
            {
                throw new ArgumentException("Cell width must be greater then 0");
            }

            var terrainKey = terrain.GetInstanceID();
            if (_registeredMaps.ContainsKey(terrainKey))
            {
                throw new ArgumentException("This GameObject already initialized as map");
            }

            _registeredMaps.Add(terrainKey, new Map(terrain, cellWidth));
            return terrainKey;
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