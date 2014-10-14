using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Domain.Map;
using UnityEngine;

namespace PathFinder2D.Core.Finder
{
    public interface IFinder
    {
        FinderResult Find(MapDefinition mapDefinition, Vector3 startVector3, Vector3 endVector3);
    }
}