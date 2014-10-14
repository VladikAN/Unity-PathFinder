using PathFinder2D.Core.Domain.Terrain;
using UnityEngine;

namespace PathFinder2D.Unity.Domain
{
    public class TerrainGameObject : ITerrain
    {
        private readonly GameObject _gameObject;

        public TerrainGameObject(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public int Id() 
        {
            return _gameObject.GetInstanceID();
        }

        public float TransformX()
        {
            return _gameObject.transform.position.x;
        }

        public float TransformZ()
        {
            return _gameObject.transform.position.z;
        }

        public float RenderX()
        {
            return _gameObject.renderer.bounds.extents.x * 2;
        }

        public float RenderZ()
        {
            return _gameObject.renderer.bounds.extents.z * 2;
        }
    }
}