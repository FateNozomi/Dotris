using Godot;
using System;

public partial class HUD : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

    public override void _Ready()
    {
		FocusDefault();
    }

	public void FocusDefault()
	{
		var startButton = GetNode<VBoxContainer>("VBoxContainer").GetNode<Button>("StartButton");
		startButton.GrabFocus();
	}

    private void OnStartButtonPressed()
	{
		Hide();
		EmitSignal(SignalName.StartGame);
	}
}
