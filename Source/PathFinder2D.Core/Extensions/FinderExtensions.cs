using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;

namespace PathFinder2D.Core.Extensions
{
    public static class FinderExtensions
    {
        public static bool ValidateMapEdges(this MapDefinition mapDefinition, int x, int y)
        {
            return !((x < 0 || x >= mapDefinition.Width) || (y < 0 || y >= mapDefinition.Height));
        }

        public static bool ValidateMapEdges(this MapDefinition mapDefinition, FinderPoint point)
        {
            return ValidateMapEdges(mapDefinition, point.X, point.Y);
        }
    }
}