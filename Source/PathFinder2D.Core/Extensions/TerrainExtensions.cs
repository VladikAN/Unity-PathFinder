using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Terrain;
using UnityEngine;

namespace PathFinder2D.Core.Extensions
{
    public static class TerrainExtensions
    {
        public static TPoint ToPoint<TPoint>(this ITerrain terrain, Vector3 vector) where TPoint : FinderPoint, new()
        {
            var x = (int)((vector.x - terrain.X()) / terrain.CellSize());
            var y = (int)((vector.z - terrain.Y()) / terrain.CellSize());

            return new TPoint { X = x, Y = y };
        }

        public static Vector3 ToVector3<TPoint>(this ITerrain terrain, TPoint point) where TPoint : FinderPoint, new()
        {
            var cellCorrection = terrain.CellSize() / 2;
            return new Vector3(point.X, 0, point.Y) * terrain.CellSize() + new Vector3(terrain.X() + cellCorrection, 0, terrain.Y() + cellCorrection);
        }
    }
}