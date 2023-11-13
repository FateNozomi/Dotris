namespace Dotris.Game.Tetrominoes;

public abstract class Tetromino
{
    /// <summary>
    /// 0 = spawn state
    /// 1 = state resulting from a clockwise rotation from spawn
    /// 2 = state resulting from 2 successive rotation in either direction from spawn
    /// 3 = state resulting from a counterclockwise rotation from spawn
    /// </summary>
    protected int RotationState { get; set; }

    protected Point[,] TileSet { get; set; }
    public TetrominoShapes Shape { get; protected set; }
    public int X { get; set; }
    public int Y { get; set; }

    public uint SoftDroppedCount { get; set; }
    public uint HardDroppedCount { get; set; }

    public Point[] GetTiles()
    {
        int tileCount = TileSet.GetLength(1);
        Point[] tiles = new Point[tileCount];
        for (int i = 0; i < tileCount; i++)
        {
            Point p = TileSet[RotationState, i];
            tiles[i] = new Point(p.X + X, p.Y + Y);
        }

        return tiles;
    }

    public void MoveLeft()
    {
        X -= 1;
    }

    public void MoveRight()
    {
        X += 1;
    }

    public void MoveUp()
    {
        Y -= 1;
    }

    public void MoveDown()
    {
        Y += 1;
    }

    public void RotateCounterclockwise()
    {
        int setCount = TileSet.GetLength(0);
        RotationState = (RotationState - 1 + setCount) % setCount;
    }

    public void RotateClockwise()
    {
        int setCount = TileSet.GetLength(0);
        RotationState = (RotationState + 1) % setCount;
    }

    public Point[] GetCCW_WallKickData() => GetWallKickData(-1);

    public Point[] GetCW_WallKickData() => GetWallKickData(1);

    protected virtual Point[] GetWallKickData(int direction)
    {
        int setCount = WallKickData.WallKick_I.GetLength(0);

        // Wallkick data for states 0 -> 1 and 1 -> 0 are the same in magitude but negative
        // The dataset Index for ccw rotation is offset by -1
        int dataSetIndex = RotationState;
        if (direction < 0)
        {
            dataSetIndex = (RotationState - 1 + setCount) % setCount;
        }

        int dataCount = WallKickData.WallKick.GetLength(1);
        Point[] wallKickData = new Point[dataCount];
        for (int i = 0; i < dataCount; i++)
        {
            var wkd = WallKickData.WallKick[dataSetIndex, i];
            wallKickData[i] = new Point(wkd.X * direction, wkd.Y * direction);
        }

        return wallKickData;
    }
}