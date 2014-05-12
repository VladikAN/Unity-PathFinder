using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.PathFinder.Finder2D.Finder.JumpPoint
{
    public class JumpPointResult : BaseResult
    {
        public IEnumerable<Vector3> Neighbors { get; set; }
    }
}