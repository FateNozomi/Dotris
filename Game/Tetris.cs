using System;
using Dotris.Game.Inputs;
using Dotris.Game.Tetrominoes;

namespace Dotris.Game;

public class Tetris
{
    private readonly int[] _gravityFrames = new int[] { 60, 50, 40, 30, 20, 10, 8, 6, 4, 2, 1, };
    private readonly double _gravityFrameSoftDropModifier = 1d / 30d;
    private readonly int _lockDelayFrame = 30;

    private double _gravityFrame;
    private double _gravityFrameModifier = 1;
    private double _gravityDelta;
    private double _lockDelayDelta;
    private bool _lockDelay;
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
        InputEngine.SoftDropCommand.JustReleased += (s, e) => ResetGravityFrameModifier();
        InputEngine.HardDropCommand.Executed += (s, e) => HardDrop();
        InputEngine.UpCommand.Executed += (s, e) => MoveUp();
        InputEngine.RotateCounterclockwiseCommand.Executed += (s, e) => RotateCounterclockwise();
        InputEngine.RotateClockwiseCommand.Executed += (s, e) => RotateClockwise();
        InputEngine.HoldCommand.Executed += (s, e) => Hold();
    }

    public event EventHandler Draw;
    public event EventHandler DrawNext;
    public event EventHandler DrawHold;
    public event EventHandler TetrominoDropped;
    public event EventHandler TetrominoLocked;
    public event EventHandler<LineClearedEventArgs> LineCleared;
    public event EventHandler GameOver;

    public int Rows { get; }
    public int Columns { get; }
    public int[,] Grid { get; }

    public InputEngine InputEngine { get; } = new InputEngine();

    public TetrominoBag TetrominoBag { get; } = new TetrominoBag();

    public Tetromino Tetromino { get; private set; }
    public Tetromino HoldTetromino { get; private set; }

    public int Lines { get; private set; }
    public double ElapsedDelta { get; set; }

    public bool IsRunning { get; private set; }

    public void ProcessGravity(double delta)
    {
        _gravityDelta += delta;
        if (_gravityDelta > _gravityFrame * 1d / 60d * _gravityFrameModifier)
        {
            MoveDown();
            _gravityDelta = 0;
        }

        _lockDelay = !CanMoveDown();
        if (_lockDelay)
        {
            _lockDelayDelta += delta;
            if (_lockDelayDelta > _lockDelayFrame * 1d / 60d)
            {
                LockTetromino();
                _lockDelayDelta = 0;
                _lockDelay = false;
            }
        }
    }

    public void Start()
    {
        Lines = 0;
        ElapsedDelta = 0;

        Tetromino = null;
        HoldTetromino = null;
        TetrominoBag.Tetrominoes.Clear();
        ClearGrid();

        ResetGravityFrameModifier();
        ResetGravityDelta();
        ResetLockDelayDelta();

        IsRunning = true;
        SpawnTetromino();
    }

    public void Pause() => IsRunning = false;

    public void SpawnTetromino()
    {
        _held = false;

        if (IsGameOver())
        {
            IsRunning = false;
            GameOver?.Invoke(this, EventArgs.Empty);
            return;
        }

        Tetromino = TetrominoBag.GetTetromino();

        _gravityFrame = GetGravity(Lines);
        ResetGravityDelta();

        Draw?.Invoke(this, EventArgs.Empty);
        DrawNext?.Invoke(this, EventArgs.Empty);
        DrawHold?.Invoke(this, EventArgs.Empty);
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

    private void LockTetromino()
    {
        foreach (var tile in Tetromino.GetTiles())
        {
            Grid[tile.Y, tile.X] = (int)Tetromino.Shape;
        }

        Lines += LineClear();
        TetrominoLocked?.Invoke(this, EventArgs.Empty);
        SpawnTetromino();
    }

    private double GetGravity(int lines)
    {
        int index = Math.Min(lines / 10, _gravityFrames.Length - 1);
        return _gravityFrames[index];
    }

    private int LineClear()
    {
        int cleared = 0;
        for (int row = Rows - 1; row >= 0; row--)
        {
            if (IsLine(row))
            {
                ClearLine(row);
                cleared++;
                LineCleared?.Invoke(this, new LineClearedEventArgs(row));
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

    private void MoveLeft()
    {
        Tetromino.MoveLeft();
        if (IsStateValid(Tetromino))
        {
            ResetLockDelayDelta();
            Draw?.Invoke(this, EventArgs.Empty);
        }
        else
            Tetromino.MoveRight();
    }

    private void MoveRight()
    {
        Tetromino.MoveRight();
        if (IsStateValid(Tetromino))
        {
            ResetLockDelayDelta();
            Draw?.Invoke(this, EventArgs.Empty);
        }
        else
            Tetromino.MoveLeft();
    }

    private void MoveDown()
    {
        Tetromino.MoveDown();
        if (IsStateValid(Tetromino))
        {
            Draw?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Tetromino.MoveUp();
        }
    }

    private bool CanMoveDown() => GetDropDistance(Tetromino) != 0;

    private void SoftDrop() => _gravityFrameModifier = _gravityFrameSoftDropModifier;

    private void ResetGravityFrameModifier() => _gravityFrameModifier = 1;

    private void HardDrop()
    {
        int distance = GetDropDistance(Tetromino);
        if (distance > 0)
        {
            Tetromino.Y += distance;
            Tetromino.HardDroppedCount = distance;
            Draw?.Invoke(this, EventArgs.Empty);
            TetrominoDropped?.Invoke(this, EventArgs.Empty);
        }

        if (IsStateValid(Tetromino))
        {
            LockTetromino();
        }
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
                ResetLockDelayDelta();
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
                ResetLockDelayDelta();
                Draw?.Invoke(this, EventArgs.Empty);
                return;
            }

            Tetromino.X -= wallKick.X;
            Tetromino.Y -= -wallKick.Y;
        }

        Tetromino.RotateCounterclockwise();
    }

    private void ResetGravityDelta() => _gravityDelta = 0;

    private void ResetLockDelayDelta() => _lockDelayDelta = 0;

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

    private void ClearGrid()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                Grid[row, column] = (int)TetrominoShapes.None;
            }
        }
    }

    private bool IsGameOver()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                if (Grid[row, column] != (int)TetrominoShapes.None)
                {
                    return true;
                }
            }
        }

        return false;
    }
}