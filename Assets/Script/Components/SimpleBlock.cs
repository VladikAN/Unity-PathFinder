using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Components
{
    public class SimpleBlock : MonoBehaviour
    {
        private bool _done;

        public void Start ()
        {
            _done = false;
        }

        public void Update ()
        {
            if (!_done && PathFinderGlobal.TerrainField != null)
            {
                var points = getPoints();
                foreach (var point in points)
                {
                    var x = (int)((point.x - PathFinderGlobal.TerrainStartX) / PathFinderGlobal.CellWidth);
                    var z = (int)((point.z - PathFinderGlobal.TerrainStartZ) / PathFinderGlobal.CellWidth);

                    PathFinderGlobal.TerrainField[x, z].Blocked = true;
                }

                _done = true;
            }
        }

        private IEnumerable<Vector3> getPoints()
        {
            var result = new List<Vector3>();
            if (PathFinderGlobal.TerrainField != null)
            {
                var scale = transform.localScale;
                var fieldWidth = PathFinderGlobal.CellWidth / 2f;

                var width = (int)(scale.x / fieldWidth);
                var height = (int)(scale.z / fieldWidth);

                Gizmos.color = Color.green;
                for (var i = 0; i < width; i++)
                {
                    for (var j = 0; j < height; j++)
                    {
                        var position = new Vector3(i * fieldWidth, 0, j * fieldWidth) - scale / 2;
                        position = transform.TransformDirection(position);
                        position = position + transform.position;
                        result.Add(position);
                    }
                }
            }

            return result;
        }
    }
}