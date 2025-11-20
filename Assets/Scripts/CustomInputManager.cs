using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CustomInputManager : MonoBehaviour
{
    // Public dictionary only for reading; DO NOT CHANGE THE DAMN DICTIONARY AT FREAKIN' RUNTIME UNLESS YOU'RE USING THE SetKeyMap() METHOD!!!!
    public static Dictionary<string, KeyCode> keyMapping;
    // If you wanna change the dictionary keys and values, do so in here, NOT AT DAMN RUNTIME!!!!!!!!!!!!!
    private static string[] keyMaps = new string[9]
    {
        "Left1",
        "Left2",
        "Right1",
        "Right2",
        "Jump",
        "Airflip/Dive",
        "Pause1",
        "Pause2",
        "Pause3"
    };
    private static KeyCode[] defaults = new KeyCode[9]
    {
        KeyCode.A,
        KeyCode.LeftArrow,
        KeyCode.D,
        KeyCode.RightArrow,
        KeyCode.Space,
        KeyCode.LeftShift,
        KeyCode.Return,
        KeyCode.Backspace,
        KeyCode.Escape
    };

    private static Dictionary<string, float> inputAxes;
    private static string[] axisNames = new string[2]
    {
        "Horizontal",
        "Vertical"
    };
    private static float[] axisValues = new float[2]
    {
        0.0f,
        0.0f
    };

    static CustomInputManager()
    {
        InitializeDictionaries();
    }

    private static void InitializeDictionaries()
    {
        keyMapping = new Dictionary<string, KeyCode>();
        for(int i = 0; i < keyMaps.Length; ++i)
        {
            keyMapping.Add(keyMaps[i], defaults[i]);
        }
        inputAxes = new Dictionary<string, float>();
        for(int i = 0; i < axisNames.Length; ++i)
        {
            inputAxes.Add(axisNames[i], axisValues[i]);
        }
    }

    public static void SetKeyMap(string keyMap,KeyCode key)
    {
        if (!keyMapping.ContainsKey(keyMap))
            throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap);
        keyMapping[keyMap] = key;
    }

    // Keyboard inputs
    public static bool GetKeyJustPressed(string keyMap)
    {
        return Input.GetKeyDown(keyMapping[keyMap]);
    }

    public static bool GetKeyPressed(string keyMap)
    {
        return Input.GetKey(keyMapping[keyMap]);
    }

    public static bool GetKeyReleased(string keyMap)
    {
        return Input.GetKeyUp(keyMapping[keyMap]);
    }

    // Input Axis stuff
    public static float GetRawAxis(string axisName)
    {
        switch(axisName)
        {
            case "Horizontal":
                if (GetKeyPressed("Left1") || GetKeyPressed("Left2"))
                    inputAxes[axisName] = -1;
                else if (GetKeyPressed("Right1") || GetKeyPressed("Right2"))
                    inputAxes[axisName] = 1;
                else
                    inputAxes[axisName] = 0;
                break;
        }
        return inputAxes[axisName];
    }

    public static float GetAxis(string axisName)
    {
        float sensitivity = 3.0f;
        switch(axisName)
        {
            case "Horizontal":
                if (GetKeyPressed("Left1") || GetKeyPressed("Left2"))
                    inputAxes[axisName] = Mathf.MoveTowards(inputAxes[axisName], -1, Time.deltaTime * sensitivity);
                else if (GetKeyPressed("Right1") || GetKeyPressed("Right2"))
                    inputAxes[axisName] = Mathf.MoveTowards(inputAxes[axisName], 1, Time.deltaTime * sensitivity);
                else
                    inputAxes[axisName] = Mathf.MoveTowards(inputAxes[axisName], 0, Time.deltaTime * sensitivity);
                break;
        }
        return inputAxes[axisName];
    }

    // All other methods
    public static KeyCode[] GetDefaultKeys()
    {
        return defaults;
    }
}

