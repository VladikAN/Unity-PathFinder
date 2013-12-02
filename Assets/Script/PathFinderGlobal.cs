using UnityEngine;

namespace Assets.Script
{
    public static class PathFinderGlobal
    {
        private static Point[,] _terrainField;

        #region Cell

        public static uint CellWidth = 1;
        public static float CellCorrection { get { return (float)CellWidth / 2; } }

        #endregion

        #region Terrain

        public static GameObject Terrain;
        public static Point[,] TerrainField
        {
            get
            {
                if (Terrain != null && _terrainField == null)
                {
                    var x = ((int)Terrain.renderer.bounds.extents.x * 2) / CellWidth;
                    var z = ((int)Terrain.renderer.bounds.extents.z * 2) / CellWidth;

                    _terrainField = new Point[x, z];
                }

                return _terrainField;
            }
        }
        public static int TerrainFieldWidth { get { return TerrainField.GetLength(0); } }
        public static int TerrainFieldHeight { get { return TerrainField.GetLength(1); } }
        public static float TerrainFieldStartX { get { return Terrain.transform.position.x - Terrain.renderer.bounds.extents.x; } }
        public static float TerrainFieldStartZ { get { return Terrain.transform.position.z - Terrain.renderer.bounds.extents.z; } }

        #endregion
    }
}