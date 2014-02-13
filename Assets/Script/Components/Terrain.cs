using System.Linq;
using Assets.Script.Components.Block;
using Assets.Script.Finder.JumpPoint;
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
            var gizmoSize = new Vector3(PathFinderGlobal.CellWidth, 1, PathFinderGlobal.CellWidth) * 0.9f;

            if (DisplayFieldGizmo)
            {
                var labelCorrection = new Vector3(0.1f, 0, 0.35f);
                var gizmoCorrection = new Vector3(PathFinderGlobal.CellCorrection, 0.5f, PathFinderGlobal.CellCorrection);
                for (var i = 0; i < fieldWidth; i++)
                {
                    for (var j = 0; j < fieldHeight; j++)
                    {
                        var x = startX + PathFinderGlobal.CellWidth * i;
                        var z = startZ + PathFinderGlobal.CellWidth * j;

                        var labelPosition = new Vector3(x, 0, z) + labelCorrection;
                        var gizmoPosition = new Vector3(x, 0, z) + gizmoCorrection;
                        
                        var color = PathFinderGlobal.TerrainField[i, j] != null && PathFinderGlobal.TerrainField[i, j].Blocked
                            ? new Color(125, 0, 0)
                            : new Color(0, 125, 0);
                        Gizmos.color = color;

                        Gizmos.DrawWireCube(gizmoPosition, gizmoSize);
                        Handles.Label(labelPosition, string.Format("{0} x {1}", i, j));
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
                        Gizmos.DrawWireSphere(finderResult.Path.First(), .4f);
                        Gizmos.DrawWireSphere(finderResult.Path.Last(), .4f);

                        Gizmos.color = Color.red;
                        var prev = finderResult.Path.First();
                        foreach (var point in finderResult.Path)
                        {
                            Gizmos.DrawWireSphere(point, .35f);
                            Gizmos.DrawLine(prev, point);
                            prev = point;
                        }

                        if (finderResult is JumpPointResult)
                        {
                            Gizmos.color = Color.yellow;
                            var jumpPointResult = finderResult as JumpPointResult;
                            foreach (var neighbor in jumpPointResult.Neighbors)
                            {
                                Gizmos.DrawWireSphere(neighbor, .3f);
                            }
                        }
                    }
                    
                    /*if (DisplayMapGizmo)
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
                    }*/
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