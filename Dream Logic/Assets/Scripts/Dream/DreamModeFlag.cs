using System;

namespace Game.Dream
{
    /// <summary>
    /// ������������ ��������� ������� ����.
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
