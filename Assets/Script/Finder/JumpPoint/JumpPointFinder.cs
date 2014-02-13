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
        private bool _pathFounded;

        public override BaseResult Find(Vector3 start, Vector3 end)
        {
            _start = ToPoint<JumpPointPoint>(start);
            _end = ToPoint<JumpPointPoint>(end);
            _pathFounded = false;

            _points = new List<JumpPointPoint>();
            AddToStack(_start, null);

            JumpPointPoint investigate = null;
            while (!_pathFounded)
            {
                investigate = _points.FirstOrDefault(x => x.Step != 0);
                if (investigate == null)
                {
                    Debug.Log("Path not founded");
                    break;
                }

                if (investigate.X == _end.X && investigate.Y == _end.Y)
                {
                    _pathFounded = true;
                    break;
                }

                while (investigate.Step != 0)
                {
                    GoDiagonally(investigate, !investigate.FromLeft, !investigate.FromUp);
                    investigate.NextStep();
                }
            }

            var path = new List<Vector3>();
            if (_pathFounded)
            {
                while (investigate.X != _start.X & investigate.Y != _start.Y)
                {
                    path.Add(ToVector3(investigate));
                    investigate = investigate.Parent;
                }

                path.Reverse();
            }

            var result = new JumpPointResult { Path = path };
            return result;
        }

        private void GoDiagonally(JumpPointPoint start, bool left, bool up)
        {
            var stepH = left ? -1 : 1;
            var stepV = up ? -1 : 1;

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

        private JumpPointPoint GoHorizontally(JumpPointPoint start, bool left)
        {
            return GoHV(start, left, null);
        }

        private JumpPointPoint GoVertically(JumpPointPoint start, bool up)
        {
            return GoHV(start, null, up);
        }

        private JumpPointPoint GoHV(JumpPointPoint start, bool? left, bool? up)
        {
            if (left.HasValue && up.HasValue)
            {
                Debug.LogError("Only one must have a value!");
            }

            var stepH = left.HasValue ? (left.Value ? -1 : 1) : 0;
            var stepV = up.HasValue ? (up.Value ? -1 : 1) : 0;

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
                if (left.HasValue)
                {
                    if (HaveForcedNeighbor(investigate, true, left.Value, true) || HaveForcedNeighbor(investigate, true, left.Value, false))
                    {
                        break;
                    }
                }
                else
                {
                    if (HaveForcedNeighbor(investigate, false, true, up.Value) || HaveForcedNeighbor(investigate, false, false, up.Value))
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

        private bool HaveForcedNeighbor(JumpPointPoint investigate, bool horizontally, bool left, bool up)
        {
            var stepH = left ? -1 : 1;
            var stepV = up ? -1 : 1;

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