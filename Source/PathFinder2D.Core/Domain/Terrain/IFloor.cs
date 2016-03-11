using System.Collections.Generic;

namespace PathFinder2D.Core.Domain.Terrain
{
    public interface IFloor
    {
        int Id();

        float X();
        float Y();
        float Width();
        float Height();

        float CellSize();

        IEnumerable<IBlock> GetBlocks();
    }
}