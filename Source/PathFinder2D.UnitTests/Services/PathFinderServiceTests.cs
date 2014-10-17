using System;
using NUnit.Framework;
using PathFinder2D.Core;
using PathFinder2D.UnitTests.Stubs;

namespace PathFinder2D.UnitTests.Services
{
    [TestFixture]
    public class PathFinderServiceTests
    {
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
    }
}