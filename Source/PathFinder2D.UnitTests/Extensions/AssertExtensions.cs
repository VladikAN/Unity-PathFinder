using NUnit.Framework;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Extensions;
using PathFinder2D.UnitTests.Stubs;

namespace PathFinder2D.UnitTests.Extensions
{
    internal static class AssertExtensions
    {
        public static void IsValidPath(PathResult result, MapDefinition mapDefinition)
        {
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Path);
            Assert.IsNotEmpty(result.Path);

            foreach (var vector3 in result.Path)
            {
                var point = mapDefinition.Terrain.ToPoint<FakePoint>(vector3);
                Assert.IsFalse(mapDefinition.Field[point.X, point.Y].Blocked);
            }
        }
    }
}