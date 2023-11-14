namespace Dotris.Game.Tetrominoes;

public static class WallKickData
{
    /// <summary>
    /// Wall Kick Data for J, L, S, T, Z
    /// Rotation states:
    /// 0 -> 1
    /// 1 -> 2
    /// 2 -> 3
    /// 3 -> 0
    /// </summary>
    public static readonly Point[,] WallKick = new Point[4, 5]
    {
        { new Point(0, 0), new Point(-1, 0), new Point(-1, 1), new Point(0, -2), new Point(-1, -2) }, // 0 -> 1
        { new Point(0, 0), new Point(1, 0), new Point(1, -1), new Point(0, 2), new Point(1, 2) }, // 1 -> 2
        { new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(0, -2), new Point(1, -2) }, // 2 -> 3
        { new Point(0, 0), new Point(-1, 0), new Point(-1, -1), new Point(0, 2), new Point(-1, 2) }, // 3 -> 0
    };

    /// <summary>
    /// Wall Kick Data for I
    /// Rotation states:
    /// 0 -> 1
    /// 1 -> 2
    /// 2 -> 3
    /// 3 -> 0
    /// </summary>
    public static readonly Point[,] WallKick_I = new Point[4, 5]
    {
        { new Point(0, 0), new Point(-2, 0), new Point(1, 0), new Point(-2, -1), new Point(1, 2) }, // 0 -> 1
        { new Point(0, 0), new Point(-1, 0), new Point(+2, 0), new Point(-1, 2), new Point(2, -1) }, // 1 -> 2
        { new Point(0, 0), new Point(2, 0), new Point(-1, 0), new Point(2, 1), new Point(-1, -2) }, // 2 -> 3
        { new Point(0, 0), new Point(1, 0), new Point(-2, 0), new Point(1, -2), new Point(-2, 1) }, // 3 -> 0
    };
}