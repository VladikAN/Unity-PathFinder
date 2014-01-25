using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Components.Block
{
    public class SimpleAutoBlock : BaseBlock
    {
        public void Start()
        {
        }

        public void Update()
        {
        }

        public override IEnumerable<Vector3> GetPoints()
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