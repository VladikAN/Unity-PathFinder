using Assets.Script.Components.Debug.Utils;
using UnityEngine;

namespace Assets.Script.Components.Debug
{
    public sealed class PathFinderDebug : MonoBehaviour
    {
        public DisplaySettings DisplaySettings = 0;
        public GizmoSettings GizmoSettings = new GizmoSettings();

        public void Start()
        {
            PathFinderGlobal.Debug = gameObject;
        }
    
        public void Update()
        {
        }

        public void OnDrawGizmosSelected()
        {
//            Gizmos.color = Color.gray;
//            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}