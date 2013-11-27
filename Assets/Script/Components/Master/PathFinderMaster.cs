using UnityEngine;

namespace Assets.Script.Components.Master
{
    public sealed class PathFinderMaster : MonoBehaviour
    {
        public void Start()
        {
            PathFinderGlobal.Master = gameObject;
        }
	
        public void Update()
        {
        }

        public void OnDrawGizmosSelected()
        {
        }
    }
}