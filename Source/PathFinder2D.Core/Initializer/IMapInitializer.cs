using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Domain.Terrain;

namespace PathFinder2D.Core.Initializer
{
    public interface IMapInitializer
    {
        /// <summary>
        /// Parce terrain object to cell array
        /// </summary>
        MapCell[,] ParceMapCells(ITerrain terrain, float cellSize);
    }
}