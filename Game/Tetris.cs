using System;
using Dotris.Game.Inputs;
using Dotris.Game.Tetrominoes;

namespace Dotris.Game;

public class Tetris
{
    private bool _held;

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
        InputEngine.HardDropCommand.Executed += (s, e) => HardDrop();
        InputEngine.UpCommand.Executed += (s, e) => MoveUp();
        InputEngine.RotateCounterclockwiseCommand.Executed += (s, e) => RotateCounterclockwise();
        InputEngine.RotateClockwiseCommand.Executed += (s, e) => RotateClockwise();
        InputEngine.HoldCommand.Executed += (s, e) => Hold();
    }

    public event EventHandler Draw;
    public event EventHandler DrawNext;
    public event EventHandler DrawHold;

    public int Rows { get; }
    public int Columns { get; }
    public int[,] Grid { get; }

    public InputEngine InputEngine { get; } = new InputEngine();

    public TetrominoBag TetrominoBag { get; } = new TetrominoBag();

    public Tetromino Tetromino { get; private set; }
    public Tetromino HoldTetromino { get; private set; }

    public void Start()
    {
        SpawnTetromino();
    }

    public void SpawnTetromino()
    {
        _held = false;
        Tetromino = TetrominoBag.GetTetromino();
        DrawNext?.Invoke(this, EventArgs.Empty);
    }

    public void LockTetromino()
    {
        foreach (var tile in Tetromino.GetTiles())
        {
            Grid[tile.Y, tile.X] = (int)Tetromino.Shape;
        }

        LineClear();
        SpawnTetromino();
        Draw?.Invoke(this, EventArgs.Empty);
    }

    public int LineClear()
    {
        int cleared = 0;
        for (int row = Rows - 1; row >= 0; row--)
        {
            if (IsLine(row))
            {
                ClearLine(row);
                cleared++;
            }
            else if (cleared > 0)
            {
                MoveLineDown(row, cleared);
            }
        }

        return cleared;

        bool IsLine(int row)
        {
            for (int column = 0; column < Columns; column++)
            {
                if (Grid[row, column] == 0)
                    return false;
            }

            return true;
        }

        void ClearLine(int row)
        {
            for (int column = 0; column < Columns; column++)
                Grid[row, column] = 0;
        }

        void MoveLineDown(int row, int count)
        {
            for (int column = 0; column < Columns; column++)
            {
                Grid[row + count, column] = Grid[row, column];
            }
        }
    }

    public int GetDropDistance(Tetromino tetromino)
    {
        int distance = Rows;
        foreach (Point tile in tetromino.GetTiles())
        {
            distance = Math.Min(distance, TileDropDistance(tile));
        }

        return distance;

        int TileDropDistance(Point tile)
        {
            int distance = 0;
            while (true)
            {
                int row = tile.Y + 1 + distance;
                if (row >= Rows)
                    break;

                if (Grid[row, tile.X] == 0)
                    distance++;
                else
                    break;
            }

            return distance;
        }
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

    private void HardDrop()
    {
        int distance = GetDropDistance(Tetromino);
        if (distance > 0)
        {
            Tetromino.Y += distance;
            Tetromino.HardDroppedCount = distance;
            Draw?.Invoke(this, EventArgs.Empty);
        }

        LockTetromino();
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

    private void Hold()
    {
        if (_held)
            return;

        Tetromino.SetSpawnState();

        if (HoldTetromino == null)
        {
            HoldTetromino = Tetromino;
            SpawnTetromino();
        }
        else
        {
            var held = Tetromino;
            Tetromino = HoldTetromino;
            HoldTetromino = held;
        }

        _held = true;
        Draw?.Invoke(this, EventArgs.Empty);
        DrawHold?.Invoke(this, EventArgs.Empty);
    }

    private bool IsStateValid(Tetromino tetromino)
    {
        foreach (var tile in Tetromino.GetTiles())
        {
            var withinBound = tile.X >= 0 && tile.X < Columns && tile.Y >= 0 && tile.Y < Rows;
            if (!withinBound)
                return false;

            if (Grid[tile.Y, tile.X] != 0)
                return false;
        }

        return true;
    }
}