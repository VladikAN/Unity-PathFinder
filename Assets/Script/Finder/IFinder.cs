using UnityEngine;

namespace Assets.Script.Finder
{
    public interface IFinder
    {
        FinderResult Find(Vector3 start, Vector3 end);
    }
}
