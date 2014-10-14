using System.Collections.Generic;
using UnityEngine;

namespace PathFinder2D.Core.Domain.Finder
{
    public class FinderResult
    {
        public IEnumerable<Vector3> Path;

        public FinderResult(IEnumerable<Vector3> path)
        {
            Path = path;
        }
    }
}