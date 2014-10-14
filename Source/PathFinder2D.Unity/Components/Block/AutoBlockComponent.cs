using System.Collections.Generic;
using PathFinder2D.Core.Domain.Map;
using UnityEngine;

namespace PathFinder2D.Unity.Components.Block
{
    [AddComponentMenu("Modules/PathFinder2D/Blocks/Auto Block")]
    public class AutoBlockComponent : MonoBehaviour, IBlockComponent
    {
        public IEnumerable<Vector3> GetPoints(MapDefinition mapDefinition)
        {
            if (mapDefinition == null || mapDefinition.Field == null)
            {
                return null;
            }

            var result = new List<Vector3>();
            var scale = transform.localScale;
            var fieldWidth = mapDefinition.CellSize / 2f;
            var width = (int)(scale.x / fieldWidth);
            var height = (int)(scale.z / fieldWidth);
            for (var x = 0; x < width; x++)
            {
                for (var z = 0; z < height; z++)
                {
                    var position = new Vector3(x, 0, z) * fieldWidth - scale / 2;
                    position = transform.TransformDirection(position);
                    position = position + transform.position;
                        
                    result.Add(position);
                }
            }

            return result;
        }
    }
}