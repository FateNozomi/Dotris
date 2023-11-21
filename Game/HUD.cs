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
		var startOverButton = GetNode<VBoxContainer>("VBoxContainer").GetNode<Button>("StartOverButton");
		startOverButton.GrabFocus();
	}

    private void OnStartOverButtonPressed()
	{
		Hide();
		EmitSignal(SignalName.StartGame);
	}
}
