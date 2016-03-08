using System;
using System.Collections.Generic;
using System.Linq;
using PathFinder2D.Core.Domain.Finder;

namespace PathFinder2D.Core.Extensions
{
    public static class PathExtensions
    {
        public static FinderPoint[] ToGeneral(this FinderPoint[] points)
        {
            if (points == null || points.Count() <= 1) return points;

            var prevX = 0;
            var prevY = 0;
            var indexes = new List<int>();

            for (var i = 0; i < points.Count() - 1; i++)
            {
                var f = points[i];
                var s = points[i + 1];

                var newX = f.X - s.X;
                var newY = f.Y - s.Y;

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

        public static FinderResult ToDetailed(this FinderResult finderResult)
        {
            return finderResult;
        }
    }
}