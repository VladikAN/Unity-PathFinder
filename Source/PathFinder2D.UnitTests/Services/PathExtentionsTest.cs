using System.Linq;
using NUnit.Framework;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Extensions;

namespace PathFinder2D.UnitTests.Services
{
    [TestFixture]
    public class PathExtentionsTest
    {
        #region ToMinimum

        [Test]
        public void ToMinimum_Horizontal_LeftToRight()
        {
            var points = new[]
            {
                new FinderPoint(0, 0),
                new FinderPoint(1, 0),
                new FinderPoint(2, 0),
                new FinderPoint(3, 0)
            };

            var newPoints = points.ToMinimum();
            Assert.That(newPoints.Count(), Is.EqualTo(2));
            Assert.That(newPoints[0].X, Is.EqualTo(0));
            Assert.That(newPoints[0].Y, Is.EqualTo(0));
            Assert.That(newPoints[1].X, Is.EqualTo(3));
            Assert.That(newPoints[1].Y, Is.EqualTo(0));
        }

        [Test]
        public void ToMinimum_Horizontal_RightToLeft()
        {
            var points = new[]
            {
                new FinderPoint(3, 0),
                new FinderPoint(2, 0),
                new FinderPoint(1, 0),
                new FinderPoint(0, 0)
            };

            var newPoints = points.ToMinimum();
            Assert.That(newPoints.Count(), Is.EqualTo(2));
            Assert.That(newPoints[0].X, Is.EqualTo(3));
            Assert.That(newPoints[0].Y, Is.EqualTo(0));
            Assert.That(newPoints[1].X, Is.EqualTo(0));
            Assert.That(newPoints[1].Y, Is.EqualTo(0));
        }
        [Test]
        public void ToMinimum_Vertical_UpToDown()
        {
            var points = new[]
            {
                new FinderPoint(0, 0),
                new FinderPoint(0, 1),
                new FinderPoint(0, 2),
                new FinderPoint(0, 3)
            };

            var newPoints = points.ToMinimum();
            Assert.That(newPoints.Count(), Is.EqualTo(2));
            Assert.That(newPoints[0].X, Is.EqualTo(0));
            Assert.That(newPoints[0].Y, Is.EqualTo(0));
            Assert.That(newPoints[1].X, Is.EqualTo(0));
            Assert.That(newPoints[1].Y, Is.EqualTo(3));
        }

        [Test]
        public void ToMinimum_Vertical_DownToUp()
        {
            var points = new[]
            {
                new FinderPoint(0, 3),
                new FinderPoint(0, 2),
                new FinderPoint(0, 1),
                new FinderPoint(0, 0)
            };

            var newPoints = points.ToMinimum();
            Assert.That(newPoints.Count(), Is.EqualTo(2));
            Assert.That(newPoints[0].X, Is.EqualTo(0));
            Assert.That(newPoints[0].Y, Is.EqualTo(3));
            Assert.That(newPoints[1].X, Is.EqualTo(0));
            Assert.That(newPoints[1].Y, Is.EqualTo(0));
        }

        [Test]
        public void ToMinimum_Diagonal_DownToUp()
        {
            var points = new[]
            {
                new FinderPoint(3, 3),
                new FinderPoint(2, 2),
                new FinderPoint(1, 1),
                new FinderPoint(0, 0)
            };

            var newPoints = points.ToMinimum();
            Assert.That(newPoints.Count(), Is.EqualTo(2));
            Assert.That(newPoints[0].X, Is.EqualTo(3));
            Assert.That(newPoints[0].Y, Is.EqualTo(3));
            Assert.That(newPoints[1].X, Is.EqualTo(0));
            Assert.That(newPoints[1].Y, Is.EqualTo(0));
        }

        [Test]
        public void ToMinimum_Diagonal_UpToDown()
        {
            var points = new[]
            {
                new FinderPoint(0, 0),
                new FinderPoint(1, 1),
                new FinderPoint(2, 2),
                new FinderPoint(3, 3)
            };

            var newPoints = points.ToMinimum();
            Assert.That(newPoints.Count(), Is.EqualTo(2));
            Assert.That(newPoints[0].X, Is.EqualTo(0));
            Assert.That(newPoints[0].Y, Is.EqualTo(0));
            Assert.That(newPoints[1].X, Is.EqualTo(3));
            Assert.That(newPoints[1].Y, Is.EqualTo(3));
        }

        #endregion

        #region ToMaximum

        [Test]
        public void ToMaximum_Horizontal_LeftToRight()
        {
            var points = new[]
            {
                new FinderPoint(0, 0),
                new FinderPoint(3, 0)
            };

            var newPoints = points.ToMaximum();
            Assert.That(newPoints.Count(), Is.EqualTo(4));
            Assert.That(newPoints[0].X, Is.EqualTo(0));
            Assert.That(newPoints[0].Y, Is.EqualTo(0));
            Assert.That(newPoints[1].X, Is.EqualTo(1));
            Assert.That(newPoints[1].Y, Is.EqualTo(0));
            Assert.That(newPoints[2].X, Is.EqualTo(2));
            Assert.That(newPoints[2].Y, Is.EqualTo(0));
            Assert.That(newPoints[3].X, Is.EqualTo(3));
            Assert.That(newPoints[3].Y, Is.EqualTo(0));
        }

        [Test]
        public void ToMaximum_Horizontal_RightToLeft()
        {
            var points = new[]
            {
                new FinderPoint(3, 0),
                new FinderPoint(0, 0)
            };

            var newPoints = points.ToMaximum();
            Assert.That(newPoints.Count(), Is.EqualTo(4));
            Assert.That(newPoints[0].X, Is.EqualTo(3));
            Assert.That(newPoints[0].Y, Is.EqualTo(0));
            Assert.That(newPoints[1].X, Is.EqualTo(2));
            Assert.That(newPoints[1].Y, Is.EqualTo(0));
            Assert.That(newPoints[2].X, Is.EqualTo(1));
            Assert.That(newPoints[2].Y, Is.EqualTo(0));
            Assert.That(newPoints[3].X, Is.EqualTo(0));
            Assert.That(newPoints[3].Y, Is.EqualTo(0));
        }

        [Test]
        public void ToMaximum_Vertical_UpToDown()
        {
            var points = new[]
            {
                new FinderPoint(0, 0),
                new FinderPoint(0, 3)
            };

            var newPoints = points.ToMaximum();
            Assert.That(newPoints.Count(), Is.EqualTo(4));
            Assert.That(newPoints[0].X, Is.EqualTo(0));
            Assert.That(newPoints[0].Y, Is.EqualTo(0));
            Assert.That(newPoints[1].X, Is.EqualTo(0));
            Assert.That(newPoints[1].Y, Is.EqualTo(1));
            Assert.That(newPoints[2].X, Is.EqualTo(0));
            Assert.That(newPoints[2].Y, Is.EqualTo(2));
            Assert.That(newPoints[3].X, Is.EqualTo(0));
            Assert.That(newPoints[3].Y, Is.EqualTo(3));
        }

        [Test]
        public void ToMaximum_Vertical_DownToUp()
        {
            var points = new[]
            {
                new FinderPoint(0, 3),
                new FinderPoint(0, 0)
            };

            var newPoints = points.ToMaximum();
            Assert.That(newPoints.Count(), Is.EqualTo(4));
            Assert.That(newPoints[0].X, Is.EqualTo(0));
            Assert.That(newPoints[0].Y, Is.EqualTo(3));
            Assert.That(newPoints[1].X, Is.EqualTo(0));
            Assert.That(newPoints[1].Y, Is.EqualTo(2));
            Assert.That(newPoints[2].X, Is.EqualTo(0));
            Assert.That(newPoints[2].Y, Is.EqualTo(1));
            Assert.That(newPoints[3].X, Is.EqualTo(0));
            Assert.That(newPoints[3].Y, Is.EqualTo(0));
        }

        [Test]
        public void ToMaximum_Diagonal_DownToUp()
        {
            var points = new[]
            {
                new FinderPoint(3, 3),
                new FinderPoint(0, 0)
            };

            var newPoints = points.ToMaximum();
            Assert.That(newPoints.Count(), Is.EqualTo(4));
            Assert.That(newPoints[0].X, Is.EqualTo(3));
            Assert.That(newPoints[0].Y, Is.EqualTo(3));
            Assert.That(newPoints[1].X, Is.EqualTo(2));
            Assert.That(newPoints[1].Y, Is.EqualTo(2));
            Assert.That(newPoints[2].X, Is.EqualTo(1));
            Assert.That(newPoints[2].Y, Is.EqualTo(1));
            Assert.That(newPoints[3].X, Is.EqualTo(0));
            Assert.That(newPoints[3].Y, Is.EqualTo(0));
        }

        [Test]
        public void ToMaximum_Diagonal_UpToDown()
        {
            var points = new[]
            {
                new FinderPoint(0, 0),
                new FinderPoint(3, 3)
            };

            var newPoints = points.ToMaximum();
            Assert.That(newPoints.Count(), Is.EqualTo(4));
            Assert.That(newPoints[0].X, Is.EqualTo(0));
            Assert.That(newPoints[0].Y, Is.EqualTo(0));
            Assert.That(newPoints[1].X, Is.EqualTo(1));
            Assert.That(newPoints[1].Y, Is.EqualTo(1));
            Assert.That(newPoints[2].X, Is.EqualTo(2));
            Assert.That(newPoints[2].Y, Is.EqualTo(2));
            Assert.That(newPoints[3].X, Is.EqualTo(3));
            Assert.That(newPoints[3].Y, Is.EqualTo(3));
        }

        #endregion
    }
}