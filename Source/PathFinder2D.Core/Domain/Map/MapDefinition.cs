using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Terrain;

namespace PathFinder2D.Core.Domain.Map
{
    public class MapDefinition
    {
        public MapDefinition(IFloor terrain, MapCell[,] field, float cellSize)
        {
            Terrain = terrain;
            Field = field;
            CellSize = cellSize;
        }

        public IFloor Terrain;
        public float CellSize;
        public PathResult LastFinderResult;

        public virtual int Id { get { return Terrain.Id(); } }
        public virtual MapCell[,] Field { get; private set; }
        public virtual int FieldWidth { get { return Field.GetLength(0); } }
        public virtual int FieldHeight { get { return Field.GetLength(1); } }
    }
}