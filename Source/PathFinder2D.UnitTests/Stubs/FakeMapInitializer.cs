using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Domain.Terrain;
using PathFinder2D.Core.Initializer;

namespace PathFinder2D.UnitTests.Stubs
{
    public class FakeMapInitializer : IMapInitializer
    {
        public MapCell[,] ParceMapCells(ITerrain terrain, float cellSize)
        {
            return new MapCell[10, 10];
        }
    }
}