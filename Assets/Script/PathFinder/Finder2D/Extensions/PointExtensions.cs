using Assets.Script.PathFinder.Finder2D.Finder;
using UnityEngine;

namespace Assets.Script.PathFinder.Finder2D.Extensions
{
    public static class PointExtensions
    {
        public static TPoint ToPoint<TPoint>(this Vector3 vector) where TPoint : BasePoint, new()
        {
            var point = vector.ToPoint();
            return new TPoint { X = point[0], Y = point[1] };
        }

        public static int[] ToPoint(this Vector3 vector)
        {
            var x = (int)((vector.x - PathFinderGlobal.TerrainGameObjectStartX) / PathFinderGlobal.CellWidth);
            var y = (int)((vector.z - PathFinderGlobal.TerrainGameObjectStartZ) / PathFinderGlobal.CellWidth);

            return new[] { x, y };
        }

        public static Vector3 ToVector3(this BasePoint point)
        {
            return ToVector3(point.X, point.Y);
        }

        public static Vector3 ToVector3(int x, int y)
        {
            var result = new Vector3(x, 0, y)
                * PathFinderGlobal.CellWidth
                + new Vector3(PathFinderGlobal.TerrainGameObjectStartX + PathFinderGlobal.CellCorrection, 0, PathFinderGlobal.TerrainGameObjectStartZ + PathFinderGlobal.CellCorrection);

            return result;
        }

        public static Vector3 NormalizeVector3(this Vector3 vector)
        {
            var point = vector.ToPoint();
            return ToVector3(point[0], point[1]);
        }
    }
}