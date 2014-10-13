using UnityEngine;

namespace PathFinder2D.Core.Domain
{
    public class Map
    {
        public Map(GameObject terrain, float cellWidth)
        {
            Terrain = terrain;
            CellWidth = cellWidth;
        }

        public GameObject Terrain;
        public float CellWidth;
        public FinderResult LastFinderResult;
        private MapCell[,] _cellField;

        public float CellCorrection { get { return CellWidth / 2; } }

        public int TerrainFieldWidth { get { return _cellField.GetLength(0); } }
        public int TerrainFieldHeight { get { return _cellField.GetLength(1); } }
        public float TerrainGameObjectStartX { get { return Terrain.transform.position.x - Terrain.renderer.bounds.extents.x; } }
        public float TerrainGameObjectStartZ { get { return Terrain.transform.position.z - Terrain.renderer.bounds.extents.z; } }

        public MapCell[,] TerrainField
        {
            get
            {
                if (Terrain == null || _cellField != null)
                {
                    return _cellField;
                }
                
                var terrainWidth = Terrain.renderer.bounds.extents.x * 2;
                var terrainHeight = Terrain.renderer.bounds.extents.z * 2;
                var x = (int)(terrainWidth / CellWidth);
                var z = (int)(terrainHeight / CellWidth);

                _cellField = new MapCell[x, z];
                for (var i = 0; i < x; i++)
                {
                    for (var j = 0; j < z; j++)
                    {
                        _cellField[i, j] = new MapCell();
                    }
                }

                return _cellField;
            }
        }
    }
}