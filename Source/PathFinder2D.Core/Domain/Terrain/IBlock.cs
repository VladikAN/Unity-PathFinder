using System.Collections.Generic;

namespace PathFinder2D.Core.Domain.Terrain
{
    public interface IBlock
    {
        /// <summary>
        /// Initiate single obstacle position by set of points. This points will be used to create map
        /// </summary>
        IEnumerable<WorldPosition> GetPoints(IFloor floor);
    }
}