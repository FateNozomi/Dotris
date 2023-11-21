using Godot;
using System;
using Dotris.Game;

public partial class HUD : CanvasLayer
{
	private Tetris _tetris;

	[Signal]
	public delegate void ResumeEventHandler();

	[Signal]
	public delegate void StartGameEventHandler();

	public void SetTetris(Tetris tetris)
	{
		_tetris = tetris;
	}

	public override void _Ready()
	{
		FocusDefault();
	}

	public void FocusDefault()
	{
		var startOverButton = GetNode<Button>("VBoxContainer/StartOverButton");
		startOverButton.GrabFocus();

		var resumeButton = GetNode<Button>("VBoxContainer/ResumeButton");
		resumeButton.Visible = !_tetris?.IsGameOver ?? false;
		if (resumeButton.Visible)
			resumeButton.GrabFocus();
	}

	private void OnResumeButtonPressed()
	{
		Hide();
		EmitSignal(SignalName.Resume);
	}

	private void OnStartOverButtonPressed()
	{
		Hide();
		EmitSignal(SignalName.StartGame);
	}

	private void OnMainMenuButtonPressed()
	{
		var main = ResourceLoader.Load<PackedScene>("res://Main/Main.tscn").Instantiate<Main>();
		GetTree().Root.AddChild(main);
		GetParent().QueueFree();
	}
}
