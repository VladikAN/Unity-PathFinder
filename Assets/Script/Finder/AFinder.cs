using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Script.Finder
{
    public class AFinder : IFinder
    {
        public Vector3[] Find(Vector3 start, Vector3 end)
        {
            var startX = (int)((start.x - PathFinderGlobal.TerrainFieldStartX) / PathFinderGlobal.CellWidth);
            var startY = (int)((start.z - PathFinderGlobal.TerrainFieldStartZ) / PathFinderGlobal.CellWidth);

            var endX = (int)((end.x - PathFinderGlobal.TerrainFieldStartX) / PathFinderGlobal.CellWidth);
            var endY = (int)((end.z - PathFinderGlobal.TerrainFieldStartZ) / PathFinderGlobal.CellWidth);

            uint weight = 0;

            var map = new uint?[PathFinderGlobal.TerrainFieldWidth, PathFinderGlobal.TerrainFieldHeight];
            map[startX, startY] = weight++;

            var lastIteration = new List<LinkedPath> { new LinkedPath(startX, startY, null) };
            LinkedPath lastPoint = null;
            while (true)
            {
                var thisIteration = new List<LinkedPath>();

                foreach (var point in lastIteration)
                {
                    if (point.X == endX && point.Y == endY)
                    {
                        lastPoint = point;
                        break;
                    }

                    mark(point.X - 1, point.Y, weight, ref map, point, thisIteration);
                    mark(point.X + 1, point.Y, weight, ref map, point, thisIteration);
                    mark(point.X, point.Y - 1, weight, ref map, point, thisIteration);
                    mark(point.X, point.Y + 1, weight, ref map, point, thisIteration);
                }

                if (!thisIteration.Any())
                {
                    break;
                }

                weight++;
                lastIteration = thisIteration;
            }

            if (lastPoint != null)
            {
                var result = new List<Vector3>();
                while (lastPoint.Parent != null)
                {
                    result.Add(new Vector3(lastPoint.X * PathFinderGlobal.CellWidth + PathFinderGlobal.TerrainFieldStartX, 0, lastPoint.Y * PathFinderGlobal.CellWidth + PathFinderGlobal.TerrainFieldStartZ));
                    lastPoint = lastPoint.Parent;
                }

                result.Reverse();
                return result.ToArray();
            }

            return null;
        }

        private void mark(int x, int y, uint weight, ref uint?[,] map, LinkedPath parent, ICollection<LinkedPath> iteration)
        {
            if (x < 0 || x >= PathFinderGlobal.TerrainFieldWidth)
            {
                return;
            }

            if (y < 0 || y >= PathFinderGlobal.TerrainFieldHeight)
            {
                return;
            }

            if (map[x, y] != null)
            {
                return;
            }

            if (!PathFinderGlobal.TerrainField[x, y].Blocked)
            {
                map[x, y] = weight;
                iteration.Add(new LinkedPath(x, y, parent));
            }
        }

        private class LinkedPath
        {
            public LinkedPath(int x, int y, LinkedPath parent)
            {
                X = x;
                Y = y;
                Parent = parent;
            }

            public int X { get; private set; }
            public int Y { get; private set; }
            public LinkedPath Parent { get; private set; }
        }
    }
}