using PathFinder2D.Core.Domain;
using UnityEngine;

namespace PathFinder2D.Core.Finder
{
    public interface IFinder
    {
        FinderResult Find(Map map, Vector3 startVector3, Vector3 endVector3);
    }
}