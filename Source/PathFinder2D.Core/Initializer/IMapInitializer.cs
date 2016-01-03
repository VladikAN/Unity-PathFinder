using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Domain.Terrain;

namespace PathFinder2D.Core.Initializer
{
    public interface IMapInitializer
    {
        /// <summary>
        /// Parse terrain object into cell 2D array
        /// </summary>
        MapCell[,] ParseMapCells(ITerrain terrain, float cellSize);
    }
}