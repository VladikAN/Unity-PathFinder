using System.Collections.Generic;
using System.Linq;
using PathFinder2D.Core.Domain.Terrain;
using UnityEngine;

namespace PathFinder2D.Unity.Domain
{
    public class TerrainGameObject : ITerrain
    {
        private readonly GameObject _gameObject;
        private readonly float _cellSize;
        private readonly Bounds _bounds;

        public TerrainGameObject(GameObject gameObject, float cellSize)
        {
            _gameObject = gameObject;
            _cellSize = cellSize;
            _bounds = new Bounds();

            getTerrainBounds(_gameObject, ref _bounds);
        }

        #region ITerrain

        public int Id() 
        {
            return _gameObject.GetInstanceID();
        }

        public float X()
        {
            return _bounds.min.x;
        }

        public float Y()
        {
            return _bounds.min.z;
        }

        public float Width()
        {
            return _bounds.extents.x * 2;
        }

        public float Height()
        {
            return _bounds.extents.z * 2;
        }

        public float CellSize()
        {
            return _cellSize;
        }

        public IEnumerable<IBlock> GetBlocks()
        {
            var components = _gameObject.GetComponentsInChildren(typeof(IBlock));
            if (components == null || !components.Any())
            {
                return null;
            }
            
            return components.OfType<IBlock>();
        }

        #endregion

        #region Private Methods

        private void getTerrainBounds(GameObject gameObject, ref Bounds resuldBounds)
        {
            var renderer = gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                resuldBounds.Encapsulate(renderer.bounds);
            }

            foreach (var child in gameObject.transform)
            {
                var childGameObject = ((Transform) child).gameObject;
                getTerrainBounds(childGameObject, ref resuldBounds);
            }
        }

        #endregion
    }
}