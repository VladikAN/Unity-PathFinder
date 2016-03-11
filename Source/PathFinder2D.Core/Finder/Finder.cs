using System;
using System.Linq;
using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Extensions;

namespace PathFinder2D.Core.Finder
{
    public abstract class Finder<T> : IFinder where T : PathPoint, new()
    {
        protected MapDefinition MapDefinition;
        protected int MapWidth;
        protected int MapHeight;

        public PathResult Find(MapDefinition mapDefinition, WorldPosition start, WorldPosition end, SearchOptions options = SearchOptions.None)
        {
            MapDefinition = mapDefinition;
            MapWidth = mapDefinition.FieldWidth;
            MapHeight = mapDefinition.FieldHeight;

            var points = Find(start, end, options);
            var results = (ApplySettings(options, points) ?? Enumerable.Empty<T>())
                .Select(x => mapDefinition.Terrain.ToWorld(x))
                .ToArray();

            return new PathResult(results);
        }

        protected abstract T[] Find(WorldPosition start, WorldPosition end, SearchOptions options);

        protected T[] ApplySettings(SearchOptions options, T[] points)
        {
            switch (options)
            {
                case SearchOptions.None: return points;
                case SearchOptions.Minimum: return points.ToMinimum();
                case SearchOptions.Maximum: return points.ToMaximum();
                default: throw new NotSupportedException("This search option currently not supported");
            } 
        }

        protected bool ValidateEdges(int x, int y)
        {
            return x >= 0 && x < MapWidth && y >= 0 && y < MapHeight;
        }

        protected bool[,] GetBoolMap()
        {
            var width = MapDefinition.FieldWidth;
            var height = MapDefinition.FieldHeight;

            var result = new bool[width, height];
            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                    result[i, j] = MapDefinition.Field[i, j].Blocked;

            return result;
        }
    }
}