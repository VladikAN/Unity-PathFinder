using System.Linq;
using Assets.Script.Components.Block;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.Components
{
    public class Terrain : MonoBehaviour
    {
        public bool DisplayFieldGizmo = false;
        public bool DisplayPathGizmo = false;
        public bool DisplayMapGizmo = false;

        public void Start()
        {
            PathFinderGlobal.Terrain = gameObject;
            updateGrid();
        }

        public void Update()
        {
        }

        public void OnDrawGizmosSelected()
        {
            if (PathFinderGlobal.TerrainField == null)
            {
                return;
            }

            var startX = PathFinderGlobal.TerrainStartX;
            var startZ = PathFinderGlobal.TerrainStartZ;

            var fieldWidth = PathFinderGlobal.TerrainFieldWidth;
            var fieldHeight = PathFinderGlobal.TerrainFieldHeight;
            var gizmoSize = new Vector3(PathFinderGlobal.CellWidth, 1, PathFinderGlobal.CellWidth) - new Vector3(0.1f, 0f, 0.1f);

            if (DisplayFieldGizmo)
            {
                var correction = PathFinderGlobal.CellCorrection;
                for (var i = 0; i < fieldWidth; i++)
                {
                    var x = startX + PathFinderGlobal.CellWidth * i + correction;
                    for (var j = 0; j < fieldHeight; j++)
                    {
                        Gizmos.color = PathFinderGlobal.TerrainField[i, j] != null && PathFinderGlobal.TerrainField[i, j].Blocked
                            ? new Color(125, 0, 0)
                            : new Color(0, 125, 0);

                        var z = startZ + PathFinderGlobal.CellWidth * j + correction;
                        var startPosition = new Vector3(x, 0.5f, z) + new Vector3(0.1f, 0f, 0.1f);
                        Gizmos.DrawWireCube(startPosition, gizmoSize);
                    }
                }
            }

            if (DisplayPathGizmo || DisplayMapGizmo)
            {
                var finderResult = PathFinderGlobal.LastResult;
                if (finderResult != null)
                {
                    if (DisplayPathGizmo && (finderResult.Path != null && finderResult.Path.Any()))
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawWireSphere(finderResult.Path.First(), .2f);
                        Gizmos.DrawWireSphere(finderResult.Path.Last(), .2f);

                        Gizmos.color = Color.red;
                        foreach (var point in finderResult.Path)
                        {
                            Gizmos.DrawWireSphere(point, .1f);
                        }
                    }
                    
                    if (DisplayMapGizmo)
                    {
                        for (var i = 0; i < fieldWidth; i++)
                        {
                            var x = startX + PathFinderGlobal.CellWidth * i;
                            for (var j = 0; j < fieldHeight; j++)
                            {
                                if (finderResult.Map[i, j] == null)
                                {
                                    continue;
                                }

                                var z = startZ + PathFinderGlobal.CellWidth * j;
                                var startPosition = new Vector3(x, 0.5f, z) + new Vector3(0.1f, 0f, 0.1f);
                                Handles.Label(startPosition, finderResult.Map[i, j].Value.ToString());
                            }
                        }
                    }
                }
            }
        }

        private void updateGrid()
        {
            var blocks = (BaseBlock[])FindObjectsOfType(typeof(BaseBlock));
            var points = blocks.SelectMany(x => x.GetPoints());

            foreach (var point in points)
            {
                var x = (int)((point.x - PathFinderGlobal.TerrainStartX) / PathFinderGlobal.CellWidth);
                var z = (int)((point.z - PathFinderGlobal.TerrainStartZ) / PathFinderGlobal.CellWidth);
                
                PathFinderGlobal.TerrainField[x, z].Blocked = true;
            }
        }
    }
}