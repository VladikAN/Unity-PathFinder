using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Domain.Terrain;

namespace PathFinder2D.Core.Initializer
{
    public class MapInitializer : IMapInitializer
    {
        public MapCell[,] ParceMapCells(ITerrain terrain, float cellSize)
        {
            var x = (int)(terrain.RenderX() / cellSize);
            var z = (int)(terrain.RenderZ() / cellSize);

            var cellField = new MapCell[x, z];
            for (var i = 0; i < x; i++)
            {
                for (var j = 0; j < z; j++)
                {
                    cellField[i, j] = new MapCell();
                }
            }

            return cellField;
        }
    }
}