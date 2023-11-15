using Godot;
using System;
using Dotris.Game;
using System.Linq;

public partial class Next : Control
{
	private Tetris _tetris;
	private TileColor _tileColor = new TileColor();

	public override void _Draw()
	{
		DrawNextTetromino();
	}

	public void SetTetris(Tetris tetris)
	{
		_tetris = tetris;
		_tetris.DrawNext += (s, e) => QueueRedraw();
	}

	private void DrawNextTetromino()
	{
		var tetrominoes = _tetris.TetrominoBag.Tetrominoes.Take(5);

		int offsetX = -3;
		int offsetY = -1;

		int count = 0;
		foreach (var tetromino in tetrominoes)
		{
			foreach (var tile in tetromino.GetTiles())
			{
				DrawCircle(
					new Vector2((tile.X + offsetX) * 32 + 16, (tile.Y + offsetY + count * 3) * 32 + 16),
					15,
					_tileColor.Colors[(int)tetromino.Shape]);
			}

			count++;
		}
	}
}
