using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.PathFinder.Finder2D.Components.Block
{
    public class SimpleBlock : BaseBlock
	{
        public void Start()
        {
        }

        public void Update()
        {
        }

        public override IEnumerable<Vector3> GetPoints()
        {
            return new[] { transform.position };
        }
	}
}