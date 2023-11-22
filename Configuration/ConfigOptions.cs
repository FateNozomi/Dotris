using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Dotris.Game.Inputs;

namespace Dotris.Configuration;

public class ConfigOptions
{
    private readonly string _filePath = "user://config.ini";
    private ConfigFile _config = new();

    public ConfigOptions()
    {
        GetInputMap();
    }

    public Dictionary<string, string> ControlKeyBindings { get; } = new Dictionary<string, string>();

    public void Load()
    {
        Error err = _config.Load(_filePath);
        if (err != Error.Ok)
            return;

        foreach (var key in ControlKeyBindings.Keys)
        {
            var value = (string)_config.GetValue("Controls", key);
            if (value != "")
            {
                ControlKeyBindings[key] = value;
            }
        }
    }

    public void Save()
    {
        foreach (var controlKeyBinding in ControlKeyBindings)
        {
            _config.SetValue("Controls", controlKeyBinding.Key, controlKeyBinding.Value);
        }

        _config.Save(_filePath);
    }

    public void GetInputMap()
    {
        foreach (var inputControl in (InputControls[])Enum.GetValues(typeof(InputControls)))
        {
            var aes = InputMap.ActionGetEvents(inputControl.ToString());
            var inputEventKey = aes.OfType<InputEventKey>().First();
            if (ControlKeyBindings.ContainsKey(inputControl.ToString()))
            {
                ControlKeyBindings[inputControl.ToString()] = inputEventKey.AsText();
            }
            else
            {
                ControlKeyBindings.Add(inputControl.ToString(), inputEventKey.AsText());
            }
        }
    }

    public void SetInputMap()
    {
        foreach (var controlKeyBinding in ControlKeyBindings)
        {
            InputEventKey iek = new();
            iek.Keycode = OS.FindKeycodeFromString(controlKeyBinding.Value);

            InputMap.ActionEraseEvents(controlKeyBinding.Key);
            InputMap.ActionAddEvent(controlKeyBinding.Key, iek);
        }
    }

}