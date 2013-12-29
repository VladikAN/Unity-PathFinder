using UnityEngine;

namespace Assets.Script.Components
{
    public class SimpleBlock : MonoBehaviour
    {
        private bool _done;

        public void Start ()
        {
        }

        public void Update ()
        {
            if (!_done && PathFinderGlobal.TerrainField != null)
            {
                var width = (int)(transform.gameObject.renderer.bounds.extents.x * 2 / PathFinderGlobal.CellWidth);
                var height = (int)(transform.gameObject.renderer.bounds.extents.z * 2 / PathFinderGlobal.CellWidth);

                Gizmos.color = Color.green;
                for (var i = 0; i < width; i++)
                {
                    for (var j = 0; j < height; j++)
                    {
                        var position = new Vector3(i * PathFinderGlobal.CellWidth, 0, j * PathFinderGlobal.CellWidth);
                    }
                }

                _done = true;
            }
        }

        public void OnDrawGizmosSelected()
        {
            if (PathFinderGlobal.TerrainField != null)
            {
                var scale = transform.localScale;
                var width = (int)(scale.x / PathFinderGlobal.CellWidth);
                var height = (int)(scale.z / PathFinderGlobal.CellWidth);

                Gizmos.color = Color.green;
                for (var i = 0; i < width; i++)
                {
                    for (var j = 0; j < height; j++)
                    {
                        var position = new Vector3(i * PathFinderGlobal.CellWidth + PathFinderGlobal.CellWidth / 2f, 0, j * PathFinderGlobal.CellWidth + PathFinderGlobal.CellWidth / 2f);
                        position = position - scale / 2;
                        position = transform.InverseTransformPoint(position);
                        position = new Vector3(position.x * transform.lossyScale.x, 0, position.z * transform.lossyScale.z);

                        Gizmos.DrawWireSphere(position, .3f);
                    }
                }
            }
        }
    }
}