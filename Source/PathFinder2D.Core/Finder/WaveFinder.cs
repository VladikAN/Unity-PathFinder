using System.Collections.Generic;
using System.Linq;
using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Extensions;

namespace PathFinder2D.Core.Finder
{
    public class WaveFinder : BaseFinder
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

        protected override FinderResult Find(WorldPosition start, WorldPosition end)
        {
            _weightMap = new uint?[MapWidth, MapHeight];

            var startPoint = MapDefinition.Terrain.ToPoint<WavePoint>(start);
            var endPoint = MapDefinition.Terrain.ToPoint<WavePoint>(end);

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

            IList<WorldPosition> path = null;
            if (completed)
            {
                path = new List<WorldPosition>();

                while (endPoint.X != startPoint.X || endPoint.Y != startPoint.Y)
                {
                    path.Add(MapDefinition.Terrain.ToWorld(endPoint));
                    var weight = _weightMap[endPoint.X, endPoint.Y];
                    endPoint = FindNearestPoints(endPoint, startPoint, weight ?? 0);
                }

                path.Add(MapDefinition.Terrain.ToWorld(endPoint));
                path = path.Reverse().ToList();
            }

            var result = new FinderResult(path);
            return result;
        }

        private WavePoint FindNearestPoints(WavePoint current, WavePoint target, uint weight)
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

            foreach (var newMove in newMoves)
            {
                var point = GetPoint(current.X + _movements[newMove][0], current.Y + _movements[newMove][1], targetWeight);
                if (point != null) return point;
            }

            return null;
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