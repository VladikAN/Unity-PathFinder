using PathFinder2D.Core.Domain;

namespace PathFinder2D.Core.Extensions
{
    public static class FinderExtensions
    {
        public static bool ValidateMapEdges(this Map map, int x, int y)
        {
            return !((x < 0 || x >= map.TerrainFieldWidth) || (y < 0 || y >= map.TerrainFieldHeight));
        }

        public static bool ValidateMapEdges(this Map map, FinderPoint point)
        {
            return ValidateMapEdges(map, point.X, point.Y);
        }

        public static bool IsPointBlocked(this Map map, int x, int y)
        {
            return map.TerrainField[x, y].Blocked;
        }

        public static bool IsPointBlocked(this Map map, FinderPoint point)
        {
            return IsPointBlocked(map, point.X, point.Y);
        }
    }
}