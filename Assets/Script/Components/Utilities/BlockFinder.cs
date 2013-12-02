using UnityEngine;

namespace Assets.Script.Components.Utilities
{
    public class BlockFinder : MonoBehaviour
    {
        private int _currentX;
        private int _currentY;

        public void Start()
        {
            var startX = PathFinderGlobal.TerrainFieldStartX;
            var startZ = PathFinderGlobal.TerrainFieldStartZ;
            var correction = PathFinderGlobal.CellCorrection;

            var iMax = PathFinderGlobal.TerrainFieldWidth;
            var jMax = PathFinderGlobal.TerrainFieldHeight;

            for (_currentX = 0; _currentX < iMax; _currentX++)
            {
                var x = startX + PathFinderGlobal.CellWidth * _currentX + correction;
                for (_currentY = 0; _currentY < jMax; _currentY++)
                {
                    PathFinderGlobal.TerrainField[_currentX, _currentY] = new Point();

                    var z = startZ + PathFinderGlobal.CellWidth * _currentY + correction;
                    transform.position = new Vector3(x, 0.5f, z);
                }
            }

            //Destroy(gameObject);
        }
	
        public void Update()
        {
        }

        public void OnTriggerEnter()
        {
            print("1");
            PathFinderGlobal.TerrainField[_currentX, _currentY].Blocked = true;
        }
    }
}