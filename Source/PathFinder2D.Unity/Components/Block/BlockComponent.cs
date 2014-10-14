using System.Collections.Generic;
using PathFinder2D.Core.Domain.Map;
using UnityEngine;

namespace PathFinder2D.Unity.Components.Block
{
    [AddComponentMenu("Modules/PathFinder2D/Blocks/Block")]
    public class BlockComponent : MonoBehaviour, IBlockComponent
	{
        public IEnumerable<Vector3> GetPoints(MapDefinition mapDefinition)
        {
            if (mapDefinition == null || mapDefinition.Field == null)
            {
                return null;
            }

            return new[] { transform.position };
        }
	}
}