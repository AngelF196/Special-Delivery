using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KeybindButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    [SerializeField] private GameObject _keybindMenu;
    private Button _buttonSelected;
    private string _keyMapName;
    private List<Button> _keyboardButtons = new List<Button>();
    private List<Button> _controllerButtons = new List<Button>();
    public static bool isMakingInput = false;


    void Start()
    {
        Button[] buttons = _keybindMenu.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            if (button.gameObject.name.Contains("Key") && !button.gameObject.name.Contains("Default"))
                _keyboardButtons.Add(button);
            else if (button.gameObject.name.Contains("Cont") && !button.gameObject.name.Contains("Default"))
                _controllerButtons.Add(button);
        }
        LoadUserSetKeybinds();
    }

    void Update()
    {
        if (buttonText.text == "Set New Input")
        {
            isMakingInput = true;
            StartCoroutine(KeyPressInput()); 
        }
    }

    public void EditButtonByName(string searchButtonName)
    {
        Button[] buttons = _keybindMenu.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            if (button.gameObject.name == searchButtonName)
            {
                _buttonSelected = button;
                break;
            }
        }
        buttonText = _buttonSelected.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetNewKeybind(string keybindName)
    {
        buttonText.text = "Set New Input";
        _keyMapName = keybindName;
    }

    public void ResetKeyboardKeybindsToDefault()
    {
        Dictionary<string, KeyCode> defaultMapping = new Dictionary<string, KeyCode>(CustomInputManager.keyMapping);
        KeyCode[] defaultKeys = CustomInputManager.GetDefaultKeys();
        int i = 0;

        foreach(string keyName in CustomInputManager.keyMapping.Keys)
        {
            buttonText = _keyboardButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            defaultMapping[keyName] = defaultKeys[i];
            buttonText.text = defaultKeys[i].ToString();
            PlayerPrefs.SetInt(keyName, (int) defaultKeys[i]);
            PlayerPrefs.Save();
            i++;
        }
        CustomInputManager.keyMapping = defaultMapping;
    }

    // When the game loads, load up all of the user set keybinds
    public void LoadUserSetKeybinds()
    {
        Button[] buttons = _keybindMenu.GetComponentsInChildren<Button>();
        Dictionary<string, KeyCode> userMapping = new Dictionary<string, KeyCode>(CustomInputManager.keyMapping);
        int i = 0;

        foreach(string keyName in CustomInputManager.keyMapping.Keys)
        {
            buttonText = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            userMapping[keyName] = (KeyCode) PlayerPrefs.GetInt(keyName);
            buttonText.text = ((KeyCode) PlayerPrefs.GetInt(keyName)).ToString();
            i++;
        }
        CustomInputManager.keyMapping = userMapping;
    }

    // Coroutine method:
    // This is to avoid the player from exiting out of the pause menu after setting a keybind for pausing the game
    IEnumerator NotMakingInput()
    {
        yield return null;
        isMakingInput = false;
    }
    
    // Coroutine method:
    // This one is to avoid setting the keybind right after the player wants to set a new keybind by pressing any confirm key
    IEnumerator KeyPressInput()
    {
        yield return null;
        foreach(KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keycode))
            {
                buttonText.text = keycode.ToString();
                CustomInputManager.SetKeyMap(_keyMapName, keycode);
                StartCoroutine(NotMakingInput());
                PlayerPrefs.SetInt(_keyMapName, (int) keycode);
                PlayerPrefs.Save();
            }
        }
    }
}
