using Godot;
using System;

public partial class Config : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void FocusDefault()
	{
		var backButton = GetNode<Button>("BackButton");
		backButton.GrabFocus();
	}

	private void OnBackButtonPressed()
	{
		Hide();
		var hud = GetParent().GetNode<HUD>("HUD");
		hud.Show();
		hud.FocusDefault();
	}
}
