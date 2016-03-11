using System.Collections.Generic;
using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Domain.Terrain;
using UnityEngine;

namespace PathFinder2D.Unity.Components.Block
{
    [AddComponentMenu("Modules/PathFinder2D/Blocks/Block")]
    public class BlockComponent : MonoBehaviour, IBlock
    {
        public bool UpdatePosition;

        public IEnumerable<WorldPosition> GetPoints(IFloor floor)
        {
            return new[] { new WorldPosition(transform.position.x, transform.position.z) };
        }
	}
}