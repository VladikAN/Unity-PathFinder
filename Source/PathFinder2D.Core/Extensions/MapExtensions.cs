using PathFinder2D.Core.Domain;
using UnityEngine;

namespace PathFinder2D.Core.Extensions
{
    public static class MapExtensions
    {
        public static TPoint ToPoint<TPoint>(this Map map, Vector3 vector) where TPoint : FinderPoint, new()
        {
            var point = map.ToPoint(vector);
            return new TPoint { X = point[0], Y = point[1] };
        }

        public static int[] ToPoint(this Map map, Vector3 vector)
        {
            var x = (int)((vector.x - map.TerrainGameObjectStartX) / map.CellWidth);
            var y = (int)((vector.z - map.TerrainGameObjectStartZ) / map.CellWidth);

            return new[] { x, y };
        }

        public static Vector3 ToVector3(this Map map, FinderPoint point)
        {
            return ToVector3(map, point.X, point.Y);
        }

        public static Vector3 ToVector3(this Map map, int x, int y)
        {
            var result = new Vector3(x, 0, y)
                * map.CellWidth
                + new Vector3(map.TerrainGameObjectStartX + map.CellCorrection, 0, map.TerrainGameObjectStartZ + map.CellCorrection);

            return result;
        }

        public static bool[,] ToBoolMap(this Map map)
        {
            var width = map.TerrainFieldWidth;
            var height = map.TerrainFieldHeight;

            var result = new bool[width, height];
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    result[i, j] = map.TerrainField[i, j].Blocked;
                }
            }

            return result;
        }

        public static uint?[,] ToWeightMap(this Map map)
        {
            var result = new uint?[map.TerrainFieldWidth, map.TerrainFieldHeight];
            return result;
        }
    }
}