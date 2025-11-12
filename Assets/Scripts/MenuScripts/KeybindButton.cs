using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections;

public class KeybindButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    [SerializeField] private GameObject _keybindMenu;
    private Button _buttonSelected;
    private string _keyMapName;
    public static bool isMakingInput = false;

    void Start()
    {
        
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
            }
        }
    }
}
