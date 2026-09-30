using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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
    [SerializeField] private PlayerInput _playerInputScript;

    [Header("Other")]
    [SerializeField] private EventSystem _eventSystem;
    private PlayerInput _playerInput;


    public static bool gamePaused = false;  // Static variable to use here and in other classes
    private bool _isOnMainPauseMenu = true;

    // Events
    public UnityEvent gamePausedEvent;
    public UnityEvent gameResumedEvent;

    void Awake()
    {
        _playerInputScript.enabled = true;
        Time.timeScale = 1.0f;
        gamePaused = false;
    }

    void Start()
    {
        _playerInput = Locator.Instance.PlayerInput;
        _playerInput.playerPause.AddListener(respondToPause);
        gameResumedEvent.AddListener(_playerInput.TimerReset);
        gamePausedEvent.AddListener(Locator.Instance.GameManager.GamePaused);
        gameResumedEvent.AddListener(Locator.Instance.GameManager.GameResumed);
    }

    private void respondToPause()
    {
        if (_isOnMainPauseMenu)
        {
            // Pop up pause menu
            gamePaused = !gamePaused;
            _playerInputScript.enabled = !_playerInputScript.enabled;
            _isOnMainPauseMenu = true;
            _pauseMenu.SetActive(gamePaused);
            _mainPauseMenu.SetActive(gamePaused);
            _optionsMenu.SetActive(false);

            if (gamePaused)
            {
                gamePausedEvent.Invoke();
                Time.timeScale = 0.0f;
                _playerInput.enabled = false;
            }
            else
            {
                gameResumedEvent.Invoke();
                _playerInput.enabled = true;

            }
        }
        else
        {
            BackButton();
            _eventSystem.SetSelectedGameObject(_optionsButton);
        }
    }

    public void ResumeButton()
    {
        gamePaused = false;
        _playerInputScript.enabled = true;
        _pauseMenu.SetActive(false);
        gameResumedEvent.Invoke();
        _playerInput.enabled = true;

    }

    public void OptionsButton()
    {
        _mainPauseMenu.SetActive(false);
        _optionsMenu.SetActive(true);
        _isOnMainPauseMenu = false;
    }
    
    public void SaveGameButton()
    {
        Locator.Instance.SaveFileManager.SaveButtonPressed();
    }

    public void BackButton()
    {
        _mainPauseMenu.SetActive(true);
        _optionsMenu.SetActive(false);
        _isOnMainPauseMenu = true;
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1.0f;
        Destroy(PlayerInfo.Instance.gameObject);
        SceneManager.LoadScene("Title Screen");
    }
}
