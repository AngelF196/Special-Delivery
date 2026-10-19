using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class KeybindButton : MonoBehaviour
{
    // Using new input system stuff
    public InputActionAsset playerInputActions;
    private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;
    private InputAction _moveAction;
    private InputAction _singleButtonAction;
    private Button _buttonComponent;
    private TextMeshProUGUI _buttonText;
    private EventSystem _eventSystem;

    void Awake()
    {
        _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        _moveAction = playerInputActions.FindAction("Move");
    }
    
    void Start()
    {
        _buttonComponent = GetComponent<Button>();
        _buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void EditButtonText()
    {
        _buttonText.text = "Set New Input";
    }

    public void SetNewKeybind(string actionName)
    {
        _buttonComponent.interactable = false;
        
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
            _rebindingOperation = _moveAction.PerformInteractiveRebinding(keyBindIndex)
                .WithControlsExcluding("<Gamepad>/")
                .OnComplete(operation => KeyRebindingComplete(actionName, keyBindIndex));
        }
        else
        {
            _singleButtonAction = playerInputActions.FindAction(actionName);
            _rebindingOperation = _singleButtonAction.PerformInteractiveRebinding(1)
                .WithControlsExcluding("<Gamepad>/")
                .OnComplete(operation => KeyRebindingComplete(actionName));
        }
        
        _rebindingOperation.Start();
    }

    private void KeyRebindingComplete(string actionName, int compKeybindIndex = -1)
    {
        _rebindingOperation.Dispose();
        _buttonComponent.interactable = true;
        _eventSystem.SetSelectedGameObject(gameObject);

        string newBinding;
        if ( actionName.Contains("Left") || actionName.Contains("Right") )
            newBinding = _moveAction.GetBindingDisplayString(compKeybindIndex);
        else
            newBinding = _singleButtonAction.GetBindingDisplayString(1);
        
        _buttonText.text = newBinding;

        // Save keybind afterwards
        string rebinds = playerInputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }

    public void SetNewControllerBind(string actionName)
    {
        _buttonComponent.interactable = false;

        _singleButtonAction = playerInputActions.FindAction(actionName);
        _rebindingOperation = _singleButtonAction.PerformInteractiveRebinding(0)
            .WithControlsExcluding("<KeyBoard>/").WithControlsExcluding("<Mouse>/")
            .OnComplete(operation => ControllerRebindingComplete());
        _rebindingOperation.Start();
    }

    private void ControllerRebindingComplete()
    {
        _rebindingOperation.Dispose();
        _buttonComponent.interactable = true;
        _eventSystem.SetSelectedGameObject(gameObject);

        // Not worrying about gamepad joysticks, so I'm just doing single button actions
        _buttonText.text = _singleButtonAction.GetBindingDisplayString(0);

        // Save keybind afterwards
        string rebinds = playerInputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }
}
