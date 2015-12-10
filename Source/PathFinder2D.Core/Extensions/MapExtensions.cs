using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;

namespace PathFinder2D.Core.Extensions
{
    public static class MapExtensions
    {
        public static bool ValidateMapEdges(this MapDefinition mapDefinition, int x, int y)
        {
            return !((x < 0 || x >= mapDefinition.FieldWidth) || (y < 0 || y >= mapDefinition.FieldHeight));
        }

        public static bool ValidateMapEdges(this MapDefinition mapDefinition, FinderPoint point)
        {
            return ValidateMapEdges(mapDefinition, point.X, point.Y);
        }

        public static bool[,] ToBoolMap(this MapDefinition mapDefinition)
        {
            var width = mapDefinition.FieldWidth;
            var height = mapDefinition.FieldHeight;

            var result = new bool[width, height];
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    result[i, j] = mapDefinition.Field[i, j].Blocked;
                }
            }

            return result;
        }
    }
}