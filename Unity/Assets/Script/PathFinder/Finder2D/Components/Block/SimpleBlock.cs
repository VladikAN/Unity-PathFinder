using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.PathFinder.Finder2D.Components.Block
{
    [AddComponentMenu("2D Path Finder/Block/Simple Block")]
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