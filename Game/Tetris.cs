using System;
using Dotris.Game.Inputs;
using Dotris.Game.Tetrominoes;

namespace Dotris.Game;

public class Tetris
{
    public Tetris(int rows = 23, int columns = 10)
    {
        Rows = rows;
        Columns = columns;
        Grid = new int[rows, columns];

        InputEngine.ARR = 2;
        InputEngine.DAS = 10;
        InputEngine.DCD = 1;
        InputEngine.LeftCommand.Executed += (s, e) => MoveLeft();
        InputEngine.RightCommand.Executed += (s, e) => MoveRight();
        InputEngine.SoftDropCommand.Executed += (s, e) => SoftDrop();
        InputEngine.UpCommand.Executed += (s, e) => MoveUp();
        InputEngine.RotateCounterclockwiseCommand.Executed += (s, e) => RotateCounterclockwise();
        InputEngine.RotateClockwiseCommand.Executed += (s, e) => RotateClockwise();
    }

    public event EventHandler Draw;

    public int Rows { get; }
    public int Columns { get; }
    public int[,] Grid { get; }

    public InputEngine InputEngine { get; } = new InputEngine();

    public TetrominoBag TetrominoBag { get; } = new TetrominoBag();

    public Tetromino Tetromino { get; private set; }

    public void Start()
    {
        SpawnTetromino();
    }

    public void SpawnTetromino()
    {
        Tetromino = TetrominoBag.GetTetromino();
    }

    private void MoveLeft()
    {
        Tetromino.MoveLeft();
        if (IsStateValid(Tetromino))
            Draw?.Invoke(this, EventArgs.Empty);
        else
            Tetromino.MoveRight();
    }

    private void MoveRight()
    {
        Tetromino.MoveRight();
        if (IsStateValid(Tetromino))
            Draw?.Invoke(this, EventArgs.Empty);
        else
            Tetromino.MoveLeft();
    }

    private void SoftDrop()
    {
        Tetromino.MoveDown();
        if (IsStateValid(Tetromino))
            Draw?.Invoke(this, EventArgs.Empty);
        else
            Tetromino.MoveUp();
    }

    private void MoveUp()
    {
        Tetromino.MoveUp();
        if (IsStateValid(Tetromino))
            Draw?.Invoke(this, EventArgs.Empty);
        else
            Tetromino.MoveDown();
    }

    private void RotateCounterclockwise()
    {
        Point[] wallKickData = Tetromino.GetCCW_WallKickData();
        Tetromino.RotateCounterclockwise();
        foreach (var wallKick in wallKickData)
        {
            Tetromino.X += wallKick.X;
            Tetromino.Y += -wallKick.Y;

            if (IsStateValid(Tetromino))
            {
                Draw?.Invoke(this, EventArgs.Empty);
                return;
            }

            Tetromino.X -= wallKick.X;
            Tetromino.Y -= -wallKick.Y;
        }

        Tetromino.RotateClockwise();
    }

    private void RotateClockwise()
    {
        Point[] wallKickData = Tetromino.GetCW_WallKickData();
        Tetromino.RotateClockwise();
        foreach (var wallKick in wallKickData)
        {
            Tetromino.X += wallKick.X;
            Tetromino.Y += -wallKick.Y;

            if (IsStateValid(Tetromino))
            {
                Draw?.Invoke(this, EventArgs.Empty);
                return;
            }

            Tetromino.X -= wallKick.X;
            Tetromino.Y -= -wallKick.Y;
        }

        Tetromino.RotateCounterclockwise();
    }

    private bool IsStateValid(Tetromino tetromino)
    {
        foreach (var tile in Tetromino.GetTiles())
        {
            var valid = tile.X >= 0 && tile.X < Columns && tile.Y >= 0 && tile.Y < Rows;
            if (!valid)
                return false;
        }

        return true;
    }
}