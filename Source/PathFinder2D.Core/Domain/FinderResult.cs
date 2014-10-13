using System.Collections.Generic;
using UnityEngine;

namespace PathFinder2D.Core.Domain
{
    public abstract class FinderResult
    {
        public IEnumerable<Vector3> Path { get; set; }
    }
}