using UnityEngine;

namespace Assets.Script.Finder
{
    public abstract class BaseFinder
    {
        public abstract BaseResult Find(Vector3 start, Vector3 end);

        protected TPoint ToPoint<TPoint>(Vector3 vector) where TPoint : BasePoint, new()
        {
            var x = (int)((vector.x - PathFinderGlobal.TerrainStartX) / PathFinderGlobal.CellWidth);
            var y = (int)((vector.z - PathFinderGlobal.TerrainStartZ) / PathFinderGlobal.CellWidth);

            return new TPoint { X = x, Y = y };
        }

        protected Vector3 ToVector3(BasePoint point)
        {
            var result = new Vector3(point.X, 0, point.Y)
                * PathFinderGlobal.CellWidth
                + new Vector3(PathFinderGlobal.TerrainStartX + PathFinderGlobal.CellCorrection, 0, PathFinderGlobal.TerrainStartZ + PathFinderGlobal.CellCorrection);

            return result;
        }
    }
}