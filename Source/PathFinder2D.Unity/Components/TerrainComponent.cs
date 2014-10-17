using System;
using System.Linq;
using PathFinder2D.Core;
using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Domain.Terrain;
using PathFinder2D.Unity.Domain;
using UnityEngine;

namespace PathFinder2D.Unity.Components
{
    [AddComponentMenu("Modules/PathFinder2D/Terrain")]
    public class TerrainComponent : MonoBehaviour
    {
        public bool DisplayFreeCells = false;
        public bool DisplayBlockCells = false;
        public bool DisplayPath = false;
        
        public ITerrain Terrain;
        public MapDefinition MapDefinition;

        public void InitMap(IPathFinderService pathFinderService, int cellSize)
        {
            Terrain = Terrain ?? new TerrainGameObject(gameObject, cellSize);
            MapDefinition = !pathFinderService.RegisteredMaps().ContainsKey(Terrain.Id())
                ? pathFinderService.InitMap(Terrain, cellSize)
                : pathFinderService.RegisteredMaps()[Terrain.Id()];
        }

        public void OnDrawGizmosSelected()
        {
            if (MapDefinition == null)
            {
                return;
            }
            
            if (DisplayFreeCells || DisplayBlockCells)
            {
                var startX = MapDefinition.Terrain.X();
                var startZ = MapDefinition.Terrain.Y();
                var fieldWidth = MapDefinition.FieldWidth;
                var fieldHeight = MapDefinition.FieldHeight;

                var gizmoSize = new Vector3(MapDefinition.CellSize, Math.Min(.1f, MapDefinition.CellSize), MapDefinition.CellSize) * 0.9f;
                var gizmoCorrection = new Vector3(MapDefinition.CellSize / 2, gizmoSize.y / 2, MapDefinition.CellSize / 2);

                for (var i = 0; i < fieldWidth; i++)
                {
                    var x = startX + MapDefinition.CellSize * i;
                    for (var j = 0; j < fieldHeight; j++)
                    {
                        var blocked = MapDefinition.Field[i, j] != null && MapDefinition.Field[i, j].Blocked;
                        if ((!blocked && !DisplayFreeCells) || (blocked && !DisplayBlockCells))
                        {
                            continue;
                        }

                        var z = startZ + MapDefinition.CellSize * j;

                        var gizmoPosition = new Vector3(x, 0, z) + gizmoCorrection;
                        Gizmos.color = blocked ? new Color(125, 0, 0) : new Color(0, 125, 0);
                        Gizmos.DrawWireCube(gizmoPosition, gizmoSize);
                    }
                }
            }

            if (DisplayPath)
            {
                var finderResult = MapDefinition.LastFinderResult;
                if (finderResult != null)
                {
                    if (DisplayPath && (finderResult.Path != null && finderResult.Path.Any()))
                    {
                        /* Draw start/end points */
                        Gizmos.color = Color.blue;
                        Gizmos.DrawWireSphere(finderResult.Path.First(), .4f);
                        Gizmos.DrawWireSphere(finderResult.Path.Last(), .4f);

                        /* Draw control points */
                        Gizmos.color = Color.red;
                        var prev = finderResult.Path.First();
                        foreach (var point in finderResult.Path)
                        {
                            Gizmos.DrawWireSphere(point, .35f);
                            Gizmos.DrawLine(prev, point);
                            prev = point;
                        }
                    }
                }
            }
        }
    }
}