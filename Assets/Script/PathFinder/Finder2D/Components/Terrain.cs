using System.Linq;
using Assets.Script.PathFinder.Finder2D.Components.Block;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.PathFinder.Finder2D.Components
{
    [AddComponentMenu("2D Path Finder/Terrain")]
    public class Terrain : MonoBehaviour
    {
        public bool DisplayFreeCellsGizmo = false;
        public bool DisplayBlockCellsGizmo = false;
        public bool DisplayPathGizmo = false;

        public void Start()
        {
        }

        public void Update()
        {
            if (PathFinderGlobal.TerrainField == null)
            {
                PathFinderGlobal.Terrain = gameObject;
                updateGrid();
            }
        }

        public void OnDrawGizmosSelected()
        {
            if (PathFinderGlobal.TerrainField == null)
            {
                return;
            }

            var startX = PathFinderGlobal.TerrainGameObjectStartX;
            var startZ = PathFinderGlobal.TerrainGameObjectStartZ;

            var fieldWidth = PathFinderGlobal.TerrainFieldWidth;
            var fieldHeight = PathFinderGlobal.TerrainFieldHeight;
            var gizmoSize = new Vector3(PathFinderGlobal.CellWidth, 1, PathFinderGlobal.CellWidth) * 0.9f;

            if (DisplayFreeCellsGizmo || DisplayBlockCellsGizmo)
            {
                var labelCorrection = new Vector3(0.1f, 0, 0.35f);
                var gizmoCorrection = new Vector3(PathFinderGlobal.CellCorrection, 0.5f, PathFinderGlobal.CellCorrection);
                for (var i = 0; i < fieldWidth; i++)
                {
                    var x = startX + PathFinderGlobal.CellWidth * i;
                    for (var j = 0; j < fieldHeight; j++)
                    {
                        var blocked = PathFinderGlobal.TerrainField[i, j] != null && PathFinderGlobal.TerrainField[i, j].Blocked;
                        if ((!blocked && !DisplayFreeCellsGizmo) || (blocked && !DisplayBlockCellsGizmo))
                        {
                            continue;
                        }

                        var z = startZ + PathFinderGlobal.CellWidth * j;

                        var labelPosition = new Vector3(x, 0, z) + labelCorrection;
                        var gizmoPosition = new Vector3(x, 0, z) + gizmoCorrection;

                        Gizmos.color = blocked ? new Color(125, 0, 0) : new Color(0, 125, 0);
                        Gizmos.DrawWireCube(gizmoPosition, gizmoSize);
                        Handles.Label(labelPosition, string.Format("{0} x {1}", i, j));
                    }
                }
            }

            if (DisplayPathGizmo)
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

                        var baseGizmo = PathFinderGlobal.GetFinderGizmo(finderResult.GetType());
                        if (baseGizmo != null)
                        {
                            baseGizmo.DisplayGizmo(finderResult);
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
                var x = (int)((point.x - PathFinderGlobal.TerrainGameObjectStartX) / PathFinderGlobal.CellWidth);
                var z = (int)((point.z - PathFinderGlobal.TerrainGameObjectStartZ) / PathFinderGlobal.CellWidth);
                
                PathFinderGlobal.TerrainField[x, z].Blocked = true;
            }
        }
    }
}