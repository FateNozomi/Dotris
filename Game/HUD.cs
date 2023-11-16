using Godot;
using System;

public partial class HUD : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

	private void OnStartButtonPressed()
	{
		Hide();
		EmitSignal(SignalName.StartGame);
	}
}
