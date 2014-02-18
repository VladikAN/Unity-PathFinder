using UnityEngine;

namespace Assets.Script.Finder.JumpPoint.Gizmo
{
    public class JumpPointGizmo : BaseGizmo
    {
        public override void DisplayGizmo(BaseResult finderResult)
        {
            var jumpPointResult = finderResult as JumpPointResult;
            
            Gizmos.color = Color.yellow;
            foreach (var neighbor in jumpPointResult.Neighbors)
            {
                Gizmos.DrawWireSphere(neighbor, .3f);
            }
        }
    }
}