using System.Collections.Generic;
using PathFinder2D.Core.Domain;
using UnityEngine;

namespace PathFinder2D.Unity.Components.Block
{
    [AddComponentMenu("Modules/PathFinder2D/Blocks/Block")]
    public class BlockComponent : MonoBehaviour, IBlockComponent
	{
        public IEnumerable<Vector3> GetPoints(Map map)
        {
            if (map == null || map.TerrainField == null)
            {
                return null;
            }

            return new[] { transform.position };
        }
	}
}