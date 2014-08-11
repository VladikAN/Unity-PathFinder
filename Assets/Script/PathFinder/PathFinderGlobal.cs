using System;
using System.Collections.Generic;
using Assets.Script.PathFinder.Finder2D.Core;
using Assets.Script.PathFinder.Finder2D.Finder;
using UnityEngine;

namespace Assets.Script.PathFinder
{
    public static class PathFinderGlobal
    {
        private static Cell[,] _terrainField;
        private static IDictionary<Type, BaseFinder> _registeredFinders;
        private static IDictionary<Type, BaseGizmo> _registeredGizmos;

        #region Cell

        public static uint CellWidth = 1;
        public static float CellCorrection { get { return (float)CellWidth / 2; } }

        #endregion

        #region Registration

        public static void RegisterFinder<TFinder>(TFinder finder)
            where TFinder : BaseFinder
        {
            var finderType = typeof (TFinder);
            if (_registeredFinders == null)
            {
                _registeredFinders = new Dictionary<Type, BaseFinder>();
            }

            if (_registeredFinders.ContainsKey(finderType))
            {
                return; /* finder already registered and new registration will be ignored */
            }

            _registeredFinders.Add(finderType, finder);
        }

        public static void RegisterGizmo<TResult, TGizmo>(TGizmo gizmo)
            where TResult : BaseResult
            where TGizmo : BaseGizmo
        {
            var resultType = typeof(TResult);
            if (_registeredGizmos == null)
            {
                _registeredGizmos = new Dictionary<Type, BaseGizmo>();
            }

            if (_registeredGizmos.ContainsKey(resultType))
            {
                return; /* Gizmo already registered and new registration will be ignored */
            }

            _registeredGizmos.Add(resultType, gizmo);
        }

        #endregion

        #region Terrain

        public static GameObject Terrain;
        public static Cell[,] TerrainField
        {
            get
            {
                if (Terrain != null && _terrainField == null)
                {
                    var terrainWidth = Terrain.renderer.bounds.extents.x * 2;
                    var terrainHeight = Terrain.renderer.bounds.extents.z * 2;

                    var x = ((int)terrainWidth) / CellWidth;
                    var z = ((int)terrainHeight) / CellWidth;

                    _terrainField = new Cell[x, z];
                    for (var i = 0; i < x; i++)
                    {
                        for (var j = 0; j < z; j++)
                        {
                            _terrainField[i, j] = new Cell();
                        }
                    }
                }

                return _terrainField;
            }
        }
        public static int TerrainFieldWidth { get { return TerrainField.GetLength(0); } }
        public static int TerrainFieldHeight { get { return TerrainField.GetLength(1); } }
        public static float TerrainGameObjectStartX { get { return Terrain.transform.position.x - Terrain.renderer.bounds.extents.x; } }
        public static float TerrainGameObjectStartZ { get { return Terrain.transform.position.z - Terrain.renderer.bounds.extents.z; } }

        #endregion

        #region BaseFinder

        public static BaseResult LastResult = null;
        public static BaseResult Find<TFinder>(Vector3 start, Vector3 end)
            where TFinder : BaseFinder
        {
            var finderType = typeof(TFinder);
            if (_registeredFinders != null && !_registeredFinders.ContainsKey(finderType))
            {
                Debug.LogError(string.Format("{0} nor registered", finderType.Name));
                return null;
            }

            var result = _registeredFinders[finderType].Find(start, end);
            LastResult = result;

            return result;
        }

        #endregion

        #region BaseGizmo

        public static BaseGizmo GetFinderGizmo(Type resultType)
        {
            if (_registeredGizmos != null && _registeredGizmos.ContainsKey(resultType))
            {
                return _registeredGizmos[resultType];
            }

            return null;
        }

        #endregion
    }
}