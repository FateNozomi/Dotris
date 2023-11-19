using Godot;
using System;
using Dotris.Game;
using Dotris.Game.Inputs;

public partial class Game : Node2D
{
	private float _boardPositionY;
	private Label _lineLabel;
	private Label _stopwatchLabel;

	public Game()
	{
		Tetris.TetrominoLocked += OnTetrominoLocked;
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

		_boardPositionY = board.Position.Y;
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

	private void OnTetrominoLocked(object sender, EventArgs e)
	{
		var board = GetNode<CanvasLayer>("GUI").GetNode<Board>("Board");
		Tween tween = CreateTween();
		tween.TweenProperty(board, "position:y", _boardPositionY + 6, 0.03).SetTrans(Tween.TransitionType.Expo);
		tween.TweenProperty(board, "position:y", _boardPositionY, 0.1);
	}

	private void OnGameOver(object sender, EventArgs e)
	{
		var hud = GetNode<HUD>("HUD");
		hud.Show();
		hud.FocusDefault();
	}

	private void HandleAction(string action, InputControls control, double delta)
	{
		if (Input.IsActionPressed(action))
			Tetris.InputEngine.Register(control, delta);
		if (Input.IsActionJustReleased(action))
			Tetris.InputEngine.Unregister(control);
	}
}
