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

        private readonly IList<int[]> _movements = new List<int[]>()
        {
            new [] { 0, -1 }, /* center top */
            new [] { 1, -1 }, /* center right */
            new [] { 1, 0 },  /* right center */
            new [] { 1, 1 },  /* right bottom */
            new [] { 0, 1 },  /* center bottom */
            new [] { -1, 1 }, /* left bottom */
            new [] { -1, 0 }, /* left center */
            new [] { -1, -1 } /* left top */
        };
        
        public FinderResult Find(MapDefinition mapDefinition, Vector3 startVector3, Vector3 endVector3)
        {
            _mapDefinition = mapDefinition;
            _weightMap = new uint?[mapDefinition.FieldWidth, mapDefinition.FieldHeight];

            var startPoint = mapDefinition.Terrain.ToPoint<WavePoint>(startVector3);
            var endPoint = mapDefinition.Terrain.ToPoint<WavePoint>(endVector3);

            uint currentWeight = 0;
            _weightMap[startPoint.X, startPoint.Y] = currentWeight;

            var completed = false;
            var lastIteration = new List<WavePoint> { startPoint };
            while (!completed && lastIteration.Any())
            {
                currentWeight++;
                var thisIteration = new List<WavePoint>();

                foreach (var point in lastIteration)
                {
                    if (point.X == endPoint.X && point.Y == endPoint.Y)
                    {
                        completed = true;
                        break;
                    }

                    thisIteration.Add(CreateNextStep(point, -1, -1, currentWeight));
                    thisIteration.Add(CreateNextStep(point, 0, -1, currentWeight));
                    thisIteration.Add(CreateNextStep(point, 1, -1, currentWeight));
                    thisIteration.Add(CreateNextStep(point, 1, 0, currentWeight));
                    thisIteration.Add(CreateNextStep(point, 1, 1, currentWeight));
                    thisIteration.Add(CreateNextStep(point, 0, 1, currentWeight));
                    thisIteration.Add(CreateNextStep(point, -1, 1, currentWeight));
                    thisIteration.Add(CreateNextStep(point, -1, 0, currentWeight));
                }

                lastIteration = thisIteration.Where(x => x != null).ToList();
            }

            IList<Vector3> path = null;
            if (completed)
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

            var directionX = current.X == target.X ? 0 : (current.X < target.X ? 1 : -1);
            var directionY = current.Y == target.Y ? 0 : (current.Y < target.Y ? 1 : -1);

            var forwardIndex = _movements.First(x => x[0] == directionX && x[1] == directionY);
            var index = _movements.IndexOf(forwardIndex);
            var newMoves = new[] { index, index - 1, index + 1, index - 2, index + 2, index - 3, index + 3, index - 4 }
                .Select(raw => raw < 0 ? raw + 8 : raw)
                .Select(raw => raw > 7 ? raw - 8 : raw)
                .ToArray();

            var result = new List<WavePoint>
            {
                GetPoint(current.X + _movements[newMoves[0]][0], current.Y + _movements[newMoves[0]][1], targetWeight),
                GetPoint(current.X + _movements[newMoves[1]][0], current.Y + _movements[newMoves[1]][1], targetWeight),
                GetPoint(current.X + _movements[newMoves[2]][0], current.Y + _movements[newMoves[2]][1], targetWeight),
                GetPoint(current.X + _movements[newMoves[3]][0], current.Y + _movements[newMoves[3]][1], targetWeight),
                GetPoint(current.X + _movements[newMoves[4]][0], current.Y + _movements[newMoves[4]][1], targetWeight),
                GetPoint(current.X + _movements[newMoves[5]][0], current.Y + _movements[newMoves[5]][1], targetWeight),
                GetPoint(current.X + _movements[newMoves[6]][0], current.Y + _movements[newMoves[6]][1], targetWeight),
                GetPoint(current.X + _movements[newMoves[7]][0], current.Y + _movements[newMoves[7]][1], targetWeight)
            };

            result = result.Where(item => item != null).ToList();
            return result.ToArray();
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