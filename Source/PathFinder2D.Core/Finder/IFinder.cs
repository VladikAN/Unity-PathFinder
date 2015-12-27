using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;
using UnityEngine;

namespace PathFinder2D.Core.Finder
{
    public interface IFinder
    {
        /// <summary>Find valid path from start to end through the map</summary>
        FinderResult Find(MapDefinition mapDefinition, Vector3 startVector3, Vector3 endVector3);
    }
}