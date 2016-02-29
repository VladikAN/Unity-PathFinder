using System.Collections.Generic;
using System.Linq;
using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Domain.Finder;
using PathFinder2D.Core.Extensions;
using PathFinder2D.Unity.Components;
using UnityEngine;

namespace Assets.Script.Components
{
    public class UnitComponent : MonoBehaviour
    {
        public GameObject TerrainObject;
        public float MoveTimeout;
        public float MoveSpeed;

        private TerrainComponent _map;

        private IList<Vector3> _path;
        private float _nextMoveTimeout;

        public void Start()
        {
            if (TerrainObject != null)
            {
                _map = TerrainObject.GetComponent<TerrainComponent>();
                _map.InitMap(Global.PathFinderService, 1);
            }
        }

        public void Update()
        {
            _path = _path ?? getRandomPath();
            if (_path != null)
            {
                _nextMoveTimeout -= Time.deltaTime;
                if (_nextMoveTimeout <= 0)
                {
                    var target = _path.First() + new Vector3(0, transform.position.y, 0);
                    if (Vector3.Distance(transform.position, target) < .1)
                    {
                        _path = _path.Except(new[] { _path.First() }).ToList();
                        _path = !_path.Any() ? null : _path;
                    }
                    else
                    {
                        var step = MoveSpeed * Time.deltaTime;
                        transform.position = Vector3.MoveTowards(transform.position, target, step);
                    }

                    _nextMoveTimeout = MoveTimeout;
                }
            }
        }

        private IList<Vector3> getRandomPath()
        {
            if (_map == null)
            {
                return null;
            }
            
            var targetX = Random.Range(0, _map.MapDefinition.FieldWidth);
            var targetY = Random.Range(0, _map.MapDefinition.FieldHeight);

            if (_map.MapDefinition.Field[targetX, targetY].Blocked)
            {
                return null;
            }

            var start = new WorldPosition(transform.position.x, transform.position.z);
            var end = _map.Terrain.ToWorld(new FinderPoint(targetX, targetY));
            var result = Global.PathFinderService.FindPath(_map.Terrain.Id(), start, end);

            return result.Path.Select(x => new Vector3(x.X, 0, x.Y)).ToList();
        }
    }
}