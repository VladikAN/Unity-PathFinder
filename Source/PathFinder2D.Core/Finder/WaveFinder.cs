using System.Collections.Generic;
using System.Linq;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Extensions;
using UnityEngine;

namespace PathFinder2D.Core.Finder
{
    public class WaveFinder : IFinder
    {
        private uint?[,] _weightMap;
        private MapDefinition _mapDefinition;

        public FinderResult Find(MapDefinition mapDefinition, Vector3 startVector3, Vector3 endVector3)
        {
            _mapDefinition = mapDefinition;
            _weightMap = new uint?[mapDefinition.FieldWidth, mapDefinition.FieldHeight];

            var startPoint = mapDefinition.Terrain.ToPoint<WavePoint>(startVector3);
            var endPoint = mapDefinition.Terrain.ToPoint<WavePoint>(endVector3);

            uint weight = 0;
            _weightMap[startPoint.X, startPoint.Y] = weight;

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
                    path.Add(mapDefinition.Terrain.ToVector3(endPoint));
                    endPoint = FindNearestPoints(endPoint, startPoint, _weightMap[endPoint.X, endPoint.Y].Value).First();
                }

                path.Add(mapDefinition.Terrain.ToVector3(endPoint));
                path.Reverse();
            }

            var result = new FinderResult(path);
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
            if (!_mapDefinition.ValidateMapEdges(x, y) || _weightMap[x, y] != targetWeight)
            {
                return null;
            }

            return new WavePoint(x, y);
        }

        private WavePoint CreateNextStep(WavePoint parent, int xMove, int yMove, uint weight)
        {
            var newX = parent.X + xMove;
            var newY = parent.Y + yMove;

            if (!_mapDefinition.ValidateMapEdges(newX, newY))
            {
                return null;
            }

            if (_weightMap[newX, newY] != null || _mapDefinition.Field[newX, newY].Blocked)
            {
                return null;
            }

            if (xMove == 0 || yMove == 0)
            {
                _weightMap[newX, newY] = weight;
                return new WavePoint(newX, newY);
            }
            else
            {
                if (!_mapDefinition.Field[newX, parent.Y].Blocked || !_mapDefinition.Field[parent.X, newY].Blocked)
                {
                    _weightMap[newX, newY] = weight;
                    return new WavePoint(newX, newY);
                }
            }

            return null;
        }
    }
}