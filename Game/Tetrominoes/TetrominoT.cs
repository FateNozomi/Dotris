namespace Dotris.Game.Tetrominoes;

public class TetrominoT : Tetromino
{
    public TetrominoT()
    {
        TileSet = new Point[,]
        {
            { new (1, 0), new (0, 1), new (1, 1), new (2, 1) },
            { new (1, 0), new (1, 1), new (2, 1), new (1, 2) },
            { new (0, 1), new (1, 1), new (2, 1), new (1, 2) },
            { new (1, 0), new (0, 1), new (1, 1), new (1, 2) },
        };

        Shape = TetrominoShapes.T;

        X = 3;
        Y = 1;
    }
}