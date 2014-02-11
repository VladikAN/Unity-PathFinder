using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Script.Finder.JumpPoint
{
    public class JumpPointFinder : BaseFinder
    {
        public override BaseResult Find(Vector3 start, Vector3 end)
        {
//            var startPoint = ToPoint<JumpPointPoint>(start);
//            var endPoint = ToPoint<JumpPointPoint>(end);

            throw new System.NotImplementedException();
        }

        private JumpPointPoint GoHorizontally(JumpPointPoint start, bool left)
        {
            var neighbors = GoHV(start, left, null);
            return null;
        }

        private JumpPointPoint GoVertically(JumpPointPoint start, bool up)
        {
            var neighbors = GoHV(start, null, up);
            return null;
        }

        private IEnumerable<JumpPointPoint> GoHV(JumpPointPoint start, bool? left, bool? up)
        {
            if (left.HasValue && up.HasValue)
            {
                Debug.Log("Only one must have a value!");
            }

            var stepH = left.HasValue ? (left.Value ? -1 : 1) : 0;
            var stepV = up.HasValue ? (up.Value ? -1 : 1) : 0;

            var investigate = new JumpPointPoint(start.X, start.Y);

            while (true)
            {
                if (!ValidateEdges(investigate) || IsBlocked(investigate))
                {
                    break;
                }

                // Check neighbors
                IEnumerable<JumpPointPoint> result;
                if (left.HasValue)
                {
                    result = new List<JumpPointPoint>
                    {
                        CheckNeighborHV(investigate, true, left.Value, true),
                        CheckNeighborHV(investigate, true, left.Value, false)
                    };
                }
                else
                {
                    result = new List<JumpPointPoint>
                    {
                        CheckNeighborHV(investigate, false, true, up.Value),
                        CheckNeighborHV(investigate, false, false, up.Value)
                    };
                }

                if (result.Any(x => x != null))
                {
                    result = result.Where(x => x != null);
                    return result;
                }

                // Next step
                investigate = new JumpPointPoint(investigate.X + stepH, investigate.Y + stepV);
            }

            return null;
        }

        private JumpPointPoint CheckNeighborHV(JumpPointPoint investigate, bool horizontally, bool left, bool up)
        {
            var stepH = left ? -1 : 1;
            var stepV = up ? -1 : 1;

            if (!ValidateEdges(investigate.X + stepH, investigate.Y + stepV))
            {
                return null;
            }

            /* horizontally */
            if (horizontally
                && IsBlocked(investigate.X, investigate.Y + stepV)
                && !IsBlocked(investigate.X + stepH, investigate.Y + stepV)
                && !IsBlocked(investigate.X + stepH, investigate.Y))
            {
                return null;    /* think */
            }

            /* Vertically */
            if (!horizontally
                && IsBlocked(investigate.X + stepH, investigate.Y)
                && !IsBlocked(investigate.X + stepH, investigate.Y + stepV)
                && !IsBlocked(investigate.X, investigate.Y + stepV))
            {
                return null;    /* think */
            }

            return null;
        }
    }
}