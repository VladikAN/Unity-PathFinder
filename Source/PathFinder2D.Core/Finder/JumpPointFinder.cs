﻿using System.Collections.Generic;
using System.Linq;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Extensions;
using UnityEngine;

namespace PathFinder2D.Core.Finder
{
    public class JumpPointFinder : Finder
    {
        private IList<JumpPoint> _openset;
        private bool[,] _wallMap;
        private bool[,] _openSetMap;

        private JumpPoint _startPoint;
        private JumpPoint _endPoint;

        protected override FinderResult Find(Vector3 startVector3, Vector3 endVector3)
        {
            _openset = new List<JumpPoint>();
            _wallMap = GetBoolMap();
            _openSetMap = new bool[MapWidth, MapHeight];

            _startPoint = MapDefinition.Terrain.ToPoint<JumpPoint>(startVector3);
            _endPoint = MapDefinition.Terrain.ToPoint<JumpPoint>(endVector3);

            AddToStack(_startPoint, null);
            JumpPoint investigate;
            while (true)
            {
                investigate = _openset
                    .Where(x => x.Step != 0)
                    .OrderBy(point => point.Cost + Mathf.Sqrt(Mathf.Pow(point.X - _endPoint.X, 2) + Mathf.Pow(point.Y - _endPoint.Y, 2)))
                    .FirstOrDefault();

                if (investigate == null) break;
                if (investigate.X == _endPoint.X && investigate.Y == _endPoint.Y) break;

				while (investigate.Step != 0)
                {
                    MakeStep(investigate, investigate.ToLeft, investigate.ToUp);
                    investigate.NextStep();
                }
            }

            var path = new List<Vector3>();
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
            var stepH = goLeft ? -1 : 1;
            var stepV = goUp ? -1 : 1;

            var investigate = new JumpPoint(start.X, start.Y);
            while (investigate != null)
            {
                var gotHorizontally = GoHV(investigate, start.ToLeft, null);
                var gotVertically = GoHV(investigate, null, start.ToUp);
                if (gotHorizontally != null || gotVertically != null)
                {
                    if (!AlreadyInStack(investigate))
                    {
                        AddToStack(investigate, start);
						return;
                    }

                    var parent = _openset.First(point => point.X == investigate.X && point.Y == investigate.Y);
                    if (gotHorizontally != null && !AlreadyInStack(gotHorizontally))
                    {
                        AddToStack(gotHorizontally, parent);
                    }

                    if (gotVertically != null && !AlreadyInStack(gotVertically))
                    {
                        AddToStack(gotVertically, parent);
                    }
                }

                /* Next step */
                investigate = GetNextInvestigation(investigate, stepH, stepV);
            }
        }

        private JumpPoint GoHV(JumpPoint start, bool? goLeft, bool? goUp)
        {
            var stepH = goLeft.HasValue ? (goLeft.Value ? -1 : 1) : 0;
            var stepV = goUp.HasValue ? (goUp.Value ? -1 : 1) : 0;

            var investigate = new JumpPoint(start.X, start.Y);
            while (investigate != null)
            {
                if (investigate.X == _endPoint.X && investigate.Y == _endPoint.Y)
                {
                    break;  /* Force exit */
                }

                /* Check neighbors */
                if (goLeft.HasValue)
                {
                    if (!AlreadyInStack(investigate) 
                        && (HaveForcedNeighbor(investigate, true, goLeft.Value, true)
                            || HaveForcedNeighbor(investigate, true, goLeft.Value, false)))
                    {
						break;
                    }
                }

                if (goUp.HasValue)
                {
                    if (!AlreadyInStack(investigate)
                        && (HaveForcedNeighbor(investigate, false, true, goUp.Value) 
                            || HaveForcedNeighbor(investigate, false, false, goUp.Value)))
                    {
						break;
                    }
                }

                /* Next step */
                investigate = GetNextInvestigation(investigate, stepH, stepV);
            }

            if (investigate == null)
            {
                return null;    /* found nothing */
            }

            if (!AlreadyInStack(start))
            {
                return start;   /* add diagonal to stack */
            }

            if (!AlreadyInStack(investigate))
            {
                return investigate; /* add horizontal/vertical to stack */
            }

            return null;
        }

        private bool HaveForcedNeighbor(JumpPoint investigate, bool horizontally, bool goLeft, bool goUp)
        {
            var stepH = goLeft ? -1 : 1;
            var stepV = goUp ? -1 : 1;

            if (!ValidateEdges(investigate.X + stepH, investigate.Y + stepV))
            {
                return false;
            }

            if (horizontally && _wallMap[investigate.X, investigate.Y + stepV])
            {
                return !_wallMap[investigate.X + stepH, investigate.Y + stepV] && !_wallMap[investigate.X + stepH, investigate.Y];
            }
            
            if (!horizontally && _wallMap[investigate.X + stepH, investigate.Y])
            {
                return !_wallMap[investigate.X + stepH, investigate.Y + stepV] && !_wallMap[investigate.X, investigate.Y + stepV];
            }

            return false;
        }

        private JumpPoint GetNextInvestigation(JumpPoint investigation, int stepH, int stepV)
        {
            if (!ValidateInvestigation(investigation.X + stepH, investigation.Y + stepV))
            {
                return null;
            }

            if (stepH != 0 && stepV != 0)
            {
                if (!ValidateInvestigation(investigation.X + stepH, investigation.Y) && !ValidateInvestigation(investigation.X, investigation.Y + stepV))
                {
                    return null; /* Diagonal blocked */
                }
            }

            var nextInvestigation = new JumpPoint(investigation.X + stepH, investigation.Y + stepV);
            return AlreadyInStack(nextInvestigation) ? null : nextInvestigation;
        }

        private bool AlreadyInStack(JumpPoint point)
        {
            return _openSetMap[point.X, point.Y];
        }

        private void AddToStack(JumpPoint point, JumpPoint parent)
        {
            var cost = parent == null ? 0 : parent.Cost + Mathf.Sqrt(Mathf.Pow(point.X - parent.X, 2) + Mathf.Pow(point.Y - parent.Y, 2));
            _openset.Add(new JumpPoint(point.X, point.Y, parent, cost));
            _openSetMap[point.X, point.Y] = true;
        }

        private bool ValidateInvestigation(int x, int y)
        {
            return ValidateEdges(x, y) && !_wallMap[x, y];
        }
    }
}