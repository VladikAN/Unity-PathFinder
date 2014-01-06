using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Components
{
    public class Block : BaseBlock
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