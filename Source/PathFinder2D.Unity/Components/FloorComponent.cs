using PathFinder2D.Core;
using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Domain.Terrain;
using PathFinder2D.Unity.Domain;
using System.Linq;
using UnityEngine;

namespace PathFinder2D.Unity.Components
{
    [AddComponentMenu("Modules/PathFinder2D/Floor")]
    public class FloorComponent : MonoBehaviour
    {
        public bool DisplayEmptyCells = false;
        public bool DisplayBlockedCells = false;
        public bool DisplayFullPath = false;
        
        public IFloor Floor;
        public MapDefinition MapDefinition;

        private readonly Vector3 _defaultSize = new Vector3(.5f, .5f, .5f);

        public void InitMap(IPathService pathFinderService, int cellSize)
        {
            Floor = Floor ?? new TerrainGameObject(gameObject, cellSize);
            MapDefinition = !pathFinderService.GetMaps().ContainsKey(Floor.Id())
                ? pathFinderService.InitMap(Floor, cellSize)
                : pathFinderService.GetMaps()[Floor.Id()];
        }

        public void OnDrawGizmosSelected()
        {
            if (MapDefinition == null) return;
            
            var gizmoSize = new Vector3(
                    Mathf.Min(_defaultSize.x, MapDefinition.CellSize),
                    Mathf.Min(_defaultSize.y, MapDefinition.CellSize),
                    Mathf.Min(_defaultSize.z, MapDefinition.CellSize)) * .8f;

            if (DisplayEmptyCells || DisplayBlockedCells)
            {
                var startX = MapDefinition.Terrain.X();
                var startZ = MapDefinition.Terrain.Y();
                var fieldWidth = MapDefinition.FieldWidth;
                var fieldHeight = MapDefinition.FieldHeight;

                var gizmoCorrection = new Vector3(MapDefinition.CellSize / 2, gizmoSize.y / 2, MapDefinition.CellSize / 2);

                for (var i = 0; i < fieldWidth; i++)
                {
                    var x = startX + MapDefinition.CellSize * i;
                    for (var j = 0; j < fieldHeight; j++)
                    {
                        var blocked = MapDefinition.Field[i, j] != null && MapDefinition.Field[i, j].Blocked;
                        if ((!blocked && !DisplayEmptyCells) || (blocked && !DisplayBlockedCells))
                        {
                            continue;
                        }

                        var z = startZ + MapDefinition.CellSize * j;

                        var gizmoPosition = new Vector3(x, 0, z) + gizmoCorrection;
                        Gizmos.color = blocked ? new Color(125, 0, 0, .15f) : new Color(0, 125, 0, .15f);
                        Gizmos.DrawCube(gizmoPosition, gizmoSize);
                    }
                }
            }

            if (DisplayFullPath)
            {
                var finderResult = MapDefinition.LastFinderResult;
                if (finderResult != null)
                {
                    if (DisplayFullPath && (finderResult.Path != null && finderResult.Path.Any()))
                    {
                        Gizmos.color = new Color(125, 0, 0, .25f);
                        var yCorrection = gizmoSize.y / 2;
                        foreach (var point in finderResult.Path)
                        {
                            Gizmos.DrawCube(new Vector3(point.X, yCorrection, point.Y), gizmoSize);
                        }

                        Gizmos.color = new Color(225, 0, 0, .5f);
                        var prev = finderResult.Path.First();
                        foreach (var point in finderResult.Path)
                        {
                            Gizmos.DrawLine(new Vector3(prev.X, yCorrection, prev.Y), new Vector3(point.X, yCorrection, point.Y));
                            prev = point;
                        }
                    }
                }
            }
        }
    }
}