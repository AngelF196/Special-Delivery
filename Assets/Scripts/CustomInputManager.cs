using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CustomInputManager : MonoBehaviour
{
    static Dictionary<string, KeyCode> keyMapping;
    static string[] keyMaps = new string[9]
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

    // Default keyboard controls; DO NOT MODIFY DURING RUNTIME!!
    static KeyCode[] defaults = new KeyCode[9]
    {
        KeyCode.A,
        KeyCode.LeftArrow,
        KeyCode.D,
        KeyCode.RightArrow,
        KeyCode.Space,
        KeyCode.LeftShift,
        KeyCode.KeypadEnter,
        KeyCode.Backspace,
        KeyCode.Escape
    };

    static KeyCode[] preferences = defaults;

    static CustomInputManager()
    {
        InitializeDictionary();
    }

    private static void InitializeDictionary()
    {
        keyMapping = new Dictionary<string, KeyCode>();
        for(int i=0;i<keyMaps.Length;++i)
        {
            keyMapping.Add(keyMaps[i], preferences[i]);
        }
    }

    public static void SetKeyMap(string keyMap,KeyCode key)
    {
        if (!keyMapping.ContainsKey(keyMap))
            throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap);
        keyMapping[keyMap] = key;
    }

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


}
