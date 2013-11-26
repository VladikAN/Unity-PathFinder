using System;

namespace Assets.Script
{
    [Flags]
    public enum ExecutionSettings : byte
    {
        ShowCellGizmos = 1,

        Debug = ShowCellGizmos
    }
}