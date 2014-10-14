using System.Collections.Generic;
using PathFinder2D.Core.Domain.Map;
using UnityEngine;

namespace PathFinder2D.Unity.Components.Block
{
    public interface IBlockComponent
    {
        IEnumerable<Vector3> GetPoints(MapDefinition mapDefinition);
    }
}