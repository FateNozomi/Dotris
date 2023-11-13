namespace Dotris.Game.Tetrominoes;

public class TetrominoI : Tetromino
{
    public TetrominoI()
    {
        TileSet = new Point[,]
        {
            { new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(3, 1) },
            { new Point(2, 0), new Point(2, 1), new Point(2, 2), new Point(2, 3) },
            { new Point(0, 2), new Point(1, 2), new Point(2, 2), new Point(3, 2) },
            { new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(1, 3) },
        };

        Shape = TetrominoShapes.I;

        X = 3;
        Y = 1;
    }

    protected override Point[] GetWallKickData(int direction)
    {
        int setCount = WallKickData.WallKick_I.GetLength(0);

        // Wallkick data for states 0 -> 1 and 1 -> 0 are the same in magitude but negative
        // The dataset Index for ccw rotation is offset by -1
        int dataSetIndex = RotationState;
        if (direction < 0)
        {
            dataSetIndex = (RotationState - 1 + setCount) % setCount;
        }

        int dataCount = WallKickData.WallKick_I.GetLength(1);
        Point[] wallKickData = new Point[dataCount];
        for (int i = 0; i < dataCount; i++)
        {
            var wkd = WallKickData.WallKick_I[dataSetIndex, i];
            wallKickData[i] = new Point(wkd.X * direction, wkd.Y * direction);
        }

        return wallKickData;
    }
}