using System;

namespace Game.Dream
{
    /// <summary>
    /// Перечисление возможных режимов игры.
    /// </summary>
    [Flags]
    public enum DreamModeFlag : int
    {
        Blank = 1,
        RunAvoid = 2,
        FallingFloor = 4,
        RunCatch = 8,
        Idle = 16,
        Think = 32,
    }
}
