using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu Objects")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _mainPauseMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _optionsButton;
    [SerializeField] private GameObject _quitButton;
    
    [Header("Scripts to Deactivate When Pausing")]
    [SerializeField] private PlayerMove _playerScript;

    [Header("Other")]
    [SerializeField] private EventSystem _eventSystem;

    public static bool gamePaused = false;  // Static variable to use here and in other classes
    private bool _pressedPause;  // For use with CustomInputManager
    private bool _isOnMainPauseMenu = true;

    void Awake()
    {
        _playerScript.enabled = true;
        Time.timeScale = 1.0f;
        gamePaused = false;
    }

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
                _eventSystem.SetSelectedGameObject(_optionsButton);
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
        _optionsMenu.SetActive(false);
        _isOnMainPauseMenu = true;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("Title Screen");
    }
}
