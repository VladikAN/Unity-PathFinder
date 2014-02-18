using Assets.Script.Finder.JumpPoint;
using Assets.Script.Finder.JumpPoint.Gizmo;
using Assets.Script.Finder.Wave;
using UnityEngine;

namespace Assets.Script.Demo
{
    public class FinderCamera : MonoBehaviour
    {
        public DemoFinderType UseFinderType = DemoFinderType.JumpPoint;

        private Vector3 _startPoint = new Vector3(0, 0, 0);
        private Vector3 _endPoint = new Vector3(0, 0, 0);

        public void Start()
        {
            PathFinderGlobal.CellWidth = 1;
            PathFinderGlobal.RegisterFinder(new WaveFinder());
            PathFinderGlobal.RegisterFinder(new JumpPointFinder());
            PathFinderGlobal.RegisterGizmo<JumpPointResult, JumpPointGizmo>(new JumpPointGizmo());
        }
	
        public void Update()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Input.GetMouseButtonUp(0) && Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 31))
            {
                _endPoint = hit.point;
                switch (UseFinderType)
                {
                    case DemoFinderType.Wave:
                        PathFinderGlobal.Find<WaveFinder>(_startPoint, _endPoint);
                        break;
                    case DemoFinderType.JumpPoint:
                        PathFinderGlobal.Find<JumpPointFinder>(_startPoint, _endPoint);
                        break;
                    default:
                        Debug.Log("Not supported finder type");
                        break;
                }

                _startPoint = _endPoint;
            }
        }

        public enum DemoFinderType
        {
            Wave = 1,
            JumpPoint = 2
        }
    }
}