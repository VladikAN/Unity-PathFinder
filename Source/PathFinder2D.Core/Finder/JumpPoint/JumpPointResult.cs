using System.Collections.Generic;
using PathFinder2D.Core.Domain;
using UnityEngine;

namespace PathFinder2D.Core.Finder.JumpPoint
{
    public class JumpPointResult : FinderResult
    {
        public IEnumerable<Vector3> ControlPoints { get; set; }
    }
}