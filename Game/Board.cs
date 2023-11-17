using Godot;
using System;
using Dotris.Game;
using Dotris.Game.Tetrominoes;
using System.Linq;

public partial class Board : Control
{
	private PackedScene _lineParticleScene = GD.Load<PackedScene>("res://Game/Particles/LineParticles.tscn");
	private PackedScene _dropParticleScene = GD.Load<PackedScene>("res://Game/Particles/DropParticles.tscn");

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
		_tetris.TetrominoDropped += OnTetrominoDropped;
		_tetris.LineCleared += OnLineCleared;
	}

	private void OnTetrominoDropped(object sender, EventArgs e)
	{
		var tiles = _tetris.Tetromino.GetTiles();
		int width = tiles.Max(p => p.X) - tiles.Min(p => p.X) + 1;
		var dropPositionX = tiles.Min(p => p.X);
		var dropPositionY = tiles.Min(p => p.Y);

		var dropParticles = _dropParticleScene.Instantiate<GpuParticles2D>();
		var particleProcessMaterial = dropParticles.ProcessMaterial as ParticleProcessMaterial;
		particleProcessMaterial.EmissionBoxExtents = new Vector3(
			width * 16f,
			_tetris.Tetromino.HardDroppedCount * 16f,
			0);
		dropParticles.Position = new Vector2(
			dropPositionX * 32 + width * 32 / 2,
			dropPositionY * 32 - _tetris.Tetromino.HardDroppedCount * 32 / 2);
		dropParticles.Emitting = true;
		AddChild(dropParticles);
	}

	private void OnLineCleared(object sender, LineClearedEventArgs e)
	{
		var lineParticles = _lineParticleScene.Instantiate<GpuParticles2D>();
		lineParticles.Position = new Vector2(160, (e.LineIndex) * 32 + 16);
		lineParticles.Emitting = true;
		AddChild(lineParticles);
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
						14,
						_tileColor.Colors[tile]);
				}
			}
		}
	}

	private void DrawTetromino()
	{
		if (_tetris.Tetromino == null)
			return;

		foreach (var tile in _tetris.Tetromino.GetTiles())
		{
			DrawCircle(
				new Vector2(tile.X * 32 + 16, tile.Y * 32 + 16),
				14,
				_tileColor.Colors[(int)_tetris.Tetromino.Shape]);
		}
	}

	private void DrawGhostTetromino()
	{
		if (_tetris.Tetromino == null)
			return;

		int distance = _tetris.GetDropDistance(_tetris.Tetromino);
		var ghostColor = new Color(_tileColor.Colors[(int)_tetris.Tetromino.Shape], 0.3f);
		foreach (var tile in _tetris.Tetromino.GetTiles())
		{
			DrawCircle(
				new Vector2(tile.X * 32 + 16, (tile.Y + distance) * 32 + 16),
				14,
				ghostColor);
			DrawArc(
				new Vector2(tile.X * 32 + 16, (tile.Y + distance) * 32 + 16),
				15,
				0f,
				Mathf.Tau,
				20,
				Colors.White,
				2,
				true);
		}
	}
}
