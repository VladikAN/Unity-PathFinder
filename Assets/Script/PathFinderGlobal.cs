using Assets.Script.Finder;
using UnityEngine;

namespace Assets.Script
{
    public static class PathFinderGlobal
    {
        private static Cell[,] _terrainField;

        #region Cell

        public static uint CellWidth = 1;
        public static float CellCorrection { get { return (float)CellWidth / 2; } }

        #endregion

        #region Terrain

        public static GameObject Terrain;
        public static Cell[,] TerrainField
        {
            get
            {
                if (Terrain != null && _terrainField == null)
                {
                    var x = ((int)TerrainFieldStartWidth) / CellWidth;
                    var z = ((int)TerrainFieldStartHeight) / CellWidth;

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
        public static float TerrainFieldStartWidth { get { return Terrain.renderer.bounds.extents.x * 2; } }
        public static float TerrainFieldStartHeight { get { return Terrain.renderer.bounds.extents.z * 2; } }
        public static float TerrainFieldStartX { get { return Terrain.transform.position.x - Terrain.renderer.bounds.extents.x; } }
        public static float TerrainFieldStartZ { get { return Terrain.transform.position.z - Terrain.renderer.bounds.extents.z; } }

        #endregion

        #region IFinder

        public static FinderResult LastResult = null;
        public static FinderResult Find(IFinder finder, Vector3 start, Vector3 end)
        {
            var result = finder.Find(start, end);
            LastResult = result;

            return result;
        }

        #endregion
    }
}