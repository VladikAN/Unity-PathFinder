using UnityEngine;

namespace Assets.Script.Components
{
    public class Terrain : MonoBehaviour
    {
        public void Start()
        {
            PathFinderGlobal.Terrain = gameObject;
            updateGrid();
        }

        public void Update()
        {
        }

        public void OnDrawGizmosSelected()
        {
            if (PathFinderGlobal.TerrainField == null)
            {
                return;
            }

            var startX = PathFinderGlobal.TerrainFieldStartX;
            var startZ = PathFinderGlobal.TerrainFieldStartZ;
            var correction = PathFinderGlobal.CellCorrection;

            var iMax = PathFinderGlobal.TerrainFieldWidth;
            var jMax = PathFinderGlobal.TerrainFieldHeight;

            var cubeSize = new Vector3(PathFinderGlobal.CellWidth, 1, PathFinderGlobal.CellWidth) - new Vector3(0.1f, 0f, 0.1f);

            for (var i = 0; i < iMax; i++)
            {
                var x = startX + PathFinderGlobal.CellWidth * i + correction;
                for (var j = 0; j < jMax; j++)
                {
                    Gizmos.color = PathFinderGlobal.TerrainField[i, j] != null && PathFinderGlobal.TerrainField[i, j].Blocked
                        ? Color.red
                        : Color.green;
                    
                    var z = startZ + PathFinderGlobal.CellWidth * j + correction;
                    var startPosition = new Vector3(x, 0.5f, z) + new Vector3(0.1f, 0f, 0.1f);
                    Gizmos.DrawWireCube(startPosition, cubeSize);
                }
            }
        }

        private void updateGrid()
        {
            var blocks = (Block[])FindObjectsOfType(typeof(Block));
            
            foreach (var block in blocks)
            {
                var blockGameObject = block.gameObject;

                var x = (int)((blockGameObject.transform.position.x - PathFinderGlobal.TerrainFieldStartX) / PathFinderGlobal.CellWidth);
                var z = (int)((blockGameObject.transform.position.z - PathFinderGlobal.TerrainFieldStartZ) / PathFinderGlobal.CellWidth);
                
                PathFinderGlobal.TerrainField[x, z].Blocked = true;
            }
        }
    }
}