﻿using System.Collections.Generic;
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

                    mark(-1, -1, weight, ref map, point, thisIteration);
                    mark(0, -1, weight, ref map, point, thisIteration);
                    mark(1, -1, weight, ref map, point, thisIteration);
                    mark(1, 0, weight, ref map, point, thisIteration);
                    mark(1, 1, weight, ref map, point, thisIteration);
                    mark(0, 1, weight, ref map, point, thisIteration);
                    mark(-1, 1, weight, ref map, point, thisIteration);
                    mark(-1, 0, weight, ref map, point, thisIteration);
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

        private void mark(int xMove, int yMove, uint weight, ref uint?[,] map, LinkedPath parent, ICollection<LinkedPath> iteration)
        {
            var newX = parent.X + xMove;
            var newY = parent.Y + yMove;

            if ((newX < 0 || newX >= PathFinderGlobal.TerrainFieldWidth)
                || (newY < 0 || newY >= PathFinderGlobal.TerrainFieldHeight))
            {
                return;
            }

            if (map[newX, newY] != null || PathFinderGlobal.TerrainField[newX, newY].Blocked)
            {
                return;
            }

            if (xMove == 0 || yMove == 0)
            {
                map[newX, newY] = weight;
                iteration.Add(new LinkedPath(newX, newY, parent));
            }
            else
            {
                if (!PathFinderGlobal.TerrainField[newX, parent.Y].Blocked
                    && !PathFinderGlobal.TerrainField[parent.X, newY].Blocked)
                {
                    map[newX, newY] = weight;
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