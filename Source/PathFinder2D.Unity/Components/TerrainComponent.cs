using System;
using System.Linq;
using PathFinder2D.Core;
using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Domain.Terrain;
using PathFinder2D.Core.Finder;
using PathFinder2D.Core.Initializer;
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

        private ITerrain _terrain;
        private MapDefinition _mapDefinition;

        public void Start()
        {
            var service = new PathFinderService(new WaveFinder(), new MapInitializer());

            _terrain = _terrain ?? new TerrainGameObject(gameObject, 1);
            _mapDefinition = service.InitMap(_terrain, 1);
        }

        public void Update()
        {
            
        }

        public void OnDrawGizmosSelected()
        {
            if (_mapDefinition == null)
            {
                return;
            }
            
            if (DisplayFreeCells || DisplayBlockCells)
            {
                var startX = _mapDefinition.Terrain.X();
                var startZ = _mapDefinition.Terrain.Y();
                var fieldWidth = _mapDefinition.FieldWidth;
                var fieldHeight = _mapDefinition.FieldHeight;

                var gizmoSize = new Vector3(_mapDefinition.CellSize, Math.Min(.1f, _mapDefinition.CellSize), _mapDefinition.CellSize) * 0.9f;
                var gizmoCorrection = new Vector3(_mapDefinition.CellSize / 2, gizmoSize.y / 2, _mapDefinition.CellSize / 2);

                for (var i = 0; i < fieldWidth; i++)
                {
                    var x = startX + _mapDefinition.CellSize * i;
                    for (var j = 0; j < fieldHeight; j++)
                    {
                        var blocked = _mapDefinition.Field[i, j] != null && _mapDefinition.Field[i, j].Blocked;
                        if ((!blocked && !DisplayFreeCells) || (blocked && !DisplayBlockCells))
                        {
                            continue;
                        }

                        var z = startZ + _mapDefinition.CellSize * j;

                        var gizmoPosition = new Vector3(x, 0, z) + gizmoCorrection;
                        Gizmos.color = blocked ? new Color(125, 0, 0) : new Color(0, 125, 0);
                        Gizmos.DrawWireCube(gizmoPosition, gizmoSize);
                    }
                }
            }

            if (DisplayPath)
            {
                var finderResult = _mapDefinition.LastFinderResult;
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