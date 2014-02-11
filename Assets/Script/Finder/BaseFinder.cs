using UnityEngine;

namespace Assets.Script.Finder
{
    public abstract class BaseFinder
    {
        public abstract BaseResult Find(Vector3 start, Vector3 end);

        protected Point ToPoint(Vector3 vector)
        {
            var x = (int)((vector.x - PathFinderGlobal.TerrainStartX) / PathFinderGlobal.CellWidth);
            var y = (int)((vector.z - PathFinderGlobal.TerrainStartZ) / PathFinderGlobal.CellWidth);

            return new Point(x, y);
        }

        protected Vector3 ToVector3(Point point)
        {
            var result = point.ToVector3()
                * PathFinderGlobal.CellWidth
                + new Vector3(PathFinderGlobal.TerrainStartX + PathFinderGlobal.CellCorrection, 0, PathFinderGlobal.TerrainStartZ + PathFinderGlobal.CellCorrection);

            return result;
        }
    }
}