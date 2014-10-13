using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Finder;
using UnityEngine;

namespace PathFinder2D.UnitTests.Domain
{
    public class FakeFinder : IFinder
    {
        public FinderResult Find(Map map, Vector3 startVector3, Vector3 endVector3)
        {
            throw new System.NotImplementedException();
        }
    }
}