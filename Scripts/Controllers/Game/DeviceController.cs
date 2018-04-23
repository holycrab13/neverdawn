using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;

/// <summary>
/// Controls the input devices, checks if any device "Start" is pressed and creates a new avatar controller
/// </summary>
public class DeviceController : MonoBehaviour
{
    public delegate void InputModuleEvent(InputModule module);

    public static event InputModuleEvent onModuleActivated;
    
    private static List<InputModule> allInputModules;

    private static List<InputModule> availableInputModules;

    private static List<InputModule> activeInputModules;

    public static InputModule[] inputModules {get; set;}

    private static DeviceController instance;

    void Awake()
    { 
        // Never allow more than one game controller
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        allInputModules = new List<InputModule>();
        availableInputModules = new List<InputModule>();
        activeInputModules = new List<InputModule>();

        allInputModules.Add(new InputModule(NeverdawnInputDevice.Joystick1));
        allInputModules.Add(new InputModule(NeverdawnInputDevice.Joystick2));
        allInputModules.Add(new InputModule(NeverdawnInputDevice.Joystick3));
        allInputModules.Add(new InputModule(NeverdawnInputDevice.Joystick4));
        allInputModules.Add(new InputModule(NeverdawnInputDevice.Keyboard));

        availableInputModules.AddRange(allInputModules);

        inputModules = activeInputModules.ToArray();
    }

    void Update()
    {
        for (int i = 0; i < availableInputModules.Count; i++)
        {
            if (availableInputModules[i].GetButtonDown(NeverdawnInputButton.Start))
            {
                activeInputModules.Add(availableInputModules[i]);
                inputModules = activeInputModules.ToArray();

                if (onModuleActivated != null)
                {
                    onModuleActivated.Invoke(availableInputModules[i]);
                }

                availableInputModules.RemoveAt(i);
            }
        }
    }



    void LateUpdate()
    {
        foreach (InputModule module in activeInputModules)
        {
            module.UpdateModule();
        }
    }
}
