using System.Collections.Generic;
using System.Linq;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Extensions;
using UnityEngine;

namespace PathFinder2D.Core.Finder
{
    public class WaveFinder : Finder
    {
        private uint?[,] _weightMap;

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

        protected override FinderResult Find(Vector3 startVector3, Vector3 endVector3)
        {
            _weightMap = new uint?[MapWidth, MapHeight];

            var startPoint = MapDefinition.Terrain.ToPoint<WavePoint>(startVector3);
            var endPoint = MapDefinition.Terrain.ToPoint<WavePoint>(endVector3);

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

                    thisIteration.Add(CreateNextStep(point, _movements[0][0], _movements[0][1], currentWeight));
                    thisIteration.Add(CreateNextStep(point, _movements[1][0], _movements[1][1], currentWeight));
                    thisIteration.Add(CreateNextStep(point, _movements[2][0], _movements[2][1], currentWeight));
                    thisIteration.Add(CreateNextStep(point, _movements[3][0], _movements[3][1], currentWeight));
                    thisIteration.Add(CreateNextStep(point, _movements[4][0], _movements[4][1], currentWeight));
                    thisIteration.Add(CreateNextStep(point, _movements[5][0], _movements[5][1], currentWeight));
                    thisIteration.Add(CreateNextStep(point, _movements[6][0], _movements[6][1], currentWeight));
                    thisIteration.Add(CreateNextStep(point, _movements[7][0], _movements[7][1], currentWeight));
                }

                lastIteration = thisIteration.Where(x => x != null).ToList();
            }

            IList<Vector3> path = null;
            if (completed)
            {
                path = new List<Vector3>();

                while (endPoint.X != startPoint.X || endPoint.Y != startPoint.Y)
                {
                    path.Add(MapDefinition.Terrain.ToVector3(endPoint));
                    endPoint = FindNearestPoints(endPoint, startPoint, _weightMap[endPoint.X, endPoint.Y].Value).First();
                }

                path.Add(MapDefinition.Terrain.ToVector3(endPoint));
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
            if (!ValidateEdges(x, y) || _weightMap[x, y] != targetWeight)
            {
                return null;
            }

            return new WavePoint(x, y);
        }

        private WavePoint CreateNextStep(WavePoint parent, int xMove, int yMove, uint weight)
        {
            var newX = parent.X + xMove;
            var newY = parent.Y + yMove;

            if (!ValidateEdges(newX, newY)) return null;
            if (_weightMap[newX, newY] != null || MapDefinition.Field[newX, newY].Blocked) return null;

            if (xMove == 0 || yMove == 0)
            {
                _weightMap[newX, newY] = weight;
                return new WavePoint(newX, newY);
            }

            if (!MapDefinition.Field[newX, parent.Y].Blocked || !MapDefinition.Field[parent.X, newY].Blocked)
            {
                _weightMap[newX, newY] = weight;
                return new WavePoint(newX, newY);
            }

            return null;
        }
    }
}