using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEditor.PackageManager;
using UnityEngine.EventSystems;

public class KeybindButton : MonoBehaviour
{
    // Using new input system stuff
    public InputActionAsset playerInputActions;
    private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;
    private InputAction _moveAction;
    private InputAction _singleButtonAction;
    private Button _buttonSelected;
    private TextMeshProUGUI buttonText;
    [SerializeField] private GameObject _keybindMenu;
    private EventSystem _eventSystem;

    // Old stuff

    // Button lists for rebinding
    private List<Button> _keyboardButtons = new List<Button>();
    private List<Button> _controllerButtons = new List<Button>();

    void Awake()
    {
        _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        _moveAction = playerInputActions.FindAction("Move");

        string rebinds = PlayerPrefs.GetString("rebinds");
        // Debug.Log(rebinds);
        if (!string.IsNullOrEmpty(rebinds))
        {
            playerInputActions.LoadBindingOverridesFromJson(rebinds);
        }
    }
    
    void Start()
    {
        // Get a list of all buttons availble for rebinding
        Button[] buttons = _keybindMenu.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            if (button.gameObject.name.Contains("Key") && !button.gameObject.name.Contains("Default"))
                _keyboardButtons.Add(button);
            else if (button.gameObject.name.Contains("Cont") && !button.gameObject.name.Contains("Default"))
                _controllerButtons.Add(button);
        }
        // LoadUserSetKeybinds();
    }

    
    // Searches for a given button name within the list of buttons, 
    public void EditButtonByName(string searchButtonName)
    {
        Button[] buttons = _keybindMenu.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            if (button.gameObject.name == searchButtonName)
            {
                _buttonSelected = button;
                playerInputActions.FindActionMap("Player").Disable();
                break;
            }
        }
        buttonText = _buttonSelected.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Set New Input";
    }

    public void SetNewKeybind(string actionName)
    {        
        playerInputActions.FindActionMap("Player").Disable();
        _buttonSelected.interactable = false;
        
        if ( actionName.Contains("Left") || actionName.Contains("Right") )
        {
            InputActionSetupExtensions.BindingSyntax movementKeys = _moveAction.ChangeCompositeBinding("WASD");  // Gets the composite named "WASD", which contains all the movement key bindings
            int keyBindIndex = -1;
            switch (actionName)
            {
                case "Left1":
                    keyBindIndex = movementKeys.NextPartBinding("Left").bindingIndex;  // First left binding
                    break;
                case "Left2":
                    keyBindIndex = movementKeys.NextPartBinding("Left").bindingIndex + 1;  // Second left binding
                    break;
                case "Right1":
                    keyBindIndex = movementKeys.NextPartBinding("Right").bindingIndex;  // First right binding
                    break;
                case "Right2":
                    keyBindIndex = movementKeys.NextPartBinding("Right").bindingIndex + 1;  // Second right binding
                    break;
            }
            _rebindingOperation = _moveAction.PerformInteractiveRebinding(keyBindIndex).OnComplete(operation => KeyRebindingComplete(actionName, keyBindIndex));
        }
        else
        {
            _singleButtonAction = playerInputActions.FindAction(actionName);
            _rebindingOperation = _singleButtonAction.PerformInteractiveRebinding(1).OnComplete(operation => KeyRebindingComplete(actionName));
        }
        
        _rebindingOperation.Start();
    }

    private void KeyRebindingComplete(string actionName, int compKeybindIndex = -1)
    {
        _rebindingOperation.Dispose();
        _buttonSelected.interactable = true;
        _eventSystem.SetSelectedGameObject(_buttonSelected.gameObject);

        string newBinding;
        if ( actionName.Contains("Left") || actionName.Contains("Right") )
            newBinding = _moveAction.bindings[compKeybindIndex].effectivePath;
        else
            newBinding = _singleButtonAction.bindings[1].effectivePath;
        
        // Formatting of the binding path to just show the key pressed
        short index = (short) newBinding.Length;
        while (newBinding[index-1] != '/')
            index--;
        newBinding = newBinding[index..].ToUpper();
        buttonText.text = newBinding;
        
        playerInputActions.FindActionMap("Player").Enable();

        // Save keybind afterwards
        string rebinds = playerInputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }

    public void SetNewControllerBind()
    {
        playerInputActions.FindActionMap("Player").Disable();
        _buttonSelected.interactable = false;
    }

    private void ControllerRebindingComplete()
    {
        _rebindingOperation.Dispose();
        _buttonSelected.interactable = true;
    }

    // // Reload everything in this section later
    // public void ResetKeyboardKeybindsToDefault()
    // {
    //     Dictionary<string, KeyCode> defaultMapping = new Dictionary<string, KeyCode>(CustomInputManager.keyMapping);
    //     KeyCode[] defaultKeys = CustomInputManager.GetDefaultKeys();
    //     int i = 0;

    //     foreach(string keyName in CustomInputManager.keyMapping.Keys)
    //     {
    //         buttonText = _keyboardButtons[i].GetComponentInChildren<TextMeshProUGUI>();
    //         defaultMapping[keyName] = defaultKeys[i];
    //         buttonText.text = defaultKeys[i].ToString();
    //         PlayerPrefs.SetInt(keyName, (int) defaultKeys[i]);
    //         PlayerPrefs.Save();
    //         i++;
    //     }
    //     CustomInputManager.keyMapping = defaultMapping;
    // }

    // When the game loads, load up all of the user set keybinds
    // public void LoadUserSetKeybinds()
    // {
    //     Button[] buttons = _keybindMenu.GetComponentsInChildren<Button>();
    //     Dictionary<string, KeyCode> userMapping = new Dictionary<string, KeyCode>(CustomInputManager.keyMapping);
    //     int i = 0;

    //     foreach(string keyName in CustomInputManager.keyMapping.Keys)
    //     {
    //         buttonText = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
    //         userMapping[keyName] = (KeyCode) PlayerPrefs.GetInt(keyName);
    //         buttonText.text = ((KeyCode) PlayerPrefs.GetInt(keyName)).ToString();
    //         i++;
    //     }
    //     CustomInputManager.keyMapping = userMapping;
    // }
}
