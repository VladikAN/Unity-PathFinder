using System;

namespace Assets.Script.Components.Debug.Utils
{
    [Flags]
    public enum DisplaySettings : byte
    {
        ShowFreeCell = 1,
        ShowBlockCell = 2,
    }
}