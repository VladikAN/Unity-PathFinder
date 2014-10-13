using UnityEngine;

namespace PathFinder2D.Unity.Components
{
    [AddComponentMenu("Modules/PathFinder2D/Terrain")]
    public class Terrain : MonoBehaviour
    {
//        public bool DisplayFreeCellsGizmo = false;
//        public bool DisplayBlockCellsGizmo = false;
//        public bool DisplayPathGizmo = false;
//
//        public void Start()
//        {
//        }
//
//        public void Update()
//        {
//            if (PathFinderService.TerrainField == null)
//            {
//                PathFinderService.Terrain = gameObject;
//                updateGrid();
//            }
//        }
//
//        public void OnDrawGizmosSelected()
//        {
//            if (PathFinderService.TerrainField == null)
//            {
//                return;
//            }
//
//            var startX = PathFinderService.TerrainGameObjectStartX;
//            var startZ = PathFinderService.TerrainGameObjectStartZ;
//
//            var fieldWidth = PathFinderService.TerrainFieldWidth;
//            var fieldHeight = PathFinderService.TerrainFieldHeight;
//            var gizmoSize = new Vector3(PathFinderService.CellWidth, Math.Min(.1f, PathFinderService.CellWidth), PathFinderService.CellWidth) * 0.9f;
//
//            if (DisplayFreeCellsGizmo || DisplayBlockCellsGizmo)
//            {
//                var labelCorrection = new Vector3(0.1f, 0, 0.35f);
//                var gizmoCorrection = new Vector3(PathFinderService.CellCorrection, gizmoSize.y / 2, PathFinderService.CellCorrection);
//                for (var i = 0; i < fieldWidth; i++)
//                {
//                    var x = startX + PathFinderService.CellWidth * i;
//                    for (var j = 0; j < fieldHeight; j++)
//                    {
//                        var blocked = PathFinderService.TerrainField[i, j] != null && PathFinderService.TerrainField[i, j].Blocked;
//                        if ((!blocked && !DisplayFreeCellsGizmo) || (blocked && !DisplayBlockCellsGizmo))
//                        {
//                            continue;
//                        }
//
//                        var z = startZ + PathFinderService.CellWidth * j;
//
//                        var labelPosition = new Vector3(x, 0, z) + labelCorrection;
//                        var gizmoPosition = new Vector3(x, 0, z) + gizmoCorrection;
//
//                        Gizmos.color = blocked ? new Color(125, 0, 0) : new Color(0, 125, 0);
//                        Gizmos.DrawWireCube(gizmoPosition, gizmoSize);
//                        Handles.Label(labelPosition, string.Format("{0} x {1}", i, j));
//                    }
//                }
//            }
//
//            if (DisplayPathGizmo)
//            {
//                var finderResult = PathFinderService.LastResult;
//                if (finderResult != null)
//                {
//                    if (DisplayPathGizmo && (finderResult.Path != null && finderResult.Path.Any()))
//                    {
//                        /* Draw start/end points */
//                        Gizmos.color = Color.blue;
//                        Gizmos.DrawWireSphere(finderResult.Path.First(), .4f);
//                        Gizmos.DrawWireSphere(finderResult.Path.Last(), .4f);
//
//                        /* Draw control points */
//                        Gizmos.color = Color.red;
//                        var prev = finderResult.Path.First();
//                        foreach (var point in finderResult.Path)
//                        {
//                            Gizmos.DrawWireSphere(point, .35f);
//                            Gizmos.DrawLine(prev, point);
//                            prev = point;
//                        }
//
//                        /* Draw algorithm gspecific ismos */
//                        var baseGizmo = PathFinderService.GetFinderGizmo(finderResult.GetType());
//                        if (baseGizmo != null)
//                        {
//                            baseGizmo.DisplayGizmo(finderResult);
//                        }
//                    }
//                }
//            }
//        }
//
//        private void updateGrid()
//        {
//            var blocks = (BaseBlock[])FindObjectsOfType(typeof(BaseBlock));
//            var points = blocks.SelectMany(x => x.GetPoints());
//
//            foreach (var point in points)
//            {
//                var x = (int)((point.x - PathFinderService.TerrainGameObjectStartX) / PathFinderService.CellWidth);
//                var z = (int)((point.z - PathFinderService.TerrainGameObjectStartZ) / PathFinderService.CellWidth);
//                
//                PathFinderService.TerrainField[x, z].Blocked = true;
//            }
//        }
    }
}