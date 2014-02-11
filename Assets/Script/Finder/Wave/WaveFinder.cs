using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Script.Finder.Wave
{
    public class WaveFinder : BaseFinder
    {
        private uint?[,] _map;

        public override BaseResult Find(Vector3 start, Vector3 end)
        {
            var startPoint = ToPoint<WavePoint>(start);
            var endPoint = ToPoint<WavePoint>(end);

            _map = new uint?[PathFinderGlobal.TerrainFieldWidth, PathFinderGlobal.TerrainFieldHeight];

            uint weight = 0;
            _map[startPoint.X, startPoint.Y] = weight;

            var founded = false;
            var lastIteration = new List<WavePoint> { startPoint };
            while (!founded)
            {
                if (!lastIteration.Any())
                {
                    break;
                }

                weight++;
                var thisIteration = new List<WavePoint>();

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

            var result = new WaveResult
            {
                Path = path,
                Map = _map
            };

            return result;
        }

        private IEnumerable<WavePoint> FindNearestPoints(WavePoint current, WavePoint target, uint weight)
        {
            var targetWeight = weight - 1;
            
            /* center top */
            if (current.X == target.X && current.Y > target.Y)
            {
                var result = new List<WavePoint>
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
                var result = new List<WavePoint>
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
                var result = new List<WavePoint>
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
                var result = new List<WavePoint>
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
                var result = new List<WavePoint>
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
                var result = new List<WavePoint>
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
                var result = new List<WavePoint>
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
                var result = new List<WavePoint>
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

        private WavePoint GetPoint(int x, int y, uint targetWeight)
        {
            if (!ValidateEdges(x, y) || _map[x, y] != targetWeight)
            {
                return null;
            }

            return new WavePoint(x, y);
        }

        private WavePoint CreateNextStep(WavePoint parent, int xMove, int yMove, uint weight)
        {
            var newX = parent.X + xMove;
            var newY = parent.Y + yMove;

            if (!ValidateEdges(newX, newY))
            {
                return null;
            }

            if (_map[newX, newY] != null || PathFinderGlobal.TerrainField[newX, newY].Blocked)
            {
                return null;
            }

            WavePoint result = null;
            if (xMove == 0 || yMove == 0)
            {
                _map[newX, newY] = weight;
                result = new WavePoint(newX, newY);
            }
            else
            {
                if (!PathFinderGlobal.TerrainField[newX, parent.Y].Blocked
                    && !PathFinderGlobal.TerrainField[parent.X, newY].Blocked)
                {
                    _map[newX, newY] = weight;
                    result = new WavePoint(newX, newY);
                }
            }

            return result;
        }
    }
}