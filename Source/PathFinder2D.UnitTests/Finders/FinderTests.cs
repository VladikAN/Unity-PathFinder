﻿using NUnit.Framework;
using PathFinder2D.Core;
using PathFinder2D.Core.Extensions;
using PathFinder2D.Core.Finder;
using PathFinder2D.Core.Initializer;
using PathFinder2D.UnitTests.Extensions;
using PathFinder2D.UnitTests.Stubs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PathFinder2D.UnitTests.Finders
{
    [TestFixture]
    public class FinderTests
    {
        private static IEnumerable<IFinder> GlobalFinders
        {
            get
            {
                /* Get all classes which realize IFinder */
                var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName != typeof (FinderTests).Assembly.FullName);
                var types = assemblies.SelectMany(assembly => assembly.GetTypes()).Where(type => typeof(IFinder).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);
                var finders = types.Select(Activator.CreateInstance).OfType<IFinder>();
                
                return finders;
            }
        }

        #region Simple Move

        private static IEnumerable<TestCaseData> SimpleMoveSource
        {
            get
            {
                var rawCases = new[]
                {
                    new TestCaseData(0, 1, 2, 1).SetName("Map 3x3. From left to right"),
                    new TestCaseData(2, 1, 0, 1).SetName("Map 3x3. From right to left"),
                    new TestCaseData(1, 0, 1, 2).SetName("Map 3x3. From up to down"),
                    new TestCaseData(1, 2, 1, 0).SetName("Map 3x3. From down to up"),
                    new TestCaseData(0, 0, 2, 2).SetName("Map 3x3. From top left to bottom right"),
                    new TestCaseData(2, 2, 0, 0).SetName("Map 3x3. From bottom right to top left"),
                    new TestCaseData(2, 0, 0, 0).SetName("Map 3x3. From top right to bottom left"),
                    new TestCaseData(0, 0, 2, 0).SetName("Map 3x3. From bottom left to top right")
                };

                var result = rawCases.CombineArguments(GlobalFinders.OfType<object>().ToArray());
                return result;
            }
        }

        [TestCaseSource("SimpleMoveSource")]
        public void SimpleMove_Empty_Success(int sX, int sY, int eX, int eY, IFinder finder)
        {
            var raw = new[]
            {
                "...",
                "...",
                "...",
            };

            var mapDefinition = raw.ParseDefinition();

            var pathFinderService = new PathService(finder, new Map());
            pathFinderService.GetMaps().Add(1, mapDefinition);

            var start = mapDefinition.Terrain.ToWorld(new FakePoint { X = sX, Y = sY });
            var end = mapDefinition.Terrain.ToWorld(new FakePoint { X = eX, Y = eY });

            var result = pathFinderService.FindPath(mapDefinition.Terrain.Id(), start, end);
            AssertExtensions.IsValidPath(result, mapDefinition);
        }

        [TestCaseSource("SimpleMoveSource")]
        public void SimpleMove_SingleWall_Success(int sX, int sY, int eX, int eY, IFinder finder)
        {
            var raw = new[]
            {
                "...",
                ".#.",
                "...",
            };

            var mapDefinition = raw.ParseDefinition();

            var pathFinderService = new PathService(finder, new Map());
            pathFinderService.GetMaps().Add(1, mapDefinition);

            var start = mapDefinition.Terrain.ToWorld(new FakePoint { X = sX, Y = sY });
            var end = mapDefinition.Terrain.ToWorld(new FakePoint { X = eX, Y = eY });

            var result = pathFinderService.FindPath(mapDefinition.Terrain.Id(), start, end);
            AssertExtensions.IsValidPath(result, mapDefinition);
        }

        #endregion

        #region Specific

        [TestCase(0, 1, 4, 1)]
        public void Wave_Horizontal_Success(int sX, int sY, int eX, int eY)
        {
            var raw = new[]
            {
                ".....",
                ".###.",
                ".....",
            };

            var mapDefinition = raw.ParseDefinition();

            var pathFinderService = new PathService(new WaveFinder(), new Map());
            pathFinderService.GetMaps().Add(1, mapDefinition);

            var start = mapDefinition.Terrain.ToWorld(new FakePoint { X = sX, Y = sY });
            var end = mapDefinition.Terrain.ToWorld(new FakePoint { X = eX, Y = eY });

            var result = pathFinderService.FindPath(mapDefinition.Terrain.Id(), start, end);
            AssertExtensions.IsValidPath(result, mapDefinition);
        }

        [TestCase(1, 0, 1, 4)]
        public void Wave_Vertical_Success(int sX, int sY, int eX, int eY)
        {
            var raw = new[]
            {
                "...",
                ".#.",
                ".#.",
                ".#.",
                "...",
            };

            var mapDefinition = raw.ParseDefinition();

            var pathFinderService = new PathService(new WaveFinder(), new Map());
            pathFinderService.GetMaps().Add(1, mapDefinition);

            var start = mapDefinition.Terrain.ToWorld(new FakePoint { X = sX, Y = sY });
            var end = mapDefinition.Terrain.ToWorld(new FakePoint { X = eX, Y = eY });

            var result = pathFinderService.FindPath(mapDefinition.Terrain.Id(), start, end);
            AssertExtensions.IsValidPath(result, mapDefinition);
        }

        [TestCase(0, 1, 4, 1)]
        public void Jump_Horizontal_Success(int sX, int sY, int eX, int eY)
        {
            var raw = new[]
            {
                ".....",
                ".###.",
                ".....",
            };

            var mapDefinition = raw.ParseDefinition();

            var pathFinderService = new PathService(new JumpPointFinder(), new Map());
            pathFinderService.GetMaps().Add(1, mapDefinition);

            var start = mapDefinition.Terrain.ToWorld(new FakePoint { X = sX, Y = sY });
            var end = mapDefinition.Terrain.ToWorld(new FakePoint { X = eX, Y = eY });

            var result = pathFinderService.FindPath(mapDefinition.Terrain.Id(), start, end);
            AssertExtensions.IsValidPath(result, mapDefinition);
        }

        [TestCase(1, 0, 1, 4)]
        public void Jump_Vertical_Success(int sX, int sY, int eX, int eY)
        {
            var raw = new[]
            {
                "...",
                ".#.",
                ".#.",
                ".#.",
                "...",
            };

            var mapDefinition = raw.ParseDefinition();

            var pathFinderService = new PathService(new JumpPointFinder(), new Map());
            pathFinderService.GetMaps().Add(1, mapDefinition);

            var start = mapDefinition.Terrain.ToWorld(new FakePoint { X = sX, Y = sY });
            var end = mapDefinition.Terrain.ToWorld(new FakePoint { X = eX, Y = eY });

            var result = pathFinderService.FindPath(mapDefinition.Terrain.Id(), start, end);
            AssertExtensions.IsValidPath(result, mapDefinition);
        }

        #endregion

        #region Blocked Move

        private static IEnumerable<TestCaseData> BlockedMoveSource
        {
            get
            {
                var rawCases = new[]
                {
                    new TestCaseData(0, 0, 2, 0).SetName("Map 3x1. From left to right"),
                    new TestCaseData(2, 0, 0, 0).SetName("Map 3x1. From right to left"),
                    /*new TestCaseData(1, 0, 2, 0).SetName("Map 3x1. From obstacle to free"), Returns valid path. Fine*/
                    new TestCaseData(2, 0, 1, 0).SetName("Map 3x1. From free to obstacle")
                };

                var result = rawCases.CombineArguments(GlobalFinders.OfType<object>().ToArray());
                return result;
            }
        }

        [TestCaseSource("BlockedMoveSource")]
        public void BlockedMove_SingleWall_PathNotFounded(int sX, int sY, int eX, int eY, IFinder finder)
        {
            var raw = new[] { ".#." };

            var mapDefinition = raw.ParseDefinition();

            var pathFinderService = new PathService(finder, new Map());
            pathFinderService.GetMaps().Add(1, mapDefinition);

            var start = mapDefinition.Terrain.ToWorld(new FakePoint { X = sX, Y = sY });
            var end = mapDefinition.Terrain.ToWorld(new FakePoint { X = eX, Y = eY });

            var result = pathFinderService.FindPath(mapDefinition.Terrain.Id(), start, end);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Path == null || !result.Path.Any());
        }

        #endregion
    }
}