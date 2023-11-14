namespace Dotris.Game.Tetrominoes;

public abstract class Tetromino
{
    public Tetromino()
    {
        WallKickDataSet = WallKickData.WallKick;
    }

    /// <summary>
    /// 0 = spawn state
    /// 1 = state resulting from a clockwise rotation from spawn
    /// 2 = state resulting from 2 successive rotation in either direction from spawn
    /// 3 = state resulting from a counterclockwise rotation from spawn
    /// </summary>
    protected int RotationState { get; set; }
    protected Point[,] WallKickDataSet { get; set; }
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

    private Point[] GetWallKickData(int direction)
    {
        int setCount = WallKickDataSet.GetLength(0);

        // Wall kick data for state 1 -> 0 is the negative of state 0 -> 1
        // The wall kick data set index for ccw rotation is offset by -1
        int dataSetIndex = RotationState;
        if (direction < 0)
        {
            dataSetIndex = (RotationState - 1 + setCount) % setCount;
        }

        int dataCount = WallKickDataSet.GetLength(1);
        Point[] wallKickData = new Point[dataCount];
        for (int i = 0; i < dataCount; i++)
        {
            var wkd = WallKickDataSet[dataSetIndex, i];
            wallKickData[i] = new Point(wkd.X * direction, wkd.Y * direction);
        }

        return wallKickData;
    }
}