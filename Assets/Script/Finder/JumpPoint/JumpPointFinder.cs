using System.Collections.Generic;
using System.Linq;
using Assets.Script.Extensions;
using UnityEngine;

namespace Assets.Script.Finder.JumpPoint
{
    public class JumpPointFinder : BaseFinder
    {
        private IList<JumpPointPoint> _openset;
        private bool[,] _wallMap;

        private JumpPointPoint _start;
        private JumpPointPoint _end;

        public override BaseResult Find(Vector3 startVector3, Vector3 endVector3)
        {
            _openset = new List<JumpPointPoint>();
            _wallMap = PathFinderGlobal.TerrainField.ToBoolMap();

            _start = ToPoint<JumpPointPoint>(startVector3);
            _end = ToPoint<JumpPointPoint>(endVector3);

            AddToStack(_start, null);
            JumpPointPoint investigate;
            while (true)
            {
                investigate = _openset
                    .Where(x => x.Step != 0)
                    .OrderBy(point => point.Cost)
                    .FirstOrDefault();

                if (investigate == null)
                {
                    Debug.Log("Path not founded");
                    break;
                }

                if (investigate.X == _end.X && investigate.Y == _end.Y)
                {
                    break;
                }

				while (investigate.Step != 0)
                {
                    MakeStep(investigate, investigate.ToLeft, investigate.ToUp);
                    investigate.NextStep();
                }
            }

            var path = new List<Vector3>();
            if (investigate != null)
            {
				var endPoint = _openset.First(point => point.X == _end.X & point.Y == _end.Y);
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

        private JumpPointPoint GoHV(JumpPointPoint start, bool? goLeft, bool? goUp)
        {
            var stepH = goLeft.HasValue ? (goLeft.Value ? -1 : 1) : 0;
            var stepV = goUp.HasValue ? (goUp.Value ? -1 : 1) : 0;

            var investigate = new JumpPointPoint(start.X, start.Y);
            while (investigate != null)
            {
                if (investigate.X == _end.X && investigate.Y == _end.Y)
                {
                    break;  /* Force exit */
                }

                /* Check neighbors */
                if (goLeft.HasValue)
                {
                    if (!AlreadyInStack(investigate) && (HaveForcedNeighbor(investigate, true, goLeft.Value, true) || HaveForcedNeighbor(investigate, true, goLeft.Value, false)))
                    {
						break;
                    }
                }
                else
                {
                    if (!AlreadyInStack(investigate) && (HaveForcedNeighbor(investigate, false, true, goUp.Value) || HaveForcedNeighbor(investigate, false, false, goUp.Value)))
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

            var nextInvestigation = new JumpPointPoint(investigation.X + stepH, investigation.Y + stepV);
            return AlreadyInStack(nextInvestigation) ? null : nextInvestigation;
        }

        private bool AlreadyInStack(JumpPointPoint point)
        {
            return _openset.Any(pt => pt.X == point.X && pt.Y == point.Y);
        }

        private void AddToStack(JumpPointPoint point, JumpPointPoint parent)
        {
            var toEnd = Mathf.Sqrt(Mathf.Pow(point.X - _end.X, 2) + Mathf.Pow(point.Y - _end.Y, 2));
            var toParent = parent == null ? 0 : parent.Cost + Mathf.Sqrt(Mathf.Pow(point.X - parent.X, 2) + Mathf.Pow(point.Y - parent.Y, 2));

            _openset.Add(new JumpPointPoint(point.X, point.Y, parent, toEnd + toParent));
        }

        private bool ValidateInvestigation(int x, int y)
        {
            return ValidateEdges(x, y) && !_wallMap[x, y];
        }
    }
}