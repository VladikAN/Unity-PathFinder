using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Script.Finder
{
    public class WaveFinder : IFinder
    {
        private uint?[,] _map;

        public FinderResult Find(Vector3 start, Vector3 end)
        {
            var startX = (int)((start.x - PathFinderGlobal.TerrainFieldStartX) / PathFinderGlobal.CellWidth);
            var startY = (int)((start.z - PathFinderGlobal.TerrainFieldStartZ) / PathFinderGlobal.CellWidth);
            var startPoint = new Point(startX, startY);

            var endX = (int)((end.x - PathFinderGlobal.TerrainFieldStartX) / PathFinderGlobal.CellWidth);
            var endY = (int)((end.z - PathFinderGlobal.TerrainFieldStartZ) / PathFinderGlobal.CellWidth);
            var endPoint = new Point(endX, endY);

            _map = new uint?[PathFinderGlobal.TerrainFieldWidth, PathFinderGlobal.TerrainFieldHeight];

            uint weight = 0;
            _map[startPoint.X, startPoint.Y] = weight;

            var founded = false;
            var lastIteration = new List<Point> { startPoint };
            while (!founded)
            {
                weight++;
                var thisIteration = new List<Point>();

                foreach (var point in lastIteration)
                {
                    if (point.X == endPoint.X && point.Y == endPoint.Y)
                    {
                        founded = true;
                        break;
                    }

                    thisIteration.Add(CreateNextStep(point, -1, -1, weight));
                    thisIteration.Add(CreateNextStep(point, 0, -1, weight));
                    thisIteration.Add(CreateNextStep(point, 1, -1, weight));
                    thisIteration.Add(CreateNextStep(point, 1, 0, weight));
                    thisIteration.Add(CreateNextStep(point, 1, 1, weight));
                    thisIteration.Add(CreateNextStep(point, 0, 1, weight));
                    thisIteration.Add(CreateNextStep(point, -1, 1, weight));
                    thisIteration.Add(CreateNextStep(point, -1, 0, weight));
                }

                if (thisIteration.Count(x => x != null) == 0)
                {
                    break;
                }

                lastIteration = thisIteration.Where(x => x != null).ToList();
            }

            IList<Vector3> path = null;
            if (founded)
            {
                path = new List<Vector3>();

                while (endPoint.X != startPoint.X || endPoint.Y != startPoint.Y)
                {
                    path.Add(endPoint.ToVector3() * PathFinderGlobal.CellWidth + new Vector3(PathFinderGlobal.TerrainFieldStartX + PathFinderGlobal.CellCorrection, 0, PathFinderGlobal.TerrainFieldStartZ + PathFinderGlobal.CellCorrection));
                    endPoint = FindNearestPoints(endPoint, startPoint, _map[endPoint.X, endPoint.Y].Value).First();
                }

                path.Add(startPoint.ToVector3() * PathFinderGlobal.CellWidth + new Vector3(PathFinderGlobal.TerrainFieldStartX + PathFinderGlobal.CellCorrection, 0, PathFinderGlobal.TerrainFieldStartZ + PathFinderGlobal.CellCorrection));
                path.Reverse();
            }

            var result = new FinderResult
            {
                Path = path,
                Map = _map
            };

            return result;
        }

        private IEnumerable<Point> FindNearestPoints(Point current, Point target, uint weight)
        {
            var targetWeight = weight - 1;
            List<Point> result = null;

            /* center top */
            if (current.X == target.X && current.Y > target.Y)
            {
                Debug.Log("center top");
                result = new List<Point>
                {
                    GetPoint(current.X, current.Y - 1, targetWeight),       /* center top */
                    GetPoint(current.X + 1, current.Y - 1, targetWeight),   /* right top */
                    GetPoint(current.X - 1, current.Y - 1, targetWeight),   /* left top */
                    GetPoint(current.X + 1, current.Y,targetWeight),        /* right center */
                    GetPoint(current.X - 1, current.Y, targetWeight),       /* left center */
                    GetPoint(current.X + 1, current.Y + 1, targetWeight),   /* right bottom */
                    GetPoint(current.X - 1, current.Y + 1, targetWeight),   /* left bottom */
                    GetPoint(current.X, current.Y + 1, targetWeight),       /* center bottom */
                };
            }

            /* right top */
            if (current.X < target.X && current.Y > target.Y)
            {
                Debug.Log("right top");
                result = new List<Point>
                {
                    GetPoint(current.X + 1, current.Y - 1, targetWeight),   /* right top */
                    GetPoint(current.X + 1, current.Y,targetWeight),        /* right center */
                    GetPoint(current.X, current.Y - 1, targetWeight),       /* center top */
                    GetPoint(current.X + 1, current.Y + 1, targetWeight),   /* right bottom */
                    GetPoint(current.X - 1, current.Y - 1, targetWeight),   /* left top */
                    GetPoint(current.X, current.Y + 1, targetWeight),       /* center bottom */
                    GetPoint(current.X - 1, current.Y, targetWeight),       /* left center */
                    GetPoint(current.X - 1, current.Y + 1, targetWeight),   /* left bottom */
                };
            }

            /* right center */
            if (current.X < target.X && current.Y == target.Y)
            {
                Debug.Log("right center");
                result = new List<Point>
                {
                    GetPoint(current.X + 1, current.Y,targetWeight),        /* right center */
                    GetPoint(current.X + 1, current.Y + 1, targetWeight),   /* right bottom */
                    GetPoint(current.X + 1, current.Y - 1, targetWeight),   /* right top */
                    GetPoint(current.X, current.Y + 1, targetWeight),       /* center bottom */
                    GetPoint(current.X, current.Y - 1, targetWeight),       /* center top */
                    GetPoint(current.X - 1, current.Y + 1, targetWeight),   /* left bottom */
                    GetPoint(current.X - 1, current.Y - 1, targetWeight),   /* left top */
                    GetPoint(current.X - 1, current.Y, targetWeight),       /* left center */
                };
            }

            /* right bottom */
            if (current.X < target.X && current.Y < target.Y)
            {
                Debug.Log("right bottom");
                result = new List<Point>
                {
                    GetPoint(current.X + 1, current.Y + 1, targetWeight),   /* right bottom */
                    GetPoint(current.X, current.Y + 1, targetWeight),       /* center bottom */
                    GetPoint(current.X + 1, current.Y,targetWeight),        /* right center */
                    GetPoint(current.X - 1, current.Y + 1, targetWeight),   /* left bottom */
                    GetPoint(current.X + 1, current.Y - 1, targetWeight),   /* right top */
                    GetPoint(current.X - 1, current.Y, targetWeight),       /* left center */
                    GetPoint(current.X, current.Y - 1, targetWeight),       /* center top */
                    GetPoint(current.X - 1, current.Y - 1, targetWeight),   /* left top */
                };
            }

            /* center bottom */
            if (current.X == target.X && current.Y < target.Y)
            {
                Debug.Log("center bottom");
                result = new List<Point>
                {
                    GetPoint(current.X, current.Y + 1, targetWeight),       /* center bottom */
                    GetPoint(current.X - 1, current.Y + 1, targetWeight),   /* left bottom */
                    GetPoint(current.X + 1, current.Y + 1, targetWeight),   /* right bottom */
                    GetPoint(current.X - 1, current.Y, targetWeight),       /* left center */
                    GetPoint(current.X + 1, current.Y,targetWeight),        /* right center */
                    GetPoint(current.X - 1, current.Y - 1, targetWeight),   /* left top */
                    GetPoint(current.X + 1, current.Y - 1, targetWeight),   /* right top */
                    GetPoint(current.X, current.Y - 1, targetWeight),       /* center top */
                };
            }

            /* left bottom */
            if (current.X > target.X && current.Y < target.Y)
            {
                Debug.Log("left bottom");
                result = new List<Point>
                {
                    GetPoint(current.X - 1, current.Y + 1, targetWeight),   /* left bottom */
                    GetPoint(current.X - 1, current.Y, targetWeight),       /* left center */
                    GetPoint(current.X, current.Y + 1, targetWeight),       /* center bottom */
                    GetPoint(current.X - 1, current.Y - 1, targetWeight),   /* left top */
                    GetPoint(current.X + 1, current.Y + 1, targetWeight),   /* right bottom */
                    GetPoint(current.X, current.Y - 1, targetWeight),       /* center top */
                    GetPoint(current.X + 1, current.Y,targetWeight),        /* right center */
                    GetPoint(current.X + 1, current.Y - 1, targetWeight),   /* right top */
                };
            }

            /* left center */
            if (current.X > target.X && current.Y == target.Y)
            {
                Debug.Log("left center");
                result = new List<Point>
                {
                    GetPoint(current.X - 1, current.Y, targetWeight),       /* left center */
                    GetPoint(current.X - 1, current.Y - 1, targetWeight),   /* left top */
                    GetPoint(current.X - 1, current.Y + 1, targetWeight),   /* left bottom */
                    GetPoint(current.X, current.Y - 1, targetWeight),       /* center top */
                    GetPoint(current.X, current.Y + 1, targetWeight),       /* center bottom */
                    GetPoint(current.X + 1, current.Y - 1, targetWeight),   /* right top */
                    GetPoint(current.X + 1, current.Y + 1, targetWeight),   /* right bottom */
                    GetPoint(current.X + 1, current.Y,targetWeight),        /* right center */
                };
            }

            /* left top */
            if (current.X > target.X && current.Y > target.Y)
            {
                Debug.Log("left top");
                result = new List<Point>
                {
                    GetPoint(current.X - 1, current.Y - 1, targetWeight),   /* left top */
                    GetPoint(current.X, current.Y - 1, targetWeight),       /* center top */
                    GetPoint(current.X - 1, current.Y, targetWeight),       /* left center */
                    GetPoint(current.X + 1, current.Y - 1, targetWeight),   /* right top */
                    GetPoint(current.X - 1, current.Y + 1, targetWeight),   /* left bottom */
                    GetPoint(current.X + 1, current.Y,targetWeight),        /* right center */
                    GetPoint(current.X, current.Y + 1, targetWeight),       /* center bottom */
                    GetPoint(current.X + 1, current.Y + 1, targetWeight),   /* right bottom */
                };
            }

            result = result.Where(item => item != null).ToList();
            return result.ToArray();
        }

        private Point GetPoint(int x, int y, uint targetWeight)
        {
            if (!Valid(x, y) || _map[x, y] != targetWeight)
            {
                return null;
            }

            return new Point(x, y);
        }

        private Point CreateNextStep(Point parent, int xMove, int yMove, uint weight)
        {
            var newX = parent.X + xMove;
            var newY = parent.Y + yMove;

            if (!Valid(newX, newY))
            {
                return null;
            }

            if (_map[newX, newY] != null || PathFinderGlobal.TerrainField[newX, newY].Blocked)
            {
                return null;
            }

            Point result = null;
            if (xMove == 0 || yMove == 0)
            {
                _map[newX, newY] = weight;
                result = new Point(newX, newY);
            }
            else
            {
                if (!PathFinderGlobal.TerrainField[newX, parent.Y].Blocked
                    && !PathFinderGlobal.TerrainField[parent.X, newY].Blocked)
                {
                    _map[newX, newY] = weight;
                    result = new Point(newX, newY);
                }
            }

            return result;
        }

        private bool Valid(int x, int y)
        {
            return !((x < 0 || x >= PathFinderGlobal.TerrainFieldWidth) || (y < 0 || y >= PathFinderGlobal.TerrainFieldHeight));
        }
    }
}