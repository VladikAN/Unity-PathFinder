using System;
using System.Collections.Generic;
using System.Linq;
using PathFinder2D.Core.Domain.Finder;

namespace PathFinder2D.Core.Extensions
{
    public static class PathExtensions
    {
        public static T[] ToGeneral<T>(this T[] points) where T : FinderPoint
        {
            if (points == null || points.Count() <= 1) return points;

            var prevX = 0;
            var prevY = 0;
            var indexes = new List<int>();

            for (var i = 0; i < points.Count() - 1; i++)
            {
                var f = points[i];
                var s = points[i + 1];

                var newX = s.X - f.X;
                var newY = s.Y - f.Y;

                if (newX == prevX && newY == prevY)
                {
                    indexes.Add(i);
                }

                prevX = newX;
                prevY = newY;
            }

            var newPoints = points.Where(pt => !indexes.Contains(Array.IndexOf(points, pt)));
            return newPoints.ToArray();
        }

        public static T[] ToDetailed<T>(this T[] points) where T : FinderPoint, new()
        {
            if (points == null || points.Count() <= 1) return points;

            var newPoints = new List<T>();
            for (var i = 0; i < points.Count() - 1; i++)
            {
                var f = points[i];
                var s = points[i + 1];

                var diffX = s.X - f.X;
                var diffY = s.Y - f.Y;
                var thisX = f.X;
                var thisY = f.Y;

                while (thisX != s.X || thisY != s.Y)
                {
                    newPoints.Add(new T { X = thisX, Y = thisY });
                    thisX += Math.Sign(diffX);
                    thisY += Math.Sign(diffY);
                }

                if (i == points.Count() - 2)
                {
                    newPoints.Add(s);
                }
            }

            return newPoints.ToArray();
        }
    }
}