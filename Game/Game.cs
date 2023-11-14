using Godot;
using System;
using Dotris.Game;
using Dotris.Game.Inputs;

public partial class Game : Node2D
{
	public Tetris Tetris { get; } = new Tetris();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var board = GetNode<CanvasLayer>("GUI").GetNode<Board>("Board");
		board.SetTetris(Tetris);

		Tetris.Start();
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleAction("move_left", InputControls.Left, delta);
		HandleAction("move_right", InputControls.Right, delta);

		HandleAction("move_down", InputControls.SoftDrop, delta);
		HandleAction("move_up", InputControls.Up, delta);

		HandleAction("rotate_ccw", InputControls.RotateCounterclockwise, delta);
		HandleAction("rotate_cw", InputControls.RotateClockwise, delta);
	}

	private void HandleAction(string action, InputControls control, double delta)
	{
		if (Input.IsActionPressed(action))
			Tetris.InputEngine.Pressed(control, delta);
		if (Input.IsActionJustReleased(action))
			Tetris.InputEngine.Released(control);
	}
}
