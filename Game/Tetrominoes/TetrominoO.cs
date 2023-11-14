namespace Dotris.Game.Tetrominoes;

public class TetrominoO : Tetromino
{
    public TetrominoO()
    {
        TileSet = new Point[,]
        {
            { new (0, 0), new (1, 0), new (0, 1), new (1, 1) },
        };

        Shape = TetrominoShapes.O;
        SetSpawnState();
    }

    public override void SetSpawnState()
    {
        RotationState = 0;
        X = 4;
        Y = 1;
    }
}