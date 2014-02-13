using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Script.Finder.JumpPoint
{
    public class JumpPointFinder : BaseFinder
    {
        private IList<JumpPointPoint> _points;

        private JumpPointPoint _start;
        private JumpPointPoint _end;

        public override BaseResult Find(Vector3 start, Vector3 end)
        {
            _start = ToPoint<JumpPointPoint>(start);
            _end = ToPoint<JumpPointPoint>(end);
            
            _points = new List<JumpPointPoint>();
            AddToStack(_start, null);

            var pathFounded = false;
            JumpPointPoint investigate = null;
            while (!pathFounded)
            {
                investigate = _points.FirstOrDefault(point => (point.X == _end.X && point.Y == _end.Y) || point.Step != 0);
                if (investigate == null)
                {
                    Debug.Log("Path not founded");
                    break;
                }

                if (investigate.X == _end.X && investigate.Y == _end.Y)
                {
                    pathFounded = true;
                    break;
                }

                while (investigate.Step != 0)
                {
//                    Debug.Log(investigate.X + " x " + investigate.Y + " s:" + investigate.Step);
                    GoDiagonally(investigate, !investigate.FromLeft, !investigate.FromUp);
                    investigate.NextStep();
                }
            }

            var path = new List<Vector3>();
            if (pathFounded)
            {
                while (investigate.Parent != null)
                {
                    path.Add(ToVector3(investigate));
                    investigate = investigate.Parent;
                }

                path.Add(ToVector3(investigate));
                path.Reverse();
            }

            var result = new JumpPointResult
            {
                Path = path,
                Neighbors = _points.Select(x => ToVector3(x))
            };
            
            return result;
        }

        private void GoDiagonally(JumpPointPoint start, bool goLeft, bool goUp)
        {
            var stepH = goLeft ? -1 : 1;
            var stepV = goUp ? -1 : 1;

            var investigate = new JumpPointPoint(start.X, start.Y);
            while (true)
            {
                if (!ValidateEdges(investigate) || IsBlocked(investigate))
                {
                    break;
                }

                var gotHorizontally = GoHorizontally(investigate, !investigate.FromLeft);
                var gotVertically = GoVertically(investigate, !investigate.FromUp);
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
                }

                // Next step
                investigate = new JumpPointPoint(investigate.X + stepH, investigate.Y + stepV);
            }
        }

        private JumpPointPoint GoHorizontally(JumpPointPoint start, bool goLeft)
        {
            return GoHV(start, goLeft, null);
        }

        private JumpPointPoint GoVertically(JumpPointPoint start, bool goUp)
        {
            return GoHV(start, null, goUp);
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
                if (!ValidateEdges(investigate) || IsBlocked(investigate))
                {
                    investigate = null;
                    break;
                }

                if (investigate.X == _end.X && investigate.Y == _end.Y)
                {
                    /* Force exit */
                    break;
                }

                // Check neighbors
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

                // Next step
                investigate = new JumpPointPoint(investigate.X + stepH, investigate.Y + stepV);
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

            if (horizontally)
            {
                return IsBlocked(investigate.X, investigate.Y + stepV) && (!IsBlocked(investigate.X + stepH, investigate.Y + stepV) && !IsBlocked(investigate.X + stepH, investigate.Y));
            }
            else
            {
                return IsBlocked(investigate.X + stepH, investigate.Y) && (!IsBlocked(investigate.X + stepH, investigate.Y + stepV) && !IsBlocked(investigate.X, investigate.Y + stepV));
            }
        }

        private bool AlreadyInStack(JumpPointPoint point)
        {
            return _points.Any(pt => pt.X == point.X && pt.Y == point.Y);
        }

        private void AddToStack(JumpPointPoint point, JumpPointPoint parent)
        {
            _points.Add(new JumpPointPoint(point.X, point.Y, parent));
        }
    }
}