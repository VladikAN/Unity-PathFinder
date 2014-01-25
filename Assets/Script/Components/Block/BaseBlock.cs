using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Components.Block
{
    public abstract class BaseBlock : MonoBehaviour
    {
        public abstract IEnumerable<Vector3> GetPoints();
    }
}