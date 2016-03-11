using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Terrain;

namespace PathFinder2D.Core.Extensions
{
    public static class FloorExtensions
    {
        public static TPoint ToPoint<TPoint>(this IFloor floor, WorldPosition position) where TPoint : PathPoint, new()
        {
            var x = (int)((position.X - floor.X()) / floor.CellSize());
            var y = (int)((position.Y - floor.Y()) / floor.CellSize());

            return new TPoint { X = x, Y = y };
        }

        public static WorldPosition ToWorld<TPoint>(this IFloor floor, TPoint point) where TPoint : PathPoint, new()
        {
            var cellCorrection = floor.CellSize() / 2;
            var x = point.X * floor.CellSize() + floor.X() + cellCorrection;
            var y = point.Y * floor.CellSize() + floor.Y() + cellCorrection;

            return new WorldPosition(x, y);
        }
    }
}