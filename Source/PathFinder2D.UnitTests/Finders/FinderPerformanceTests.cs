using NUnit.Framework;
using PathFinder2D.Core;
using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Extensions;
using PathFinder2D.Core.Finder;
using PathFinder2D.Core.Initializer;
using PathFinder2D.UnitTests.Extensions;
using PathFinder2D.UnitTests.Stubs;
using System;
using System.Diagnostics;

namespace PathFinder2D.UnitTests.Finders
{
    [TestFixture]
    public class FinderPerformanceTests
    {
        private MapDefinition _testMap;
        private readonly Random _random = new Random();

        [TestFixtureSetUp]
        public void Init()
        {
            var width = 500;
            var height = 500;
            var walls = Math.Min(width, height);

            var field = new MapCell[width, height];
            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                    field[i, j] = new MapCell();

            for (var i = 0; i < walls; i++)
            {
                var w = _random.Next(1, width - 1);
                var h = _random.Next(1, height - 1);
                field[w, h].Blocked = true;
            }

            var terrain = new FakeTerrain(1, 0, 0, width, height, 1);
            _testMap = new MapDefinition(terrain, field, 1);
        }

        [TestCase(1, TestName = "Wave performance")]
        [TestCase(2, TestName = "Jump point performance")]
        public void PerformanceTests(int finderNumber)
        {
            var finder = finderNumber == 1 ? (BaseFinder) new WaveFinder() : new JumpPointFinder();

            var pathFinderService = new PathFinderService(finder, new MapInitializer());
            pathFinderService.GetMaps().Add(1, _testMap);

            var start = _testMap.Terrain.ToWorld(new FakeFinderPoint { X = 0, Y = 0 });
            var end = _testMap.Terrain.ToWorld(new FakeFinderPoint { X = _testMap.FieldWidth - 1, Y = _testMap.FieldHeight - 1 });

            var stopWatch = new Stopwatch();

            stopWatch.Start();
            var result = pathFinderService.FindPath(_testMap.Terrain.Id(), start, end);
            stopWatch.Stop();

            Console.WriteLine("Time taken: {0:D5} ms", stopWatch.ElapsedMilliseconds);
            AssertExtensions.IsValidPath(result, _testMap);
        }
    }
}