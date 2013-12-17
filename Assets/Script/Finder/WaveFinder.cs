using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Script.Finder
{
    public class WaveFinder : IFinder
    {
        public Vector3[] Find(Vector3 start, Vector3 end)
        {
            var startX = (int)((start.x - PathFinderGlobal.TerrainFieldStartX) / PathFinderGlobal.CellWidth);
            var startY = (int)((start.z - PathFinderGlobal.TerrainFieldStartZ) / PathFinderGlobal.CellWidth);

            var endX = (int)((end.x - PathFinderGlobal.TerrainFieldStartX) / PathFinderGlobal.CellWidth);
            var endY = (int)((end.z - PathFinderGlobal.TerrainFieldStartZ) / PathFinderGlobal.CellWidth);

            var map = new bool[PathFinderGlobal.TerrainFieldWidth, PathFinderGlobal.TerrainFieldHeight];
            map[startX, startY] = true;

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

                    createStep(-1, -1, ref map, point, thisIteration);
                    createStep(0, -1, ref map, point, thisIteration);
                    createStep(1, -1, ref map, point, thisIteration);
                    createStep(1, 0, ref map, point, thisIteration);
                    createStep(1, 1, ref map, point, thisIteration);
                    createStep(0, 1, ref map, point, thisIteration);
                    createStep(-1, 1, ref map, point, thisIteration);
                    createStep(-1, 0, ref map, point, thisIteration);
                }

                if (!thisIteration.Any())
                {
                    break;
                }

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

        private void createStep(int xMove, int yMove, ref bool[,] map, LinkedPath parent, ICollection<LinkedPath> iteration)
        {
            var newX = parent.X + xMove;
            var newY = parent.Y + yMove;

            if ((newX < 0 || newX >= PathFinderGlobal.TerrainFieldWidth)
                || (newY < 0 || newY >= PathFinderGlobal.TerrainFieldHeight))
            {
                return;
            }

            if (map[newX, newY] || PathFinderGlobal.TerrainField[newX, newY].Blocked)
            {
                return;
            }

            if (xMove == 0 || yMove == 0)
            {
                map[newX, newY] = true;
                iteration.Add(new LinkedPath(newX, newY, parent));
            }
            else
            {
                if (!PathFinderGlobal.TerrainField[newX, parent.Y].Blocked
                    && !PathFinderGlobal.TerrainField[parent.X, newY].Blocked)
                {
                    map[newX, newY] = true;
                    iteration.Add(new LinkedPath(newX, newY, parent));
                }
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