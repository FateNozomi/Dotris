using Godot;
using System;
using Dotris.Game;

public partial class Hold : Control
{
	private Tetris _tetris;
	private TileColor _tileColor = new TileColor();

	public override void _Draw()
	{
		DrawHoldTetromino();
	}

	public void SetTetris(Tetris tetris)
	{
		_tetris = tetris;
		_tetris.DrawHold += (s, e) => QueueRedraw();
	}

	private void DrawHoldTetromino()
	{
		if (_tetris.HoldTetromino == null)
			return;

		int offsetX = -3;
		int offsetY = -1;

		foreach (var tile in _tetris.HoldTetromino.GetTiles())
		{
			DrawCircle(
				new Vector2((tile.X + offsetX) * 32 + 16, (tile.Y + offsetY) * 32 + 16),
				15,
				_tileColor.Colors[(int)_tetris.HoldTetromino.Shape]);
		}
	}
}
