using System.Collections.Generic;
using PathFinder2D.Core.Domain;
using UnityEngine;

namespace PathFinder2D.Unity.Components.Block
{
    public interface IBlockComponent
    {
        IEnumerable<Vector3> GetPoints(Map map);
    }
}