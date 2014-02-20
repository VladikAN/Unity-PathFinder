﻿using System.Collections.Generic;
using System.Linq;
using Assets.Script.Extensions;
using UnityEngine;

namespace Assets.Script.Finder.JumpPoint
{
    public class JumpPointFinder : BaseFinder
    {
        private IList<JumpPointPoint> _openset;
        private bool[,] _wallMap;
        private bool[,] _doneMap;

        private bool _endPointFounded;

        private JumpPointPoint _start;
        private JumpPointPoint _end;

        public override BaseResult Find(Vector3 startVector3, Vector3 endVector3)
        {
            _openset = new List<JumpPointPoint>();
            _wallMap = PathFinderGlobal.TerrainField.ToBoolMap();
            _doneMap = PathFinderGlobal.TerrainField.ToBoolMap();

            _endPointFounded = false;

            _start = ToPoint<JumpPointPoint>(startVector3);
            _end = ToPoint<JumpPointPoint>(endVector3);
            
            AddToStack(_start, null);
            while (!_endPointFounded)
            {
                var investigate = _openset
                    .Where(x => x.Step != 0)
                    .OrderBy(point => point.Cost + Mathf.Sqrt(Mathf.Pow(point.X - _end.X, 2) + Mathf.Pow(point.Y - _end.Y, 2)))
                    .FirstOrDefault();

                if (investigate == null)
                {
                    Debug.Log("Path not founded");
                    break;
                }

                while (!_endPointFounded && investigate.Step != 0)
                {
                    MakeStep(investigate, !investigate.FromLeft, !investigate.FromUp);
                }
            }

            var path = new List<Vector3>();
            if (_endPointFounded)
            {
                var endPoint = _openset.First(point => point.X == _end.X && point.Y == _end.Y);
                while (endPoint.Parent != null)
                {
                    path.Add(ToVector3(endPoint));
                    endPoint = endPoint.Parent;
                }

                path.Add(ToVector3(endPoint));
                path.Reverse();
            }

            var result = new JumpPointResult
            {
                Path = path,
                Neighbors = _openset.Select(x => ToVector3(x))
            };
            
            return result;
        }

        private void MakeStep(JumpPointPoint start, bool goLeft, bool goUp)
        {
            var stepH = goLeft ? -1 : 1;
            var stepV = goUp ? -1 : 1;

            var investigate = new JumpPointPoint(start.X, start.Y);
            while (true)
            {
                if (!ValidateInvestigation(investigate))
                {
                    break;
                }

				if (AlreadyInStack(investigate))
				{
					investigate = _openset.First(point => point.X == investigate.X && point.Y == investigate.Y);
				}

                var gotHorizontally = GoHV(investigate, !investigate.FromLeft, null);
                var gotVertically = GoHV(investigate, null, !investigate.FromUp);
                if (gotHorizontally != null || gotVertically != null)
                {
                    if (!AlreadyInStack(investigate))
                    {
                        AddToStack(investigate, start);
                        break;
                    }

                    if (gotHorizontally != null && !AlreadyInStack(gotHorizontally))
                    {
                        AddToStack(gotHorizontally, investigate);
                    }

                    if (gotVertically != null && !AlreadyInStack(gotVertically))
                    {
                        AddToStack(gotVertically, investigate);
                    }

                    if (_endPointFounded)
                    {
                        return;
                    }
                }

                /* Next step */
                investigate = GetNextInvestigation(investigate, stepH, stepV);
            }

            start.NextStep();
        }

        private JumpPointPoint GoHV(JumpPointPoint start, bool? goLeft, bool? goUp)
        {
            if (goLeft.HasValue && goUp.HasValue)
            {
                Debug.LogError("Only one must have a value!");
            }

            var stepH = goLeft.HasValue ? (goLeft.Value ? -1 : 1) : 0;
            var stepV = goUp.HasValue ? (goUp.Value ? -1 : 1) : 0;

            var investigate = new JumpPointPoint(start.X, start.Y);
            while (true)
            {
                if (!ValidateInvestigation(investigate)/* || _doneMap[investigate.X, investigate.Y]*/)
                {
                    investigate = null;
                    break;
                }
                
                if (AlreadyInStack(investigate))
                {
                    investigate = GetNextInvestigation(investigate, stepH, stepV);
                    continue;
                }

                if (investigate.X == _end.X && investigate.Y == _end.Y)
                {
                    /* Force exit */
                    break;
                }

                /* Check neighbors */
                if (goLeft.HasValue)
                {
                    if (HaveForcedNeighbor(investigate, true, goLeft.Value, true) || HaveForcedNeighbor(investigate, true, goLeft.Value, false))
                    {
                        break;
                    }
                }
                else
                {
                    if (HaveForcedNeighbor(investigate, false, true, goUp.Value) || HaveForcedNeighbor(investigate, false, false, goUp.Value))
                    {
                        break;
                    }
                }

                if (investigate.X != start.X || investigate.Y != start.Y)
                {
                    _doneMap[investigate.X, investigate.Y] = true;
                }

                /* Next step */
                investigate = GetNextInvestigation(investigate, stepH, stepV);
            }

            if (investigate != null)
            {
                if (!AlreadyInStack(start))
                {
                    return start;
                }

                if (!AlreadyInStack(investigate))
                {
                    return investigate;
                }
            }

            return null;
        }

        private bool HaveForcedNeighbor(JumpPointPoint investigate, bool horizontally, bool goLeft, bool goUp)
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

        private JumpPointPoint GetNextInvestigation(JumpPointPoint investigation, int stepH, int stepV)
        {
            return new JumpPointPoint(investigation.X + stepH, investigation.Y + stepV);
        }

        private bool AlreadyInStack(JumpPointPoint point)
        {
            return _openset.Any(pt => pt.X == point.X && pt.Y == point.Y);
        }

        private void AddToStack(JumpPointPoint point, JumpPointPoint parent)
        {
            _openset.Add(new JumpPointPoint(point.X, point.Y, parent));
            _endPointFounded |= (point.X == _end.X && point.Y == _end.Y);
        }

        private bool ValidateInvestigation(JumpPointPoint point)
        {
            return ValidateEdges(point) && !_wallMap[point.X, point.Y];
        }
    }
}