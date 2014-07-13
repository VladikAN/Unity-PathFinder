using UnityEngine;

namespace Assets.Script.PathFinder.Finder2D.Finder
{
    public abstract class BaseFinder
    {
        public abstract BaseResult Find(Vector3 startVector3, Vector3 endVector3);
        
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