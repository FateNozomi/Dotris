using Godot;
using System;
using Dotris.Game;
using Dotris.Game.Inputs;

public partial class Game : Node2D
{
	private Label _lineLabel;
	private Label _stopwatchLabel;

	public Game()
	{
		Tetris.GameOver += OnGameOver;
	}

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

		_lineLabel = gui.GetNode<Label>("LineLabel");
		_stopwatchLabel = gui.GetNode<Label>("StopwatchLabel");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Tetris.IsRunning)
		{
			HandleAction("move_left", InputControls.Left, delta);
			HandleAction("move_right", InputControls.Right, delta);

			HandleAction("move_down", InputControls.SoftDrop, delta);
			HandleAction("move_up", InputControls.HardDrop, delta);

			HandleAction("rotate_ccw", InputControls.RotateCounterclockwise, delta);
			HandleAction("rotate_cw", InputControls.RotateClockwise, delta);

			HandleAction("hold", InputControls.Hold, delta);

			Tetris.ProcessGravity(delta);

			_lineLabel.Text = Tetris.Lines.ToString();

			Tetris.ElapsedDelta += delta;
			_stopwatchLabel.Text = TimeSpan.FromSeconds(Tetris.ElapsedDelta).ToString(@"mm\:ss\.fff");
		}
	}

	public void NewGame()
	{
		Tetris.Start();
	}

	private void OnGameOver(object sender, EventArgs e)
	{
		var hud = GetNode<CanvasLayer>("HUD");
		hud.Show();
	}

	private void HandleAction(string action, InputControls control, double delta)
	{
		if (Input.IsActionPressed(action))
			Tetris.InputEngine.Register(control, delta);
		if (Input.IsActionJustReleased(action))
			Tetris.InputEngine.Unregister(control);
	}
}
