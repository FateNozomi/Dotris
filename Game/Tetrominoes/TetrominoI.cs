namespace Dotris.Game.Tetrominoes;

public class TetrominoI : Tetromino
{
    public TetrominoI()
    {
        WallKickDataSet = WallKickData.WallKick_I;
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
}