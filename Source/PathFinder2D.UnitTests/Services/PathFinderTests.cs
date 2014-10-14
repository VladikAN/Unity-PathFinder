using System;
using NUnit.Framework;
using PathFinder2D.Core;
using PathFinder2D.UnitTests.Domain;

namespace PathFinder2D.UnitTests.Services
{
    [TestFixture]
    public class PathFinderTests
    {
        [Test, ExpectedException(typeof(ArgumentException))]
        public void Constructor_NullArgument_Exception()
        {
            var target = new PathFinderService(null);
        }

        [Test]
        public void Constructor_FinderInstance_Success()
        {
            var target = new PathFinderService(new FakeFinder());
        }
    }
}