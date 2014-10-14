using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;
using UnityEngine;

namespace PathFinder2D.Core.Extensions
{
    public static class MapExtensions
    {
        public static TPoint ToPoint<TPoint>(this MapDefinition mapDefinition, Vector3 vector) where TPoint : FinderPoint, new()
        {
            var point = mapDefinition.ToPoint(vector);
            return new TPoint { X = point[0], Y = point[1] };
        }

        public static int[] ToPoint(this MapDefinition mapDefinition, Vector3 vector)
        {
            var x = (int)((vector.x - mapDefinition.X) / mapDefinition.CellSize);
            var y = (int)((vector.z - mapDefinition.Y) / mapDefinition.CellSize);

            return new[] { x, y };
        }

        public static Vector3 ToVector3(this MapDefinition mapDefinition, FinderPoint point)
        {
            return ToVector3(mapDefinition, point.X, point.Y);
        }

        public static Vector3 ToVector3(this MapDefinition mapDefinition, int x, int y)
        {
            var result = new Vector3(x, 0, y)
                * mapDefinition.CellSize
                + new Vector3(mapDefinition.X + mapDefinition.CellCorrection, 0, mapDefinition.Y + mapDefinition.CellCorrection);

            return result;
        }

        public static bool[,] ToBoolMap(this MapDefinition mapDefinition)
        {
            var width = mapDefinition.Width;
            var height = mapDefinition.Height;

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