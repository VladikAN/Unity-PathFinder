using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Domain.Terrain;

namespace PathFinder2D.Core.Initializer
{
    public interface IMap
    {
        /// <summary>Parse terrain object into cell 2D array</summary>
        MapCell[,] ParseMapCells(IFloor terrain, float cellSize);
    }
}