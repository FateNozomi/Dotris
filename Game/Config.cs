using Godot;
using System;
using System.Linq;
using Dotris.Game.Inputs;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class Config : CanvasLayer
{
	private List<InputControlConfig> _inputControlConfigs = new();
	private TaskCompletionSource<InputEvent> _inputEventTCS;

	public Config()
	{
		foreach (var inputControl in (InputControls[])Enum.GetValues(typeof(InputControls)))
		{
			var inputControlConfig = new InputControlConfig(inputControl.ToString());
			_inputControlConfigs.Add(inputControlConfig);
			inputControlConfig.PollInput += (s, e) =>
			{
				ClearInputEventTCS();
				_inputEventTCS = e.InputEventTCS;
			};
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var vBoxContainer = GetNode<VBoxContainer>("ScrollContainer/VBoxContainer");
		foreach (var inputControlConfig in _inputControlConfigs)
		{
			vBoxContainer.AddChild(inputControlConfig.Node);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey && _inputEventTCS != null)
		{
			if (eventKey.Keycode == Key.Escape)
			{
				ClearInputEventTCS();
				GetViewport().SetInputAsHandled();
			}
			else
			{
				_inputEventTCS.TrySetResult(@event);
				_inputEventTCS = null;
				GetViewport().SetInputAsHandled();
			}
		}
	}

	public void FocusDefault()
	{
		var backButton = GetNode<Button>("MarginContainer/BackButton");
		backButton.GrabFocus();
	}

	private void OnBackButtonPressed()
	{
		ClearInputEventTCS();

		Hide();
		var hud = GetParent().GetNode<HUD>("HUD");
		hud.Show();
		hud.FocusDefault();
	}

	private void ClearInputEventTCS()
	{
		_inputEventTCS?.TrySetResult(null);
		_inputEventTCS = null;
	}

	private class InputControlConfig
	{
		public InputControlConfig(string inputAction)
		{
			InputAction = inputAction;

			var hBoxContainer = new HBoxContainer();
			var marginContainer = new MarginContainer();
			marginContainer.AddThemeConstantOverride("margin_top", 5);
			marginContainer.AddThemeConstantOverride("margin_left", 5);
			marginContainer.AddThemeConstantOverride("margin_bottom", 5);
			marginContainer.AddThemeConstantOverride("margin_right", 5);

			var controlLabel = new Label { Text = InputAction };
			var assignButton = new Button { Text = GetActionEventAsText() };

			assignButton.Pressed += async () =>
			{
				var inputEventTCS = new TaskCompletionSource<InputEvent>();
				PollInput?.Invoke(this, new PollInputEventArgs(inputEventTCS));
				assignButton.Text = "[Press Any Key...]";

				InputEvent newInputEvent = await inputEventTCS.Task;
				if (newInputEvent != null)
				{
					InputMap.ActionEraseEvents(InputAction);
					InputMap.ActionAddEvent(InputAction, newInputEvent);
					assignButton.Text = newInputEvent.AsText();
				}
				else
				{
					assignButton.Text = GetActionEventAsText();
				}
			};

			hBoxContainer.AddChild(controlLabel);
			marginContainer.AddChild(assignButton);
			hBoxContainer.AddChild(marginContainer);
			Node = hBoxContainer;
		}

		public event EventHandler<PollInputEventArgs> PollInput;

		public string InputAction { get; }

		public Node Node { get; }

		private string GetActionEventAsText()
		{
			var events = InputMap.ActionGetEvents(InputAction);
			InputEvent inputEvent = events.FirstOrDefault();
			return inputEvent != null ? inputEvent.AsText() : "Unassigned";
		}
	}

	private class PollInputEventArgs : EventArgs
	{
		public PollInputEventArgs(TaskCompletionSource<InputEvent> inputEventTCS)
		{
			InputEventTCS = inputEventTCS;
		}

		public TaskCompletionSource<InputEvent> InputEventTCS { get; }
	}
}
