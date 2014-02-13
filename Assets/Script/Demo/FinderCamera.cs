using Assets.Script.Finder;
using Assets.Script.Finder.JumpPoint;
using Assets.Script.Finder.Wave;
using UnityEngine;

namespace Assets.Script.Demo
{
    public class FinderCamera : MonoBehaviour
    {
        private Vector3 _startPoint = new Vector3(0, 0, 0);
        private Vector3 _endPoint = new Vector3(0, 0, 0);

        public void Start()
        {
        }
	
        public void Update()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Input.GetMouseButtonUp(0) && Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 31))
            {
                _endPoint = hit.point;
                PathFinderGlobal.Find(new JumpPointFinder(), _startPoint, _endPoint);
                _startPoint = _endPoint;
            }
        }
    }
}