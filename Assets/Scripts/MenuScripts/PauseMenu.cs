using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu Objects")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _mainPauseMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _quitButton;
    [Header("Scripts to Deactivate When Pausing")]
    [SerializeField] private PlayerMove _playerScript;
    [SerializeField] private EventSystem _eventSystem;

    public static bool gamePaused = false;
    private bool _pressedPause;
    private bool _isOnMainPauseMenu = true;

    // Update is called once per frame
    void Update()
    {
        if (!KeybindButton.isMakingInput)
            _pressedPause = CustomInputManager.GetKeyJustPressed("Pause1") || CustomInputManager.GetKeyJustPressed("Pause2") || CustomInputManager.GetKeyJustPressed("Pause3");

        if (_pressedPause)
        {
            if (_isOnMainPauseMenu)
            {
                gamePaused = !gamePaused;
                _playerScript.enabled = !_playerScript.enabled;
                _isOnMainPauseMenu = true;
                _pauseMenu.SetActive(gamePaused);
                _mainPauseMenu.SetActive(gamePaused);
                _optionsMenu.SetActive(false);
                Time.timeScale = 0.0f;

                if (!gamePaused)
                {
                    Time.timeScale = 1.0f;
                }
            }
            else
            {
                BackButton();
            }
            
        }
    }

    public void ResumeButton()
    {
        gamePaused = false;
        _playerScript.enabled = true;
        _pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void OptionsButton()
    {
        _mainPauseMenu.SetActive(false);
        _optionsMenu.SetActive(true);
        _isOnMainPauseMenu = false;
    }
    
    public void BackButton()
    {
        _mainPauseMenu.SetActive(true);
        _eventSystem.SetSelectedGameObject(_optionsMenu);
        _optionsMenu.SetActive(false);
        _isOnMainPauseMenu = true;
    }
}
