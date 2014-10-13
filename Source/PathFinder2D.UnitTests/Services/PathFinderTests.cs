using System;
using NUnit.Framework;
using PathFinder2D.Core;
using PathFinder2D.UnitTests.Domain;

namespace PathFinder2D.UnitTests.Services
{
    [TestFixture]
    public class PathFinderTests
    {
        private IPathFinderService _target;

        [SetUp]
        public void SetUp()
        {
            _target = new PathFinderService();
        }

        [Test, ExpectedException(typeof(NullReferenceException))]
        public void RegisterFinder_NullArgument_Exception()
        {
            _target.RegisterFinder<FakeFinder>(null);
        }

        [Test]
        public void RegisterFinder_InstanceArgument_Registered()
        {
            _target.RegisterFinder(new FakeFinder());
            var result = _target.ResolveFinder<FakeFinder>();

            Assert.NotNull(result);
        }
    }
}