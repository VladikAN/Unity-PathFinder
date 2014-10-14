using PathFinder2D.Core.Domain;
using UnityEngine;

namespace PathFinder2D.Core
{
    public interface IPathFinderService
    {
        Map InitMap(GameObject terrain, float cellWidth);
        FinderResult Find(int terrainId, Vector3 start, Vector3 end);
    }
}