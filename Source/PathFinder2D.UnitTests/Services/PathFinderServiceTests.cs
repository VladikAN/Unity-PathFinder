using System;
using NUnit.Framework;
using PathFinder2D.Core;
using PathFinder2D.Core.Domain;
using PathFinder2D.UnitTests.Stubs;

namespace PathFinder2D.UnitTests.Services
{
    [TestFixture]
    public class PathFinderServiceTests
    {
        #region Constructors

        [Test]
        public void Constructor_AllNullArgument_Exception()
        {
            Assert.Throws<ArgumentException>(() => new PathFinderService(null, null));
        }

        [Test]
        public void Constructor_InitializerNullArgument_Exception()
        {
            Assert.Throws<ArgumentException>(() => new PathFinderService(new FakeFinder(), null));
        }

        [Test]
        public void Constructor_FinderNullArgument_Exception()
        {
            Assert.Throws<ArgumentException>(() => new PathFinderService(null, new FakeMapInitializer()));
        }

        [Test]
        public void Constructor_Instances_Success()
        {
            var target = new PathFinderService(new FakeFinder(), new FakeMapInitializer());
        }

        #endregion

        #region InitMap

        [Test]
        public void InitMap_NullTerrain_Exception()
        {
            var target = new PathFinderService(new FakeFinder(), new FakeMapInitializer());
            Assert.Throws<ArgumentException>(() => target.InitMap(null, 1));
        }

        [Test]
        public void InitMap_NegativeCellSize_Exception()
        {
            var target = new PathFinderService(new FakeFinder(), new FakeMapInitializer());
            Assert.Throws<ArgumentException>(() => target.InitMap(new FakeTerrain(1, 0, 0, 0, 0, 0), 0));
        }

        [Test]
        public void InitMap_ValidArgs_Exception()
        {
            var target = new PathFinderService(new FakeFinder(), new FakeMapInitializer());
            target.InitMap(new FakeTerrain(1, 0, 0, 0, 0, 1), 1);
        }

        [Test]
        public void InitMap_InitTwice_Exception()
        {
            var target = new PathFinderService(new FakeFinder(), new FakeMapInitializer());
            target.InitMap(new FakeTerrain(1, 0, 0, 0, 0, 1), 1);
            Assert.Throws<ArgumentException>(() => target.InitMap(new FakeTerrain(1, 0, 0, 0, 0, 1), 1));
        }

        #endregion

        #region Find

        [Test]
        public void Find_NotInitMap_Exception()
        {
            var target = new PathFinderService(new FakeFinder(), new FakeMapInitializer());
            Assert.Throws<ArgumentException>(() => target.FindPath(1, new WorldPosition(0, 0), new WorldPosition(1, 1)));
        }

        [Test]
        public void Find_InitMap_Success()
        {
            var target = new PathFinderService(new FakeFinder(), new FakeMapInitializer());
            target.InitMap(new FakeTerrain(1, 0, 0, 0, 0, 1), 1);
            target.FindPath(1, new WorldPosition(0, 0), new WorldPosition(1, 1));
        }

        #endregion
    }
}