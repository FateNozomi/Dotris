using System;

namespace Dotris.Game.Inputs;

public class InputCommand
{
    private bool _repeat;

    private double _totalDelta;

    private bool _pressed;
    private bool _dasHandled;

    public InputCommand(bool repeat = true)
    {
        _repeat = repeat;
    }

    public event EventHandler Executed;
    public event EventHandler JustReleased;

    public void Register(double delta, int arr, int das, int dcd)
    {
        if (_pressed)
        {
            if (_repeat)
            {
                if (_dasHandled)
                {
                    ExecuteAfterDelay(delta, arr);
                }
                else
                {
                    if (ExecuteAfterDelay(delta, das))
                    {
                        _dasHandled = true;
                    }
                }
            }
        }
        else
        {
            _pressed = true;
            Executed?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Unregister()
    {
        _totalDelta = 0;

        _pressed = false;
        _dasHandled = false;
        JustReleased?.Invoke(this, EventArgs.Empty);
    }

    private bool ExecuteAfterDelay(double delta, int frameDelay)
    {
        _totalDelta += delta;
        if (_totalDelta >= frameDelay * (1 / 60f))
        {
            Executed?.Invoke(this, EventArgs.Empty);
            _totalDelta = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
}