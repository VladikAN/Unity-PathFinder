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
            var startPoint = ToPoint(start);
            var endPoint = ToPoint(end);

            _map = new uint?[PathFinderGlobal.TerrainFieldWidth, PathFinderGlobal.TerrainFieldHeight];

            uint weight = 0;
            _map[startPoint.X, startPoint.Y] = weight;

            var founded = false;
            var lastIteration = new List<Point> { startPoint };
            while (!founded)
            {
                if (!lastIteration.Any())
                {
                    break;
                }

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

                lastIteration = thisIteration.Where(x => x != null).ToList();
            }

            IList<Vector3> path = null;
            if (founded)
            {
                path = new List<Vector3>();

                while (endPoint.X != startPoint.X || endPoint.Y != startPoint.Y)
                {
                    path.Add(ToVector3(endPoint));
                    endPoint = FindNearestPoints(endPoint, startPoint, _map[endPoint.X, endPoint.Y].Value).First();
                }

                path.Add(ToVector3(startPoint));
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
            
            /* center top */
            if (current.X == target.X && current.Y > target.Y)
            {
                var result = new List<Point>
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

                result = result.Where(item => item != null).ToList();
                return result.ToArray();
            }

            /* right top */
            if (current.X < target.X && current.Y > target.Y)
            {
                var result = new List<Point>
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

                result = result.Where(item => item != null).ToList();
                return result.ToArray();
            }

            /* right center */
            if (current.X < target.X && current.Y == target.Y)
            {
                var result = new List<Point>
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

                result = result.Where(item => item != null).ToList();
                return result.ToArray();
            }

            /* right bottom */
            if (current.X < target.X && current.Y < target.Y)
            {
                var result = new List<Point>
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

                result = result.Where(item => item != null).ToList();
                return result.ToArray();
            }

            /* center bottom */
            if (current.X == target.X && current.Y < target.Y)
            {
                var result = new List<Point>
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

                result = result.Where(item => item != null).ToList();
                return result.ToArray();
            }

            /* left bottom */
            if (current.X > target.X && current.Y < target.Y)
            {
                var result = new List<Point>
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

                result = result.Where(item => item != null).ToList();
                return result.ToArray();
            }

            /* left center */
            if (current.X > target.X && current.Y == target.Y)
            {
                var result = new List<Point>
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

                result = result.Where(item => item != null).ToList();
                return result.ToArray();
            }

            /* left top */
            if (current.X > target.X && current.Y > target.Y)
            {
                var result = new List<Point>
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

                result = result.Where(item => item != null).ToList();
                return result.ToArray();
            }

            return null;
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

        private Point ToPoint(Vector3 vector)
        {
            var x = (int)((vector.x - PathFinderGlobal.TerrainStartX) / PathFinderGlobal.CellWidth);
            var y = (int)((vector.z - PathFinderGlobal.TerrainStartZ) / PathFinderGlobal.CellWidth);

            return new Point(x, y);
        }

        private Vector3 ToVector3(Point point)
        {
            var result = point.ToVector3() 
                * PathFinderGlobal.CellWidth
                + new Vector3(PathFinderGlobal.TerrainStartX + PathFinderGlobal.CellCorrection, 0, PathFinderGlobal.TerrainStartZ + PathFinderGlobal.CellCorrection);

            return result;
        }
    }
}