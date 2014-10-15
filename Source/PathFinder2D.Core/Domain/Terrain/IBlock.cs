using System.Collections.Generic;
using UnityEngine;

namespace PathFinder2D.Core.Domain.Terrain
{
    public interface IBlock
    {
        IEnumerable<Vector3> GetPoints(ITerrain terrain);
    }
}