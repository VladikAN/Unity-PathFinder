using System;
using NUnit.Framework;
using PathFinder2D.Core;
using PathFinder2D.UnitTests.Stubs;
using UnityEngine;

namespace PathFinder2D.UnitTests.Services
{
    [TestFixture]
    public class PathFinderServiceTests
    {
        #region Constructors

        [Test, ExpectedException(typeof(ArgumentException))]
        public void Constructor_AllNullArgument_Exception()
        {
            var target = new PathFinderService(null, null);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void Constructor_InitializerNullArgument_Exception()
        {
            var target = new PathFinderService(new FakeFinder(), null);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void Constructor_FinderNullArgument_Exception()
        {
            var target = new PathFinderService(null, new FakeMapInitializer());
        }

        [Test]
        public void Constructor_Instances_Success()
        {
            var target = new PathFinderService(new FakeFinder(), new FakeMapInitializer());
        }

        #endregion

        #region InitMap

        [Test, ExpectedException(typeof(ArgumentException))]
        public void InitMap_NullTerrain_Exception()
        {
            var target = new PathFinderService(new FakeFinder(), new FakeMapInitializer());
            target.InitMap(null, 1);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void InitMap_NegativeCellSize_Exception()
        {
            var target = new PathFinderService(new FakeFinder(), new FakeMapInitializer());
            target.InitMap(new FakeTerrain(1, 0, 0, 0, 0, 0), 0);
        }

        [Test]
        public void InitMap_ValidArgs_Exception()
        {
            var target = new PathFinderService(new FakeFinder(), new FakeMapInitializer());
            target.InitMap(new FakeTerrain(1, 0, 0, 0, 0, 1), 1);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void InitMap_InitTwice_Exception()
        {
            var target = new PathFinderService(new FakeFinder(), new FakeMapInitializer());
            target.InitMap(new FakeTerrain(1, 0, 0, 0, 0, 1), 1);
            target.InitMap(new FakeTerrain(1, 0, 0, 0, 0, 1), 1);
        }

        #endregion

        #region Find

        [Test, ExpectedException(typeof (ArgumentException))]
        public void Find_NotInitMap_Exception()
        {
            var target = new PathFinderService(new FakeFinder(), new FakeMapInitializer());
            target.Find(1, Vector3.zero, Vector3.one);
        }

        [Test]
        public void Find_InitMap_Success()
        {
            var target = new PathFinderService(new FakeFinder(), new FakeMapInitializer());
            target.InitMap(new FakeTerrain(1, 0, 0, 0, 0, 1), 1);
            target.Find(1, Vector3.zero, Vector3.one);
        }

        #endregion
    }
}