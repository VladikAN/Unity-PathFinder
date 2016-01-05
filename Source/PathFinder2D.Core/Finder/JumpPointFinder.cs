using System;
using System.Collections.Generic;
using System.Linq;
using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Extensions;

namespace PathFinder2D.Core.Finder
{
    public class JumpPointFinder : Finder
    {
        private IList<JumpPoint> _openset;
        private bool[,] _wallMap;
        private bool[,] _openSetMap;

        private JumpPoint _startPoint;
        private JumpPoint _endPoint;

        protected override FinderResult Find(WorldPosition startVector3, WorldPosition endVector3)
        {
            _openset = new List<JumpPoint>();
            _wallMap = GetBoolMap();
            _openSetMap = new bool[MapWidth, MapHeight];

            _startPoint = MapDefinition.Terrain.ToPoint<JumpPoint>(startVector3);
            _endPoint = MapDefinition.Terrain.ToPoint<JumpPoint>(endVector3);

            /* Search */
            AddToStack(_startPoint, null);
            JumpPoint investigate;
            while (true)
            {
                investigate = _openset
                    .Where(x => x.Step != 0)
                    .OrderBy(point => point.Cost + Math.Sqrt(Math.Pow(point.X - _endPoint.X, 2) + Math.Pow(point.Y - _endPoint.Y, 2)))
                    .FirstOrDefault();

                if (investigate == null) break;
                if (investigate.X == _endPoint.X && investigate.Y == _endPoint.Y) break;

				while (investigate.Step != 0)
                {
                    MakeStep(investigate, investigate.ToLeft, investigate.ToUp);
                    investigate.NextStep();
                }
            }

            /* End */
            var path = new List<WorldPosition>();
            if (investigate != null)
            {
				var endPoint = _openset.First(point => point.X == _endPoint.X & point.Y == _endPoint.Y);
				while (endPoint.Parent != null)
                {
					path.Add(MapDefinition.Terrain.ToVector3(endPoint));
					endPoint = endPoint.Parent;
                }

                path.Add(MapDefinition.Terrain.ToVector3(endPoint));
                path.Reverse();
            }

            var result = new FinderResult(path);
            return result;
        }

        private void MakeStep(JumpPoint start, bool goLeft, bool goUp)
        {
            var investigate = new JumpPoint(start.X, start.Y);
            while (investigate != null)
            {
                var gotHorizontally = GoNext(investigate, start.ToLeft, null);
                var gotVertically = GoNext(investigate, null, start.ToUp);
                if (gotHorizontally != null || gotVertically != null)
                {
                    if (!SkipPoint(investigate.X, investigate.Y))
                    {
                        AddToStack(investigate, start);
						return;
                    }

                    var parent = _openset.First(point => point.X == investigate.X && point.Y == investigate.Y);
                    if (gotHorizontally != null && !SkipPoint(gotHorizontally.X, gotHorizontally.Y))
                    {
                        AddToStack(gotHorizontally, parent);
                    }

                    if (gotVertically != null && !SkipPoint(gotVertically.X, gotVertically.Y))
                    {
                        AddToStack(gotVertically, parent);
                    }
                }

                /* Next step */
                var stepH = goLeft ? -1 : 1;
                var stepV = goUp ? -1 : 1;
                investigate = GetNextInvestigation(investigate.X, investigate.Y, stepH, stepV);
            }
        }

        private JumpPoint GoNext(JumpPoint start, bool? left, bool? up)
        {
            var stepH = left.HasValue ? (left.Value ? -1 : 1) : 0;
            var stepV = up.HasValue ? (up.Value ? -1 : 1) : 0;

            var next = new JumpPoint(start.X, start.Y);
            while (next != null)
            {
                if (next.X == _endPoint.X && next.Y == _endPoint.Y)
                {
                    break;  /* Force exit */
                }

                /* Check neighbors */
                if (left.HasValue)
                {
                    if (!SkipPoint(next.X, next.Y) 
                        && (HaveForcedNeighbor(next.X, next.Y, true, left.Value, true)
                            || HaveForcedNeighbor(next.X, next.Y, true, left.Value, false)))
                    {
						break;
                    }
                }

                if (up.HasValue)
                {
                    if (!SkipPoint(next.X, next.Y)
                        && (HaveForcedNeighbor(next.X, next.Y, false, true, up.Value) 
                            || HaveForcedNeighbor(next.X, next.Y, false, false, up.Value)))
                    {
						break;
                    }
                }

                /* Next step */
                next = GetNextInvestigation(next.X, next.Y, stepH, stepV);
            }

            if (next == null)
            {
                return null;    /* found nothing */
            }

            if (!SkipPoint(start.X, start.Y))
            {
                return start;   /* add diagonal to stack */
            }

            if (!SkipPoint(next.X, next.Y))
            {
                return next; /* add horizontal/vertical to stack */
            }

            return null;
        }

        private bool HaveForcedNeighbor(int x, int y, bool hor, bool left, bool up)
        {
            var stepH = left ? -1 : 1;
            var stepV = up ? -1 : 1;

            if (!ValidateEdges(x + stepH, y + stepV))
            {
                return false;
            }

            if (hor && _wallMap[x, y + stepV])
            {
                return !_wallMap[x + stepH, y + stepV] && !_wallMap[x + stepH, y];
            }
            
            if (!hor && _wallMap[x + stepH, y])
            {
                return !_wallMap[x + stepH, y + stepV] && !_wallMap[x, y + stepV];
            }

            return false;
        }

        private JumpPoint GetNextInvestigation(int x, int y, int ax, int ay)
        {
            if (!ValidatePoint(x + ax, y + ay))
            {
                return null;
            }

            if (ax != 0 && ay != 0 && !ValidatePoint(x + ax, y) && !ValidatePoint(x, y + ay))
            {
                return null; /* Diagonal blocked */
            }

            var next = new JumpPoint(x + ax, y + ay);
            return SkipPoint(next.X, next.Y) ? null : next;
        }

        private bool SkipPoint(int x, int y)
        {
            return _openSetMap[x, y];
        }

        private void AddToStack(JumpPoint point, JumpPoint parent)
        {
            _openset.Add(new JumpPoint(point, parent));
            _openSetMap[point.X, point.Y] = true;
        }

        private bool ValidatePoint(int x, int y)
        {
            return ValidateEdges(x, y) && !_wallMap[x, y];
        }
    }
}