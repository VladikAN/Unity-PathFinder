using System.Collections.Generic;
using PathFinder2D.Core.Domain.Terrain;
using UnityEngine;

namespace PathFinder2D.Unity.Components.Block
{
    [AddComponentMenu("Modules/PathFinder2D/Blocks/Auto Block")]
    public class AutoBlockComponent : MonoBehaviour, IBlock
    {
        public IEnumerable<Vector3> GetPoints(ITerrain terrain)
        {
            var result = new List<Vector3>();
            var scale = transform.localScale;
            var quality = terrain.CellSize() / 2;

            var width = (int)(scale.x / quality);
            var height = (int)(scale.z / quality);
            for (var x = 0; x < width; x++)
            {
                for (var z = 0; z < height; z++)
                {
                    var position = new Vector3(x, 0, z) * quality - scale / 2;
                    position = transform.TransformDirection(position);
                    position = position + transform.position;
                        
                    result.Add(position);
                }
            }

            return result;
        }
    }
}