using UnityEngine;

namespace Assets.Script.Components
{
    public class Terrain : MonoBehaviour
    {
        public void Start()
        {
            var x = ((int)gameObject.renderer.bounds.extents.x * 2) / PathFinderGlobal.CellWidth;
            var z = ((int)gameObject.renderer.bounds.extents.z * 2) / PathFinderGlobal.CellWidth;

            PathFinderGlobal.TerrainField = new int[x, z];
        }

        public float StartX { get { return gameObject.transform.position.x - gameObject.renderer.bounds.extents.x; } }
        public float StartZ { get { return gameObject.transform.position.z - gameObject.renderer.bounds.extents.z; } }

        public void Update()
        {
        }

        public void OnDrawGizmosSelected()
        {
            if (PathFinderGlobal.TerrainField == null)
            {
                return;
            }

            var startX = StartX;
            var startZ = StartZ;
            var correctionX = (float)PathFinderGlobal.CellWidth / 2;
            var correctionZ = (float)PathFinderGlobal.CellWidth / 2;

            var iMax = PathFinderGlobal.TerrainField.GetLength(0);
            var jMax = PathFinderGlobal.TerrainField.GetLength(1);

            var cubeSize = new Vector3(PathFinderGlobal.CellWidth, 1, PathFinderGlobal.CellWidth);

            Gizmos.color = Color.green;
            for (var i = 0; i < iMax; i++)
            {
                for (var j = 0; j < jMax; j++)
                {
                    var x = startX + PathFinderGlobal.CellWidth * i + correctionX;
                    var z = startZ + PathFinderGlobal.CellWidth * j + correctionZ;

                    var startPosition = new Vector3(x, 0.5f, z);
                    Gizmos.DrawWireCube(startPosition, cubeSize);
                }
            }
        }
    }
}