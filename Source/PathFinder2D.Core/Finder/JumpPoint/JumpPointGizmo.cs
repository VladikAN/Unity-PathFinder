using PathFinder2D.Core.Domain;
using UnityEngine;

namespace PathFinder2D.Core.Finder.JumpPoint
{
    public class JumpPointGizmo : IGizmo
    {
        public void DisplayGizmo(FinderResult finderResult)
        {
            var jumpPointResult = finderResult as JumpPointResult;
            if (jumpPointResult == null)
            {
                return;
            }

            var saveColor = Gizmos.color;
            Gizmos.color = Color.yellow;
            
            foreach (var neighbor in jumpPointResult.ControlPoints)
            {
                Gizmos.DrawWireSphere(neighbor, .3f);
            }

            Gizmos.color = saveColor;
        }
    }
}