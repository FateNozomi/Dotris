using Godot;
using System;

public partial class Main : Node
{
	public Main()
	{
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("GUI/VBoxContainer/StartButton").GrabFocus();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnStartButtonPressed()
	{
		var game = ResourceLoader.Load<PackedScene>("res://Game/Game.tscn").Instantiate<Game>();
		GetTree().Root.AddChild(game);
		QueueFree();

		var startOverButton = game.GetNode<Button>("HUD/VBoxContainer/StartOverButton");
		startOverButton.EmitSignal("pressed");
	}

	private void OnConfigButtonPressed()
	{
		var config = ResourceLoader.Load<PackedScene>("res://Configuration/Config.tscn").Instantiate<Config>();
		GetTree().Root.AddChild(config);
		QueueFree();
	}
}
