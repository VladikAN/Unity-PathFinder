using UnityEngine;

namespace PathFinder2D.Unity.Components
{
    [AddComponentMenu("Modules/PathFinder2D/Camera")]
    public class FinderCamera : MonoBehaviour
    {
        public void Update()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Input.GetMouseButtonUp(0) && Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 31))
            {
                var point = hit.point;
            }
        }
    }
}