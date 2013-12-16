using UnityEngine;

namespace Assets.Script.Finder
{
    public interface IFinder
    {
        Vector3[] Find(Vector3 start, Vector3 end);
    }
}
