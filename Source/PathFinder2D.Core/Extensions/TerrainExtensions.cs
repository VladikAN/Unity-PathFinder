using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Terrain;

namespace PathFinder2D.Core.Extensions
{
    public static class TerrainExtensions
    {
        public static TPoint ToPoint<TPoint>(this ITerrain terrain, WorldPosition vector) where TPoint : FinderPoint, new()
        {
            var x = (int)((vector.X - terrain.X()) / terrain.CellSize());
            var y = (int)((vector.Y - terrain.Y()) / terrain.CellSize());

            return new TPoint { X = x, Y = y };
        }

        public static WorldPosition ToVector3<TPoint>(this ITerrain terrain, TPoint point) where TPoint : FinderPoint, new()
        {
            var cellCorrection = terrain.CellSize() / 2;
            var x = point.X * terrain.CellSize() + terrain.X() + cellCorrection;
            var y = point.Y * terrain.CellSize() + terrain.Y() + cellCorrection;

            return new WorldPosition(x, y);
        }
    }
}