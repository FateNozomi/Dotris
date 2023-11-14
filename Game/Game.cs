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
		var gui = GetNode<CanvasLayer>("GUI");
		var board = gui.GetNode<Board>("Board");
		var next = gui.GetNode<Next>("Next");
		var hold = gui.GetNode<Hold>("Hold");

		board.SetTetris(Tetris);
		next.SetTetris(Tetris);
		hold.SetTetris(Tetris);

		Tetris.Start();
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleAction("move_left", InputControls.Left, delta);
		HandleAction("move_right", InputControls.Right, delta);

		HandleAction("move_down", InputControls.SoftDrop, delta);
		HandleAction("move_up", InputControls.HardDrop, delta);

		HandleAction("rotate_ccw", InputControls.RotateCounterclockwise, delta);
		HandleAction("rotate_cw", InputControls.RotateClockwise, delta);

		HandleAction("hold", InputControls.Hold, delta);
	}

	private void HandleAction(string action, InputControls control, double delta)
	{
		if (Input.IsActionPressed(action))
			Tetris.InputEngine.Pressed(control, delta);
		if (Input.IsActionJustReleased(action))
			Tetris.InputEngine.Released(control);
	}
}
