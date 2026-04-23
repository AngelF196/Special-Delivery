using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    private PlayerInput _playerInput;


    public static bool gamePaused = false;  // Static variable to use here and in other classes
    private bool _pressedPause;  // For use with CustomInputManager
    private bool _isOnMainPauseMenu = true;

    // Events
    public UnityEvent gamePausedEvent;
    public UnityEvent gameResumedEvent;

    void Awake()
    {
        _playerScript.enabled = true;
        Time.timeScale = 1.0f;
        gamePaused = false;
    }

    void Start()
    {
        _playerInput = GameObject.Find("player").GetComponent<PlayerInput>();
        _playerInput.playerPause.AddListener(respondToPause);
        _playerScript = GameObject.Find("player").GetComponent<PlayerMove>();
        gamePausedEvent.AddListener(GameObject.Find("GameManager").GetComponent<GameManager>().GamePaused);
        gameResumedEvent.AddListener(GameObject.Find("GameManager").GetComponent<GameManager>().GameResumed);
    }

    private void respondToPause()
    {
        if (_isOnMainPauseMenu)
        {
            gamePaused = !gamePaused;
            _playerScript.enabled = !_playerScript.enabled;
            _isOnMainPauseMenu = true;
            _pauseMenu.SetActive(gamePaused);
            _mainPauseMenu.SetActive(gamePaused);
            _optionsMenu.SetActive(false);

            if (gamePaused)
            {
                gamePausedEvent.Invoke();
                Time.timeScale = 0.0f;
            }
            else
            {
                Time.timeScale = 1.0f;
                gameResumedEvent.Invoke();
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
