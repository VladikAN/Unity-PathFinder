using System.Collections.Generic;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Domain.Terrain;
using System.Linq;
using PathFinder2D.Core.Extensions;

namespace PathFinder2D.Core.Initializer
{
    public class MapInitializer : IMapInitializer
    {
        public MapCell[,] ParseMapCells(ITerrain terrain, float cellSize)
        {
            var x = (int)(terrain.Width() / cellSize);
            var z = (int)(terrain.Height() / cellSize);

            var blocks = (terrain.GetBlocks() ?? Enumerable.Empty<IBlock>()).ToList();
            var points = new List<FinderPoint>();
            if (blocks.Any())
            {
                points = blocks
                    .SelectMany(block => block.GetPoints(terrain))
                    .Select(terrain.ToPoint<FinderPoint>)
                    .ToList();
            }

            var cellField = new MapCell[x, z];
            for (var i = 0; i < x; i++)
            {
                for (var j = 0; j < z; j++)
                {
                    var block = points.FirstOrDefault(point => point.X == i && point.Y == j);
                    cellField[i, j] = new MapCell { Blocked = block != null };
                }
            }

            return cellField;
        }
    }
}