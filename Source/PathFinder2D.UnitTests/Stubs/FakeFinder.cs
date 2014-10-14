using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Finder;
using UnityEngine;

namespace PathFinder2D.UnitTests.Stubs
{
    public class FakeFinder : IFinder
    {
        public FinderResult Find(MapDefinition mapDefinition, Vector3 startVector3, Vector3 endVector3)
        {
            throw new System.NotImplementedException();
        }
    }
}