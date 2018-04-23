using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NeverdawnInputDevice
{
    Joystick1,
    Joystick2,
    Joystick3,
    Joystick4,
    Keyboard
}

public enum NeverdawnInputButton
{
    Confirm,
    Cancel,
    Info,
    SwitchCharacter,
    Start,
    Settings,
    Left,
    Right,
}

public enum NeverdawnInputAxis
{
    HorizontalLeft,
    VerticalLeft,
    VerticalRight,
    HorizontalRight,
    HorizontalDPad,
    VerticalDPad,
    Trigger,
}

public enum NeverdawnInputAxisDirection
{
    Positive,
    Negative
}



public class InputModule  {

    private static string STRING_JOYSTICK = "Joystick";

    private static string STRING_BUTTON = "Button";

    private static string STRING_AXIS_HORIZONTAL_L = "Horizontal";

    private static string STRING_AXIS_VERTICAL_L = "Vertical";

    private static string STRING_AXIS_HORIZONTAL_R = "HorizontalRight";

    private static string STRING_AXIS_VERTICAL_R = "VerticalRight";

    private static string STRING_AXIS_HORIZONTAL_DPAD = "HorizontalDPad";

    private static string STRING_AXIS_VERTICAL_DPAD = "VerticalDPad";

    private static string STRING_AXIS_TRIGGER = "Trigger";

    private Dictionary<NeverdawnInputButton, KeyCode> buttonCodes;

    private Dictionary<string, string> defaultMappings;

    private Dictionary<NeverdawnInputAxis, string> axisMap;

    private NeverdawnInputDevice device;

    private Dictionary<NeverdawnInputAxis, float> previousValues;

    private Array values;

    public Vector3 cursorPosition { get; private set; }

    public InputModule(NeverdawnInputDevice device)
    {
        this.device = device;


        axisMap = new Dictionary<NeverdawnInputAxis, string>();
        buttonCodes = new Dictionary<NeverdawnInputButton, KeyCode>();
        previousValues = new Dictionary<NeverdawnInputAxis, float>();
        
        switch(device)
        {
            case NeverdawnInputDevice.Joystick1:
            case NeverdawnInputDevice.Joystick2:
            case NeverdawnInputDevice.Joystick3:
            case NeverdawnInputDevice.Joystick4:
                createDefaultJoystickMappings();
                setupButtons();
                setupAxis();
                setUpEvents();
                break;
            case NeverdawnInputDevice.Keyboard:
                createDefaultKeyboardMappings();
                setupButtons();
                setupAxis();
                break;
        }
    }



    private void setUpEvents()
    {
       
    }

    /// <summary>
    /// Call this every frame to access axis events
    /// </summary>
    public void UpdateModule()
    {
        foreach (NeverdawnInputAxis axis in values)
        {
            previousValues[axis] = GetAxis(axis);
        }

        if (device == NeverdawnInputDevice.Keyboard)
        {
            cursorPosition = Input.mousePosition;
        }
        else
        {
            cursorPosition = Vector3.zero;
        }
    }

    public override string ToString()
    {
        string result = string.Empty;

        result += "\nDOWN: "; 
        foreach (NeverdawnInputAxis axis in values)
        {
            result += GetAxisDown(axis, NeverdawnInputAxisDirection.Positive) + ", ";    
        }

        result += "\nUP: "; 

        foreach (NeverdawnInputAxis axis in values)
        {
            result += GetAxisUp(axis, NeverdawnInputAxisDirection.Positive) + ", ";
        }

        result += "\n";

        foreach(float f in previousValues.Values)
        {
            result += f.ToString() + ", ";    
        }

        return result;
    }

    private void setupAxis()
    {
        values = Enum.GetValues(typeof(NeverdawnInputAxis));

        foreach (NeverdawnInputAxis axis in values)
        {
            axisMap.Add(axis, getAxisCode(axis));
            previousValues.Add(axis, 0.0f);
        }
    }

    private string getAxisCode(NeverdawnInputAxis axis)
    {
        string prefString = string.Concat(device, axis);
        return PlayerPrefs.GetString(prefString, defaultMappings[prefString]);
    }

   
    private void setupButtons()
    {
        var values = Enum.GetValues(typeof(NeverdawnInputButton));

        foreach(NeverdawnInputButton button in values)
        {
            buttonCodes.Add(button, getButtonCode(button));
        }
    }

    private KeyCode getButtonCode(NeverdawnInputButton neverdawnInputButton)
    {
        string prefString = string.Concat(device, neverdawnInputButton);

        return (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(prefString, defaultMappings[prefString]));
    }

  

    public bool GetButtonDown(NeverdawnInputButton button)
    {
        return Input.GetKeyDown(buttonCodes[button]);
    }

    public bool GetButtonUp(NeverdawnInputButton button)
    {
        return Input.GetKeyUp(buttonCodes[button]);
    }

    public float GetAxis(NeverdawnInputAxis axis)
    {
        return Input.GetAxis(axisMap[axis]);
    }

    public bool GetAxisDown(NeverdawnInputAxis axis, NeverdawnInputAxisDirection direction)
    {
        if (direction == NeverdawnInputAxisDirection.Positive)
        {
            return GetAxis(axis) > 0.0f && previousValues[axis] == 0.0f;
        }
        else
        {
            return GetAxis(axis) < 0.0f && previousValues[axis] == 0.0f;
        }
    }

    public bool GetAxisUp(NeverdawnInputAxis axis, NeverdawnInputAxisDirection direction)
    {
        if (direction == NeverdawnInputAxisDirection.Positive)
        {
            return GetAxis(axis) == 0.0f && previousValues[axis] > 0.0f;
        }
        else
        {
            return GetAxis(axis) == 0.0f && previousValues[axis] < 0.0f;
        }
    }

    private void createDefaultKeyboardMappings()
    {
        defaultMappings = new Dictionary<string, string>();

        addDefaultKeyboardButtonMapping(NeverdawnInputButton.Confirm, "Mouse0");
        addDefaultKeyboardButtonMapping(NeverdawnInputButton.Cancel, "Mouse1");
        addDefaultKeyboardButtonMapping(NeverdawnInputButton.Info, "F");
        addDefaultKeyboardButtonMapping(NeverdawnInputButton.SwitchCharacter, "Tab");
        addDefaultKeyboardButtonMapping(NeverdawnInputButton.Left, "LeftArrow");
        addDefaultKeyboardButtonMapping(NeverdawnInputButton.Right, "RightArrow");
        addDefaultKeyboardButtonMapping(NeverdawnInputButton.Settings, "Escape");
        addDefaultKeyboardButtonMapping(NeverdawnInputButton.Start, "Space");

        addDefaultJoystickAxisMapping(NeverdawnInputAxis.HorizontalLeft, STRING_AXIS_HORIZONTAL_L);
        addDefaultJoystickAxisMapping(NeverdawnInputAxis.VerticalLeft, STRING_AXIS_VERTICAL_L);
        addDefaultJoystickAxisMapping(NeverdawnInputAxis.HorizontalRight, STRING_AXIS_HORIZONTAL_R);
        addDefaultJoystickAxisMapping(NeverdawnInputAxis.VerticalRight, STRING_AXIS_VERTICAL_R);
        addDefaultJoystickAxisMapping(NeverdawnInputAxis.HorizontalDPad, STRING_AXIS_HORIZONTAL_DPAD);
        addDefaultJoystickAxisMapping(NeverdawnInputAxis.VerticalDPad, STRING_AXIS_VERTICAL_DPAD);
        addDefaultJoystickAxisMapping(NeverdawnInputAxis.Trigger, STRING_AXIS_TRIGGER);
    }

    private void createDefaultJoystickMappings()
    {
        defaultMappings = new Dictionary<string, string>();

        addDefaultJoystickButtonMapping(NeverdawnInputButton.Confirm, 0);
        addDefaultJoystickButtonMapping(NeverdawnInputButton.Cancel, 1);
        addDefaultJoystickButtonMapping(NeverdawnInputButton.Info, 2);
        addDefaultJoystickButtonMapping(NeverdawnInputButton.SwitchCharacter, 3);
        addDefaultJoystickButtonMapping(NeverdawnInputButton.Left, 4);
        addDefaultJoystickButtonMapping(NeverdawnInputButton.Right, 5);
        addDefaultJoystickButtonMapping(NeverdawnInputButton.Settings, 6);
        addDefaultJoystickButtonMapping(NeverdawnInputButton.Start, 7);

        addDefaultJoystickAxisMapping(NeverdawnInputAxis.HorizontalLeft, STRING_AXIS_HORIZONTAL_L);
        addDefaultJoystickAxisMapping(NeverdawnInputAxis.VerticalLeft, STRING_AXIS_VERTICAL_L);
        addDefaultJoystickAxisMapping(NeverdawnInputAxis.HorizontalRight, STRING_AXIS_HORIZONTAL_R);
        addDefaultJoystickAxisMapping(NeverdawnInputAxis.VerticalRight, STRING_AXIS_VERTICAL_R);
        addDefaultJoystickAxisMapping(NeverdawnInputAxis.HorizontalDPad, STRING_AXIS_HORIZONTAL_DPAD);
        addDefaultJoystickAxisMapping(NeverdawnInputAxis.VerticalDPad, STRING_AXIS_VERTICAL_DPAD);
        addDefaultJoystickAxisMapping(NeverdawnInputAxis.Trigger, STRING_AXIS_TRIGGER);
    }

    private void addDefaultKeyboardAxisMapping(NeverdawnInputAxis neverdawnInputAxis, string axisName)
    {
        defaultMappings.Add(string.Concat(device, neverdawnInputAxis), string.Concat(device, axisName));
    }

    private void addDefaultKeyboardButtonMapping(NeverdawnInputButton button, string buttonName)
    {
        defaultMappings.Add(string.Concat(device, button), buttonName);
    }

    private void addDefaultJoystickAxisMapping(NeverdawnInputAxis neverdawnInputAxis, string axisName)
    {
        defaultMappings.Add(string.Concat(device, neverdawnInputAxis), string.Concat(device, axisName));
    }

    private void addDefaultJoystickButtonMapping(NeverdawnInputButton button, int targetIndex)
    {
        defaultMappings.Add(string.Concat(device, button), string.Concat(device, STRING_BUTTON, targetIndex));
    }

    public Vector3 normalizedDirection
    {
        get
        {
            return new Vector3(GetAxis(NeverdawnInputAxis.HorizontalLeft), 0.0f, GetAxis(NeverdawnInputAxis.VerticalLeft));
        }
    }

    public bool HasCursor
    {
        get { return device == NeverdawnInputDevice.Keyboard; }
    }
}
