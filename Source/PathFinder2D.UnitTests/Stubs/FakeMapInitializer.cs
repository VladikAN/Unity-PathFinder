using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Domain.Terrain;
using PathFinder2D.Core.Initializer;

namespace PathFinder2D.UnitTests.Stubs
{
    public class FakeMapInitializer : IMapInitializer
    {
        public MapCell[,] ParceMapCells(ITerrain terrain, float cellSize)
        {
            var rawMap = new[]
            {
                "..........",
                ".X......X.",
                ".X......X.",
                ".X.XXXX.X.",
                ".X......X.",
                ".X......X.",
                ".X.XXXX.X.",
                ".X......X.",
                ".X......X.",
                "..........",
            };

            var field = new MapCell[10, 10];
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    field[j, i] = new MapCell { Blocked = rawMap[j][i] == '#' };
                }
            }

            return field;
        }
    }
}