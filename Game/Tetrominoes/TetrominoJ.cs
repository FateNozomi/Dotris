namespace Dotris.Game.Tetrominoes;

public class TetrominoJ : Tetromino
{
    public TetrominoJ()
    {
        TileSet = new Point[,]
        {
            { new (0, 0), new (0, 1), new (1, 1), new (2, 1) },
            { new (1, 0), new (2, 0), new (1, 1), new (1, 2) },
            { new (0, 1), new (1, 1), new (2, 1), new (2, 2) },
            { new (1, 0), new (1, 1), new (0, 2), new (1, 2) },
        };

        Shape = TetrominoShapes.J;

        X = 3;
        Y = 1;
    }
}