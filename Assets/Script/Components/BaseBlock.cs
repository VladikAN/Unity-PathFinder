using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Components
{
    public abstract class BaseBlock : MonoBehaviour
    {
        public abstract IEnumerable<Vector3> GetPoints();
    }
}