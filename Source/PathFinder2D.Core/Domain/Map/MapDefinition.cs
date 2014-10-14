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

        public virtual float CellCorrection { get { return CellSize / 2; } }
        public virtual int Width { get { return Field.GetLength(0); } }
        public virtual int Height { get { return Field.GetLength(1); } }
        public virtual float X { get { return Terrain.TransformX() - Terrain.RenderX() / 2; } }
        public virtual float Y { get { return Terrain.TransformZ() - Terrain.RenderZ() / 2; } }

        public virtual MapCell[,] Field { get; private set; }
    }
}