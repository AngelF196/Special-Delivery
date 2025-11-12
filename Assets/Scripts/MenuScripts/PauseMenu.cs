using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _mainPauseMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _quitButton;
    [SerializeField] private EventSystem _eventSystem;
    private bool _gamePaused = false;
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
                _gamePaused = !_gamePaused;
                _isOnMainPauseMenu = true;
                _pauseMenu.SetActive(_gamePaused);
                _mainPauseMenu.SetActive(_gamePaused);
                _optionsMenu.SetActive(false);
                Time.timeScale = 0.0f;

                if (!_gamePaused)
                {
                    Time.timeScale = 1.0f;
                }
            }
            else
            {
                _eventSystem.SetSelectedGameObject(_optionsMenu.transform.Find("BackButton").gameObject);
            }
        }
    }

    public void ResumeButton()
    {
        _gamePaused = false;
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
        _optionsMenu.SetActive(false);
        _isOnMainPauseMenu = true;
    }
}
