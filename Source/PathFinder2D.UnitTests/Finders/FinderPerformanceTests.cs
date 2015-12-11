using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PathFinder2D.Core;
using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Extensions;
using PathFinder2D.Core.Finder;
using PathFinder2D.Core.Initializer;
using PathFinder2D.UnitTests.Extensions;
using PathFinder2D.UnitTests.Stubs;

namespace PathFinder2D.UnitTests.Finders
{
    [TestFixture]
    public class FinderPerformanceTests
    {
        private MapDefinition _testMap;
        private Random _random = new Random();

        [TestFixtureSetUp]
        public void Init()
        {
            var width = 1024;
            var height = 1024;
            var walls = 1024;

            /* Field */
            var raw = new string[height];
            for (var i = 0; i < width; i++)
            {
                raw[i] = string.Join("", Enumerable.Range(0, width).Select(x => ".").ToArray());
            }

            /* Walls */
            for (var i = 0; i < walls; i++)
            {
                var w = _random.Next(10, width - 10);
                var h = _random.Next(10, height - 10);

                var sb = new StringBuilder(raw[h]);
                sb[w] = '#';
                raw[h] = sb.ToString();
            }

            _testMap = raw.ParseDefinition();
        }

        [Test]
        public void Wave_PerformanceTests()
        {
            var pathFinderService = new PathFinderService(new WaveFinder(), new MapInitializer());
            pathFinderService.GetMaps().Add(1, _testMap);

            var start = _testMap.Terrain.ToVector3(new FakeFinderPoint { X = 0, Y = 0 });
            var end = _testMap.Terrain.ToVector3(new FakeFinderPoint { X = 1023, Y = 1023 });

            var result = pathFinderService.FindPath(_testMap.Terrain.Id(), start, end);
            AssertExtensions.IsValidPath(result, _testMap);
        }
    }
}