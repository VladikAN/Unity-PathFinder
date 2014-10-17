using System.Collections.Generic;
using System.Linq;
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

        private IEnumerable<Vector3> _path;
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
                    if (Vector3.Distance(transform.position, _path.First()) < .1)
                    {
                        _path = _path.Except(new[] { _path.First() });
                        _path = !_path.Any() ? null : _path;
                    }
                    else
                    {
                        var step = MoveSpeed * Time.deltaTime;
                        transform.position = Vector3.MoveTowards(transform.position, _path.First(), step);
                    }

                    _nextMoveTimeout = MoveTimeout;
                }
            }
        }

        private IEnumerable<Vector3> getRandomPath()
        {
            if (_map == null)
            {
                return null;
            }
            
            var endX = Random.Range(0, _map.MapDefinition.FieldWidth);
            var endY = Random.Range(0, _map.MapDefinition.FieldHeight);

            if (_map.MapDefinition.Field[endX, endY].Blocked)
            {
                return null;
            }

            var start = transform.position;
            var end = _map.Terrain.ToVector3(new FinderPoint(endX, endY));
            var result = Global.PathFinderService.Find(_map.Terrain.Id(), start, end);

            return result.Path;
        }
    }
}