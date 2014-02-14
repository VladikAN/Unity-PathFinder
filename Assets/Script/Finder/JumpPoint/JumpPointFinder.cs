using System.Collections.Generic;
using System.Linq;
using Assets.Script.Extensions;
using UnityEngine;

namespace Assets.Script.Finder.JumpPoint
{
    public class JumpPointFinder : BaseFinder
    {
        private IList<JumpPointPoint> _points;
        private bool[,] _wallMap;
        private bool[,] _doneMap;

        private bool _endPointFounded;

        private JumpPointPoint _start;
        private JumpPointPoint _end;

        public override BaseResult Find(Vector3 start, Vector3 end)
        {
            _points = new List<JumpPointPoint>();
            _wallMap = PathFinderGlobal.TerrainField.ToBoolMap();
            _doneMap = PathFinderGlobal.TerrainField.ToBoolMap();

            _endPointFounded = false;

            _start = ToPoint<JumpPointPoint>(start);
            _end = ToPoint<JumpPointPoint>(end);
            
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

                Debug.Log("-> " + investigate.X + " x " + investigate.Y + " s:" + investigate.Step);
                if (investigate.X == _end.X && investigate.Y == _end.Y)
                {
                    pathFounded = true;
                    break;
                }

                while (investigate.Step != 0)
                {
                    MakeStep(investigate, !investigate.FromLeft, !investigate.FromUp);

                    if (_endPointFounded)
                    {
                        break;
                    }
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

                    if (_endPointFounded)
                    {
                        return;
                    }
                }

                /* Next step */
                investigate = new JumpPointPoint(investigate.X + stepH, investigate.Y + stepV);
            }

            start.NextStep();
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

            var markAsDone = !goLeft.HasValue;
            var investigate = new JumpPointPoint(start.X, start.Y);
            while (true)
            {
                if (!ValidateInvestigation(investigate)/* || _doneMap[investigate.X, investigate.Y]*/)
                {
                    investigate = null;
                    break;
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

                /* Next step */
                if (markAsDone)
                {
                    _doneMap[investigate.X, investigate.Y] = true;
                }
                
                investigate = new JumpPointPoint(investigate.X + stepH, investigate.Y + stepV);
                markAsDone = true;
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

        private bool AlreadyInStack(JumpPointPoint point)
        {
            return _points.Any(pt => pt.X == point.X && pt.Y == point.Y);
        }

        private void AddToStack(JumpPointPoint point, JumpPointPoint parent)
        {
            Debug.Log("R: " + point.X + " x " + point.Y + " s:" + point.Step);
            _points.Add(new JumpPointPoint(point.X, point.Y, parent));
            if (point.X == _end.X && point.Y == _end.Y)
            {
                _endPointFounded = true;
            }
        }

        private bool ValidateInvestigation(JumpPointPoint point)
        {
            if (!ValidateEdges(point))
            {
                return false;
            }

            if (_wallMap[point.X, point.Y])
            {
                return false;
            }

            return true;
        }
    }
}