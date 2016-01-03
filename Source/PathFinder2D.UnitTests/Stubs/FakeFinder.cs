using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Finder;
using UnityEngine;

namespace PathFinder2D.UnitTests.Stubs
{
    public class FakeFinder : Finder
    {
        protected override FinderResult Find(Vector3 startVector3, Vector3 endVector3)
        {
            return new FinderResult(null);
        }
    }
}