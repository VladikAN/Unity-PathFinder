using UnityEngine;

namespace Assets.Script.PathFinder.Finder2D.Finder
{
    public abstract class BaseFinder
    {
        public abstract BaseResult Find(Vector3 startVector3, Vector3 endVector3);

        protected TPoint ToPoint<TPoint>(Vector3 vector) where TPoint : BasePoint, new()
        {
            var x = (int)((vector.x - PathFinderGlobal.TerrainGameObjectStartX) / PathFinderGlobal.CellWidth);
            var y = (int)((vector.z - PathFinderGlobal.TerrainGameObjectStartZ) / PathFinderGlobal.CellWidth);

            return new TPoint { X = x, Y = y };
        }

        protected Vector3 ToVector3(BasePoint point)
        {
            var result =new Vector3(point.X, 0, point.Y)
                * PathFinderGlobal.CellWidth
                + new Vector3(PathFinderGlobal.TerrainGameObjectStartX + PathFinderGlobal.CellCorrection, 0, PathFinderGlobal.TerrainGameObjectStartZ + PathFinderGlobal.CellCorrection);

            return result;
        }
        
        protected bool ValidateEdges(int x, int y)
        {
            return !((x < 0 || x >= PathFinderGlobal.TerrainFieldWidth) || (y < 0 || y >= PathFinderGlobal.TerrainFieldHeight));
        }

        protected bool ValidateEdges(BasePoint point)
        {
            return ValidateEdges(point.X, point.Y);
        }
        
        protected bool IsBlocked(int x, int y)
        {
            return PathFinderGlobal.TerrainField[x, y].Blocked;
        }

        protected bool IsBlocked(BasePoint point)
        {
            return IsBlocked(point.X, point.Y);
        }
    }
}