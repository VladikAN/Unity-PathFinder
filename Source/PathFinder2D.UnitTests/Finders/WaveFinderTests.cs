using NUnit.Framework;
using PathFinder2D.Core;
using PathFinder2D.Core.Extensions;
using PathFinder2D.Core.Finder;
using PathFinder2D.Core.Initializer;
using PathFinder2D.UnitTests.Stubs;

namespace PathFinder2D.UnitTests.Finders
{
    [TestFixture]
    public class WaveFinderTests
    {
        private IFinder _finder;
        private IMapInitializer _mapInitializer;
        private PathFinderService _pathFinderService;

        [SetUp]
        public void SetUp()
        {
            _finder = new WaveFinder();
            _mapInitializer = new MapInitializer();
            _pathFinderService = new PathFinderService(_finder, _mapInitializer);
        }

        [TestCase(0, 0, 9, 9, TestName = "(0, 0) => (9, 9)")]
        [TestCase(9, 0, 0, 9, TestName = "(9, 0) => (0, 9)")]
        [TestCase(5, 0, 5, 9, TestName = "(5, 0) => (5, 9)")]
        [TestCase(5, 9, 5, 0, TestName = "(5, 9) => (5, 0)")]
        public void Find_ValidPoints_Success(int sX, int sY, int eX, int eY)
        {
            var terrain = new FakeTerrain(1, 0, 0, 10, 10);
            var mapDefinition = _pathFinderService.InitMap(terrain, 1);
            
            var start = mapDefinition.ToVector3(sX, sY);
            var end = mapDefinition.ToVector3(eX, eY);

            var result = _pathFinderService.Find(mapDefinition.Terrain.Id(), start, end);
            AssertExtensions.IsValidPath(result, mapDefinition);
        }
    }
}