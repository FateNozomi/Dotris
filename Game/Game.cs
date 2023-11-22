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
	}

	public Tetris Tetris { get; private set; }

	public void Init(Tetris tetris)
	{
		Tetris = tetris;
		Tetris.TetrominoLocked += OnTetrominoLocked;
		Tetris.GameOver += OnGameOver;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var gui = GetNode<CanvasLayer>("GUI");
		var board = gui.GetNode<Board>("Board");
		var next = gui.GetNode<Next>("Next");
		var hold = gui.GetNode<Hold>("Hold");
		var hud = GetNode<HUD>("HUD");

		board.SetTetris(Tetris);
		next.SetTetris(Tetris);
		hold.SetTetris(Tetris);
		hud.SetTetris(Tetris);

		_boardPositionY = board.Position.Y;
		_lineLabel = gui.GetNode<Label>("LineLabel");
		_stopwatchLabel = gui.GetNode<Label>("StopwatchLabel");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Tetris.IsRunning)
		{
			HandleAction("Left", InputControls.Left, delta);
			HandleAction("Right", InputControls.Right, delta);

			HandleAction("SoftDrop", InputControls.SoftDrop, delta);
			HandleAction("HardDrop", InputControls.HardDrop, delta);

			HandleAction("RotateCounterclockwise", InputControls.RotateCounterclockwise, delta);
			HandleAction("RotateClockwise", InputControls.RotateClockwise, delta);

			HandleAction("Hold", InputControls.Hold, delta);

			Tetris.ProcessGravity(delta);

			_lineLabel.Text = Tetris.Lines.ToString();

			Tetris.ElapsedDelta += delta;
			_stopwatchLabel.Text = TimeSpan.FromSeconds(Tetris.ElapsedDelta).ToString(@"mm\:ss\.fff");
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Back"))
		{
			Tetris.Pause();
			ShowHUD();
		}
	}

	public void NewGame()
	{
		Tetris.Start();
	}

	public void ResumeGame()
	{
		Tetris.Resume();
	}

	private void ShowHUD()
	{
		var hud = GetNode<HUD>("HUD");
		hud.Show();
		hud.FocusDefault();
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
		ShowHUD();
	}

	private void HandleAction(string action, InputControls control, double delta)
	{
		if (Input.IsActionPressed(action))
			Tetris.InputEngine.Register(control, delta);
		if (Input.IsActionJustReleased(action))
			Tetris.InputEngine.Unregister(control);
	}
}
