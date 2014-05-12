using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.PathFinder.Finder2D.Finder
{
    public abstract class BaseResult
    {
        public IEnumerable<Vector3> Path { get; set; }
    }
}