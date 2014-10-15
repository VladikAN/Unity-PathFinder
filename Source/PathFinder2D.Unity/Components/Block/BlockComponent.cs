using System.Collections.Generic;
using PathFinder2D.Core.Domain.Terrain;
using UnityEngine;

namespace PathFinder2D.Unity.Components.Block
{
    [AddComponentMenu("Modules/PathFinder2D/Blocks/Block")]
    public class BlockComponent : MonoBehaviour, IBlock
	{
        public IEnumerable<Vector3> GetPoints(ITerrain terrain)
        {
            return new[] { transform.position };
        }
	}
}