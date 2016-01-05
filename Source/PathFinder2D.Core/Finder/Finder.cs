using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;

namespace PathFinder2D.Core.Finder
{
    public abstract class Finder
    {
        protected MapDefinition MapDefinition;
        protected int MapWidth;
        protected int MapHeight;

        public FinderResult Find(MapDefinition mapDefinition, WorldPosition startVector3, WorldPosition endVector3)
        {
            MapDefinition = mapDefinition;
            MapWidth = mapDefinition.FieldWidth;
            MapHeight = mapDefinition.FieldHeight;

            return Find(startVector3, endVector3);
        }

        protected abstract FinderResult Find(WorldPosition startVector3, WorldPosition endVector3);

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