﻿using UnityEngine;

namespace Assets.Script.Finder
{
    public interface IFinder
    {
        BaseFinderResult Find(Vector3 start, Vector3 end);
    }
}