using NUnit.Framework;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Extensions;

namespace PathFinder2D.UnitTests
{
    internal static class AssertExtensions
    {
        public static void IsValidPath(FinderResult result, MapDefinition mapDefinition)
        {
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Path);
            Assert.IsNotEmpty(result.Path);

            foreach (var vector3 in result.Path)
            {
                var point = mapDefinition.ToPoint(vector3);
                Assert.IsFalse(mapDefinition.Field[point[0], point[1]].Blocked);
            }
        }
    }
}