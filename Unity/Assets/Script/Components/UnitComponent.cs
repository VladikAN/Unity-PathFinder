﻿using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Finder;
using PathFinder2D.Unity.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Script.Components
{
    public class UnitComponent : MonoBehaviour
    {
        public GameObject FloorObject;
        public float MoveTimeout;
        public float MoveSpeed;

        private FloorComponent _floor;

        private IList<Vector3> _path;
        private Vector3 _nextPoint;
        private float _nextMoveTimeout;

        public void Start()
        {
            if (FloorObject != null)
            {
                _floor = FloorObject.GetComponent<FloorComponent>();
                _floor.InitMap(Global.PathService, 1);
            }
        }

        public void Update()
        {
            if (_path == null || !_path.Any())
            {
                _path = GetRandomPath();
                if (_path == null || !_path.Any()) return;
                _nextPoint = _path.First() + new Vector3(0, transform.position.y, 0);
            }

            _nextMoveTimeout -= Time.deltaTime;
            if (_nextMoveTimeout > 0) return;

            if (Vector3.Distance(transform.position, _nextPoint) < .1)
            {
                _path = _path.Skip(1).ToList();
                _nextPoint = _path.FirstOrDefault();
                if (_nextPoint != null)
                {
                    _nextPoint += new Vector3(0, transform.position.y, 0);
                }
            }
            else
            {
                var step = MoveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _nextPoint, step);
            }

            _nextMoveTimeout = MoveTimeout;
        }

        private IList<Vector3> GetRandomPath()
        {
            if (_floor == null) return null;
            
            var coins = FindObjectsOfType<CoinComponent>()
                .Where(x => x.GetComponent<MeshRenderer>().enabled)
                .ToList();

            if (!coins.Any()) return null;

            var index = Random.Range(0, coins.Count);

            var start = new WorldPosition(transform.position.x, transform.position.z);
            var end = new WorldPosition(coins[index].transform.position.x, coins[index].transform.position.z);

            var result = Global.PathService.FindPath(_floor.Floor.Id(), start, end, SearchOptions.Minimum);
            return result.Path.Select(x => new Vector3(x.X, 0, x.Y)).ToList();
        }
    }
}