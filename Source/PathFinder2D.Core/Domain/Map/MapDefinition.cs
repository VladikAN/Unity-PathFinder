using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Terrain;

namespace PathFinder2D.Core.Domain.Map
{
    public class MapDefinition
    {
        public MapDefinition(
            ITerrain terrain,
            MapCell[,] field,
            float cellSize)
        {
            Terrain = terrain;
            Field = field;
            CellSize = cellSize;
        }

        public ITerrain Terrain;
        public float CellSize;
        public FinderResult LastFinderResult;

        public virtual int Id { get { return Terrain.Id(); } }
        public virtual MapCell[,] Field { get; }
        public virtual int FieldWidth { get { return Field.GetLength(0); } }
        public virtual int FieldHeight { get { return Field.GetLength(1); } }
    }
}