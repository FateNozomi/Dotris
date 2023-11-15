using Godot;
using System;
using Dotris.Game;
using Dotris.Game.Tetrominoes;

public partial class Board : Control
{
	private Tetris _tetris;
	private TileColor _tileColor = new TileColor();

	public override void _Draw()
	{
		DrawTiles();
		DrawTetromino();
		DrawGhostTetromino();
	}

	public void SetTetris(Tetris tetris)
	{
		_tetris = tetris;
		_tetris.Draw += (s, e) => QueueRedraw();
	}

	private void DrawBorder()
	{
		DrawLine(new Vector2(-2, 96), new Vector2(-2, Size.Y), Colors.White, 4);
		DrawLine(new Vector2(0, Size.Y + 2), new Vector2(Size.X, Size.Y + 2), Colors.White, 4);
		DrawLine(new Vector2(Size.X + 2, Size.Y), new Vector2(Size.X + 2, 96), Colors.White, 4);
	}

	private void DrawTiles()
	{
		for (int y = 3; y < _tetris.Rows; y++)
		{
			for (int x = 0; x < _tetris.Columns; x++)
			{
				int tile = _tetris.Grid[y, x];
				if (tile == (int)TetrominoShapes.None)
				{
					DrawArc(
						new Vector2(x * 32 + 16, y * 32 + 16),
						14,
						0f,
						Mathf.Tau,
						20,
						_tileColor.Colors[tile],
						2,
						true);
				}
				else
				{
					DrawCircle(
						new Vector2(x * 32 + 16, y * 32 + 16),
						15,
						_tileColor.Colors[tile]);
				}
			}
		}
	}

	private void DrawTetromino()
	{
		foreach (var tile in _tetris.Tetromino.GetTiles())
		{
			DrawCircle(
				new Vector2(tile.X * 32 + 16, tile.Y * 32 + 16),
				15,
				_tileColor.Colors[(int)_tetris.Tetromino.Shape]);
		}
	}

	private void DrawGhostTetromino()
	{
		int distance = _tetris.GetDropDistance(_tetris.Tetromino);
		var ghostColor = new Color(_tileColor.Colors[(int)_tetris.Tetromino.Shape], 0.3f);
		foreach (var tile in _tetris.Tetromino.GetTiles())
		{
			DrawCircle(
				new Vector2(tile.X * 32 + 16, (tile.Y + distance) * 32 + 16),
				15,
				ghostColor);
		}
	}
}
