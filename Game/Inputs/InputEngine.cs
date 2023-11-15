using Godot;

namespace Dotris.Game.Inputs;

public class InputEngine
{
	/// <summary>
	/// Automatic Repeat Rate
	/// </summary>
	public int ARR { get; set; }

	/// <summary>
	/// Delay Auto Shift
	/// </summary>
	public int DAS { get; set; }

	/// <summary>
	/// DAS Cut Delay
	/// </summary>
	public int DCD { get; set; }

	public InputCommand LeftCommand { get; } = new InputCommand();
	public InputCommand SoftDropCommand { get; } = new InputCommand();
	public InputCommand UpCommand { get; } = new InputCommand();
	public InputCommand RightCommand { get; } = new InputCommand();
	public InputCommand HardDropCommand { get; } = new InputCommand(repeat: false);
	public InputCommand RotateCounterclockwiseCommand { get; } = new InputCommand(repeat: false);
	public InputCommand RotateClockwiseCommand { get; } = new InputCommand(repeat: false);
	public InputCommand HoldCommand { get; } = new InputCommand(repeat: false);

	public void Register(InputControls controls, double delta)
	{
		InputCommand command = GetCommand(controls);
		command?.Register(delta, ARR, DAS, DCD);
	}

	public void Unregister(InputControls controls)
	{
		InputCommand command = GetCommand(controls);
		command?.Unregister();
	}

	private InputCommand GetCommand(InputControls controls)
	{
		InputCommand command;
		switch (controls)
		{
			case InputControls.Left:
				command = LeftCommand;
				break;
			case InputControls.SoftDrop:
				command = SoftDropCommand;
				break;
			case InputControls.Up:
				command = UpCommand;
				break;
			case InputControls.Right:
				command = RightCommand;
				break;
			case InputControls.HardDrop:
				command = HardDropCommand;
				break;
			case InputControls.RotateCounterclockwise:
				command = RotateCounterclockwiseCommand;
				break;
			case InputControls.RotateClockwise:
				command = RotateClockwiseCommand;
				break;
			case InputControls.Hold:
				command = HoldCommand;
				break;
			default:
				command = null;
				break;
		}

		return command;
	}
}